using UnityEngine;
using System.Collections;
using Assets.Scripts.Events;
using Assets.Scripts.Magic.Effects;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Magic.Spells
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Spell : MonoBehaviour
    {
        public SpellStopper spellStopper;
        public float lifespan;
        public bool immediateDamage;
        public bool hasCasterEffect;
        public Effect casterEffect;
        public bool hasTargetEffect;
        public Effect targetEffect;

        public event SpellCompleted OnSpellCompleted;

        ParticleSystem partSys;
        MeshRenderer meshRend;
        bool initialized = false;

        private void Initialize()
        {
            partSys = GetComponent<ParticleSystem>();
            meshRend = GetComponent<MeshRenderer>();
            if (meshRend != null)
            {
                meshRend.enabled = false;
            }
            initialized = true;
        }
        //ParticleSystem particleSystem;
        // Use this for initialization
        void Start()
        {
            if (!initialized) Initialize();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Cast(GameObject Caster)
        {
            Messenger<Spell>.Broadcast(MagicEvent.SPELL_CAST, this,MessengerMode.DONT_REQUIRE_LISTENER);
            if (!initialized) Initialize();
            transform.position = Caster.transform.position;
            if (meshRend != null)
            {
                meshRend.enabled = false;
            }

            partSys.Play();
            switch (spellStopper)
            {
                case SpellStopper.Lifespan:
                    StartCoroutine(LifespanSpellStopper());
                    break;
                case SpellStopper.UserAction:
                    break;
                case SpellStopper.Event:
                    break;
                default:
                    break;
            }
        }
        private IEnumerator LifespanSpellStopper()
        {
            yield return new WaitForSeconds(lifespan);
            CompleteSpell();
        }

        private void CompleteSpell()
        {
            Debug.Log("Spell Completed");
            Messenger<Spell>.Broadcast(MagicEvent.SPELL_COMPLETED, this,MessengerMode.DONT_REQUIRE_LISTENER);
            if(OnSpellCompleted != null)
            {
                OnSpellCompleted.Invoke(this);
            }
            Destroy(this);
        }
        private void OnParticleCollision(GameObject other)
        {
            Messenger.Broadcast(MagicEvent.SPELL_COLLISION, MessengerMode.DONT_REQUIRE_LISTENER);

            casterEffect.Apply(gameObject);
            targetEffect.Apply(other);
        }
    }

    public delegate void SpellCompleted(Spell spell); 
}