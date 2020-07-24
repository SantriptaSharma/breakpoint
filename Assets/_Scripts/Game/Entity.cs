using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

        private Vector3 lastDir;
        private float health;
        private float lastDamage;

        private void Start()
        {
            health = maxHealth;
            lastDamage = -100;
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Damage dmg = collision.GetComponent<Damage>();
            if (dmg == null || !dmg.takeTriggerDamage) return;

            bool isEvil = false;
            for(int i = 0; i < damageTags.Length; i++)
            { 
                if(dmg.dTag == damageTags[i])
                {
                    isEvil = true;
                    break;
                }
            }

            if(isEvil)
            {
                TakeDamage(dmg.damage);
                TakeKnockback(dmg.knockbackAmount, dmg.transform.position);
                if (dmg.destroyOnCollide) Destroy(dmg.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Damage dmg = collision.gameObject.GetComponent<Damage>();
            if (dmg == null || !dmg.takeColliderDamage) return;

            bool isEvil = false;
            for (int i = 0; i < damageTags.Length; i++)
            {
                if (dmg.dTag == damageTags[i])
                {
                    isEvil = true;
                    break;
                }
            }

            if (isEvil)
            {
                TakeDamage(dmg.damage);
                TakeKnockback(dmg.knockbackAmount, dmg.transform.position);
                if (dmg.destroyOnCollide) Destroy(dmg.gameObject);
            }
        }

        public float TakeDamage(float damage)
        {
            if (Time.time - lastDamage <= 0.1f) return health;

            health -= damage;

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