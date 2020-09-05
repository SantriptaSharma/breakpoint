using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SantriptaSharma.Breakpoint.Game
{
    public class ActivatedFlapMovement : MonoBehaviour
    {
        public float flapStrength;
        public float flapCooldown;

        private bool isActive;
        private float flapTimer;
        private Rigidbody2D rb;
        private Player player;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = Player.instance;
            flapTimer = flapCooldown;
            isActive = false;
        }

        public void Activate()
        {
            isActive = true;
        }

        private void Update()
        {
            if (!isActive || PauseController.isPaused) return;

            flapTimer -= Time.deltaTime;

            if (flapTimer > 0)
                return;

            Vector3 dir = (player.transform.position - transform.position).normalized;
            rb.AddForce(dir * flapStrength);
            flapTimer = flapCooldown;
        }
    }
}