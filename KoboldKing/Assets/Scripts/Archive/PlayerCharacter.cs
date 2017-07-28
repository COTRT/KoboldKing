using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerCharacter : MonoBehaviour
    {
        private GameObject player;

        public void Hurt(int damage)
        {
            //Managers.Managers.Player.ChangeHealth(-damage);
        }

        public void Heal(int heal)
        {
            //Managers.Managers.Player.ChangeHealth(heal);
        }

        public void Death()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            DestroyObject(player);
        }
    }
}
