using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{

    void Die();
    int Experience { get; set; }
    int Id { get; set; }
    void PerformAttack();
    Spawner Spawner { get; set; }
    void TakeDamage(int amount);


}
