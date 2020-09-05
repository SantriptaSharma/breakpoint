using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class EnemyFlapMovement : MonoBehaviour
    {
        public float flapStrength;
        public float flapCooldown;

        private float flapTimer;
        private Rigidbody2D rb;
        private Player player;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = Player.instance;
            flapTimer = flapCooldown;
        }

        private void Update()
        {
            if (PauseController.isPaused) return;

            flapTimer -= Time.deltaTime;

            if (flapTimer > 0)
                return;

            Vector3 dir = (player.transform.position - transform.position).normalized;
            rb.AddForce(dir * flapStrength);
            flapTimer = flapCooldown;
        }
    }
}