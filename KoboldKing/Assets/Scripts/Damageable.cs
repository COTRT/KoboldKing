using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Events;
using System;

public class Damageable : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public DamageableType damageableType;
    public DamageableDeadAction deathAction;
    public RectTransform healthBarForeground;
    public RectTransform healthBarBackground;
    public bool useHealthBar;

    public Dictionary<DamageType, float> DamageTypeMultipliers;
    //This DamageTypeMultiplierListItem monkey business is here to let you edit the above dicitonary in the Unity Inspector.
    public List<DamageTypeMultiplierListItem> DamageTypeMultiplierList;
    [System.Serializable]
    public class DamageTypeMultiplierListItem
    {
        public DamageType damageType;
        public float multiplier = 1;
    }
    void BuildDamageMultipliers()
    {
        DamageTypeMultipliers = DamageTypeMultiplierList.ToDictionary(dtm => dtm.damageType, dtm => dtm.multiplier);
        //Make sure that the Direct DamageType is direct, no matter what is entered into the Editor.
        DamageTypeMultipliers[DamageType.Direct] = 1;
        //Ditto for None, except... none
        DamageTypeMultipliers[DamageType.None] = 0;
        DamageTypeMultipliers[DamageType.Default] = 1;
    }
    private void OnValidate()
    {
        BuildDamageMultipliers();
        if (currentHealth <= 0)
        {
            OnBroken_internal(new DamageableDamagedEventArgs() { });

        }
    }
    //End Monkey Business

    public event DamageableDamaged OnDamageDealt;
    public event DamageableDamaged OnDamaged;
    public event DamageableDamaged OnBroken;

    //Lazy load DamageTypes.  For Efficiency.  Because why not.
    private DamageType[] _damageType;
    private DamageType[] DamageTypes
    {
        get
        {
            return _damageType ?? (_damageType = (DamageType[])Enum.GetValues(typeof(DamageType)));
        }
        set
        {
            _damageType = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        SetHealthBar(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Deal Damage of the <see cref="DamageType"/> supplied, in the amount applied.  returns a float of how much damage was actually dealt (after damage strengths and weaknesses have been applied).
    /// For Example, the amount supplied might be 10, but if the Damageable instance being damaged has a weakness for the supplied <see cref="DamageType"/>, (perhaps the DamageType is Hot and the Damageable is a glacier) then the actual damage dealt might be 15.  
    /// In another example, if the damage amount supplied is 10, but the Damageable being damaged has a strength against the supplied <see cref="DamageType"/> (Perhaps the DamageType is Cold but the Damageable is a glacier) then the actual damage dealt might be 5.
    /// </summary>
    /// <param name="damageType">The Type of damage being applied</param>
    /// <param name="amount">The amount of damage to apply.  The actual amount of damage applied might vary due to the strengths and weaknesses of the Damageable</param>
    /// <returns>How much damage was actually dealt to the Damageable</returns>
    public float DealDamage(DamageType damageType, float amount)
    {
        float OldHealth = currentHealth;
        //The supplied damageType might be DamageType.Enemy,
        //but the DamageTypeMultiplier dictionary might only define DamageType.EnemyOrBoss
        //In case that happens, this code will find out the best DamageTypeMultiplier dictionary entry to use
        DamageType damageTypeToUse;
        if (DamageTypeMultipliers.ContainsKey(damageType))
        {
            damageTypeToUse = damageType;
        }
        else
        {
            //Default to... Default
            damageTypeToUse = DamageType.Default;
        }

        float actualAmount = amount * DamageTypeMultipliers[damageTypeToUse];
        currentHealth -= actualAmount;
        var eArgs = new DamageableDamagedEventArgs()
        {
            DamageableType = this.damageableType,
            DamageType = damageType,
            Amount = amount,
            ActualAmount = actualAmount,
            OldHealth = OldHealth,
            NewHealth = currentHealth,
            DamagedDamageable = this
        };
        OnDamageDealt_internal(eArgs);
        if (currentHealth <= 0)
        {
            OnBroken_internal(eArgs);
        }
        else
        {
            OnDamaged_internal(eArgs);
        }
        return actualAmount;
    }

    private void OnBroken_internal(DamageableDamagedEventArgs eArgs)
    {
        if (OnBroken != null)
        {
            OnBroken.Invoke(eArgs);
        }
        DispatchDamageEvent(eArgs, DamageEvent.BROKEN);
        switch (deathAction)
        {
            case DamageableDeadAction.None:
                break;
            case DamageableDeadAction.Destroy:
                Destroy(this.gameObject);
                break;
            case DamageableDeadAction.Custom:
                throw new NotImplementedException("Custom Damageable Broken Actions are not supported.  yet.");
            default:
                Debug.LogWarning("This Damageable was broken, but the DamageableBrokenAction was set to an invalid value.");
                break;
        }
    }
    private void OnDamaged_internal(DamageableDamagedEventArgs eArgs)
    {
        if (OnDamaged != null)
        {
            OnDamaged.Invoke(eArgs);
        }
        DispatchDamageEvent(eArgs, DamageEvent.DAMAGED);
    }
    private void OnDamageDealt_internal(DamageableDamagedEventArgs eArgs)
    {
        if (OnDamageDealt != null)
        {
            OnDamageDealt.Invoke(eArgs);
        }
        SetHealthBar(eArgs.NewHealth);
        DispatchDamageEvent(eArgs, DamageEvent.DAMAGE_DEALT);
    }

    private void SetHealthBar(float NewHealth)
    {
        if (useHealthBar)
            healthBarForeground.sizeDelta = new Vector2((healthBarBackground.sizeDelta.x / maxHealth) * NewHealth, healthBarForeground.sizeDelta.y);
    }

    private void DispatchDamageEvent(DamageableDamagedEventArgs a, string eventType)
    {
        //Brodcast to two listers: 1) everyone looking for the eventtype (no matter the DamageableType) and 2) only folks filtering to a particular type of DamageableType.
        Messenger<DamageableDamagedEventArgs>.Broadcast(eventType, a, MessengerMode.DONT_REQUIRE_LISTENER);
        Messenger<DamageableDamagedEventArgs>.Broadcast(eventType + "_" + a.DamageableType.ToString(), a, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}

public enum DamageType
{
    Default = 0,
    /// <summary>
    /// Dealing with a piercing projectile or sword 
    /// </summary>
    Sharp,
    /// </summary> 
    /// Dealing with an explosive force (bomb damage...?)
    /// </summary> 
    Explosive,
    /// </summary> 
    /// Dealing with a strong, blunt force (battering ram, hammer, etc)
    /// </summary> 
    Battering,

    /// </summary> 
    /// Damage due to a very hot object (firey coal, lava bullet, fire attack...) NOT from an effect, such as being on fire
    /// </summary> 
    Hot,
    /// </summary> 
    /// Damage due to a very cold object (ice shard, snow attack...) NOT from an effect, such as hypothermia
    /// </summary> 
    Cold,
    /// </summary> 
    /// Damage due to electricty (lightning, telephone wire, etc)
    /// </summary> 
    Electric,
    /// </summary> 
    /// Damage due to a magical attack that doesn't fit in a logical damage category (such as a forceful magic energy extraction extraction, or other magical exercize that doesn't deal direct damage, but doesn't leave an effect)
    /// </summary> 
    Magic,
    /// <summary>
    /// Dealing with an effect (fire,  poison,or other extended-release damages).
    /// </summary>
    Effectual,
    /// <summary>
    /// Signifies that the damage being dealt should be dealt in precisely the specified amount, no matter what strengths the recipentent might have.
    /// </summary>
    Direct,
    /// </summary> 
    /// A damage that really doesn't fit in any category.  Try not to use this, in favor of coming up with a new (general) category that fits a desired spell.
    /// </summary> 
    Other,
    /// <summary>
    /// No Damage should be dealt, no matter the amount specified.
    /// </summary>
    None
}

[Flags]
public enum DamageableType
{
    Default = 0,
    Player = 1,
    /// <summary>
    /// An entity CREATED BY or otherwise WORKING FOR the player, but not the player itself
    /// </summary>
    PlayerEntity = 2,
    PlayerEntityOrPlayer = Player | PlayerEntity,
    Enemy = 4,
    Boss = 8,
    EnemyOrBoss = Enemy | Boss,
    Obstacle = 16,
    Structure = 32,
    /// <summary>
    /// Any generic (moveable) object.  Beach Balls, anyone?
    /// </summary>
    Object = 64,
    ObstacleOrStructure = Obstacle | Structure,
    /// <summary>
    /// Any DamageableType that moves
    /// </summary>
    Movable = Player | PlayerEntity | Enemy | Boss | Object,
    /// <summary>
    /// Any DamageableType that never, ever moves
    /// </summary>
    Unmoveable = Obstacle | Structure,
    All = Default | Player | PlayerEntity | Enemy | Boss | Obstacle | Structure | Object,

}

public enum DamageableDeadAction
{
    None, Destroy, Custom
}


public class DamageableDamagedEventArgs
{
    public DamageType DamageType { get; set; }
    public DamageableType DamageableType { get; set; }
    public float Amount { get; set; }
    public float ActualAmount { get; set; }
    public float OldHealth { get; set; }
    public float NewHealth { get; set; }
    public Damageable DamagedDamageable { get; set; }
}

public delegate void DamageableDamaged(DamageableDamagedEventArgs args);