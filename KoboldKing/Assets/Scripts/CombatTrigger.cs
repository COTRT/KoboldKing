using Assets.Scripts;
using UnityEngine;

public class CombatTrigger : MonoBehaviour {

    public float attackTimer = 0;
    public float coolDown = 2.0f;
    public bool attackAgain = false;
    private GameObject Target;

    private void Attack()
    {
        CombatCalculator.Attack(gameObject, Target.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other);
        if(!other.CompareTag("Player"))
        {
            return;
        }
        Target = other.gameObject;
        if (attackTimer <= 0)
        {
            Attack();
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
        if(Target == null)
        {
            attackAgain = false;
        }
        if (attackTimer == 0 && attackAgain)
        {
            Attack();
            attackTimer = coolDown;
        }
    }
}