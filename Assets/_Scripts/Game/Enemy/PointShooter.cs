using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{ 
    public class PointShooter : MonoBehaviour
    {
        public float shootCooldown;
        public float firstShotOffset;
        public bool homing;
        public GameObject bullet;
        public float bulletVelocity;
        public float bulletLiveTime;
        public float kickForce;
        public bool shootingEnabled = true;

        Player player;
        PolygonalPoint point;
        PolygonalBody body;
        float shootTimer;

        private void Start()
        {
            shootTimer = firstShotOffset + shootCooldown;
            player = Player.instance;
            point = GetComponent<PolygonalPoint>();
            body = point.body;
        }

        public void Reset()
        {
            shootTimer = firstShotOffset + shootCooldown;
        }

        private void Update()
        {
            if (!shootingEnabled) return;

            shootTimer -= Time.deltaTime;

            if(shootTimer > 0) return;

            Vector3 direction = homing ? (player.transform.position - transform.position).normalized : (transform.position - body.transform.position).normalized;
            Rigidbody2D bulletRb = Instantiate(bullet, transform.position, Quaternion.FromToRotation(Vector3.up, direction), ProjectileHolder.instance).GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * bulletVelocity;
            point.rb.AddForce(direction * kickForce * -1);
            Destroy(bulletRb.gameObject, bulletLiveTime);
            shootTimer = shootCooldown;
        }
    }
}