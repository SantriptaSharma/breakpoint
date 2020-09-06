using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

namespace SantriptaSharma.Breakpoint.Game
{
    [System.Serializable]
    public class EntityDeathEvent : UnityEvent<Entity>{}
    [System.Serializable]
    public class DamageEvent : UnityEvent<float, float> {}

    [RequireComponent(typeof(Rigidbody2D))]
    public class Entity : MonoBehaviour
    {
        public DamageTag[] damageTags;
        public float maxHealth;
        public EntityDeathEvent onEntityDied;
        public DamageEvent onTakeDamage;
        public Rigidbody2D rb;
        [System.NonSerialized]
        public bool takingDamage;

        private Vector3 lastDir;
        private float health;
        private float lastDamage;

        private void Start()
        {
            health = maxHealth;
            lastDamage = -100;
            lastDir = Vector3.zero;
            rb = GetComponent<Rigidbody2D>();
            takingDamage = true;
        }

        public void ProcessHit(Damage dmg)
        {
            if (!takingDamage) return;

            Vector3 knockSource = dmg.useLastPositionKnocking ? dmg.lastPosition : dmg.transform.position;
            TakeKnockback(dmg.knockbackAmount, knockSource);
            TakeDamage(dmg.damage);
            if (dmg.destroyOnCollide)
            {
                Destroy(dmg.gameObject);
                dmg.SetDamageActive(false);
            }
        }

        public bool CheckValidityAndProcessHit(Damage dmg)
        {
            if (!takingDamage) return false;

            bool isEvil = CheckValidity(dmg);
            if (isEvil) ProcessHit(dmg);
            return isEvil;
        }

        public bool CheckValidity(Damage dmg)
        {
            if (!takingDamage) return false;

            for (int i = 0; i < damageTags.Length; i++)
            {
                if (dmg.dTag == damageTags[i])
                {
                    return true;
                }
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!takingDamage) return;

            Damage dmg = collision.GetComponent<Damage>();
            if (dmg == null || !dmg.takeTriggerDamage) return;

            for(int i = 0; i < damageTags.Length; i++)
            { 
                if(dmg.dTag == damageTags[i])
                {
                    ProcessHit(dmg);
                    break;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!takingDamage) return;

            Damage dmg = collision.gameObject.GetComponent<Damage>();
            if (dmg == null || !dmg.takeColliderDamage) return;

            for (int i = 0; i < damageTags.Length; i++)
            {
                if (dmg.dTag == damageTags[i])
                {
                    ProcessHit(dmg);
                    break;
                }
            }
        }

        public float TakeDamage(float damage)
        {
            if (Time.time - lastDamage <= 0.05f || !takingDamage) return health;

            health -= damage;
            health = Mathf.Clamp(health, health, maxHealth);
            if (health <= 0)
            {
                onEntityDied.Invoke(this);
            }

            onTakeDamage.Invoke(damage, health);
            lastDamage = Time.deltaTime;
            return health;
        }

        public void TakeKnockback(float magnitude, Vector3 source)
        {
            if (!takingDamage) return;

            Vector3 dir = (transform.position - source).normalized;
            rb.AddForce(dir * magnitude);
            lastDir = dir;
        }

        public void RepeatLastKnockback(float magnitude)
        {
            rb.AddForce(lastDir * magnitude);
        }
    }
}