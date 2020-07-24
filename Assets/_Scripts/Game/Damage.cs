using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{ 
    public enum DamageTag
    {
        PlayerDamage, EnemyDamage
    }

    public class Damage : MonoBehaviour
    {
        public float damage;
        public DamageTag dTag;
        public float knockbackAmount;
        public bool takeTriggerDamage;
        public bool takeColliderDamage;
        public bool destroyOnCollide;
        
        public void SetDamageActive(bool active)
        {
            takeTriggerDamage = takeColliderDamage = active;
        }
    }
}