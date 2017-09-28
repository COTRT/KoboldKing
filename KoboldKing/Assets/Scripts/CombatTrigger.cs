using Assets.Scripts;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {

    public float attackTimer = 0;
    public float coolDown = 2.0f;
    public bool attackAgain = false;
    GameObject Target;	

    private void Attack()
    {
        CombatCalculator.Attack(gameObject, Target.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name != Target.name)
        {
            Target = other.gameObject;
        }
        attackTimer = coolDown;
        attackAgain = true;
    }
    private void OnTriggerExit(Collider other)
    {
        attackAgain = false;
    }

    // Update is called once per frame
    void Update () {
		if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        if(attackTimer < 0)
        {
            attackTimer = 0;
        }
        if (attackTimer == 0 && attackAgain)
        {
            Attack();
        }
    }
}