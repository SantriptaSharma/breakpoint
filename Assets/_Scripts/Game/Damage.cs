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
        [Space]
        public bool takeTriggerDamage;
        public bool takeColliderDamage;
        public bool destroyOnCollide;
        [Space]
        public bool useLastPositionKnocking;
        [System.NonSerialized]
        public Vector3 lastPosition;

        private void LateUpdate()
        {
            if (PauseController.isPaused) return;

            lastPosition = transform.position;
        }

        public void SetDamageActive(bool active)
        {
            takeTriggerDamage = takeColliderDamage = active;
        }
    }
}