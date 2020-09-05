using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class PointBehaviour : MonoBehaviour
    {
        public PolygonalPoint point;
        public PointShooter shooter;
        public Entity entity;
        public SpriteRenderer spriteRenderer;
        public bool fade;

        private void Start()
        {
            point = GetComponent<PolygonalPoint>();
            shooter = GetComponent<PointShooter>();
            entity = GetComponent<Entity>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            entity.onEntityDied.AddListener(Die);
        }

        private void Update()
        {
            if (PauseController.isPaused) return;

            if (fade)
            {
                Color c = spriteRenderer.color;
                c.a -= (Time.deltaTime * 2);
                spriteRenderer.color = c;
                if(c.a <= 0.05f)
                {
                    Destroy(gameObject);
                    fade = false;
                }
            }
        }

        public void Die(Entity e)
        {
            entity.rb.velocity *= 0;
            //entity.RepeatLastKnockback(1200);
            point.RemoveSelf();
            shooter.shootingEnabled = false;
            fade = true;
        }
    }
}