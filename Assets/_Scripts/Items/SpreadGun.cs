using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SantriptaSharma.Breakpoint.Game;

namespace SantriptaSharma.Breakpoint.Items
{ 
    public class SpreadGun : Weapon
    {
        public int numberOfProjectiles;
        public float spreadDegrees;
        public float separation;
        public float kickForce;
        public float projectileLiveTime;

        private float anglePerProjectile;

        public override void Use()
        {
            if (currentTime > 0) return;

            Vector3 dir = player.aimDirection;

            float turnsToRotateBack = (numberOfProjectiles % 2 == 0) ? (numberOfProjectiles / 2) - 0.5f : Mathf.Floor(numberOfProjectiles / 2);
            dir = Quaternion.AngleAxis(turnsToRotateBack * -anglePerProjectile, Vector3.back) * dir;
            Quaternion rot = Quaternion.AngleAxis(anglePerProjectile, Vector3.back);

            for(int i = 0; i < numberOfProjectiles; i++)
            {
                var o = Instantiate(projectile, transform.position + dir * separation, Quaternion.FromToRotation(Vector3.up, dir), ProjectileHolder.instance);
                o.GetComponent<Rigidbody2D>().velocity = dir * shootVelocity;
                player.AddForce(dir * kickForce * -1);
                PlayerCamera.instance.DoScreenShake(0.2f, 0.012f * numberOfProjectiles, (int)(0.5f * numberOfProjectiles));
                currentTime = cooldown;
                Destroy(o, projectileLiveTime);

                dir = rot * dir;
            }
        }

        protected override void Start()
        {
            base.Start();
            anglePerProjectile = spreadDegrees / (numberOfProjectiles - 1);
        }
    }
}
