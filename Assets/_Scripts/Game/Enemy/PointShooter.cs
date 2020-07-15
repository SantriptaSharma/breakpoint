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

        PlayerCamera cam;
        Player player;
        PolygonalBody body;
        float shootTimer;

        private void Start()
        {
            shootTimer = firstShotOffset + shootCooldown;
            player = Player.instance;
            body = GetComponent<PolygonalPoint>().body;
            cam = PlayerCamera.instance;
        }

        public void Reset()
        {
            shootTimer = firstShotOffset + shootCooldown;
        }

        private void Update()
        {
            shootTimer -= Time.deltaTime;

            if(shootTimer > 0) return;

            Vector3 direction = homing ? (player.transform.position - transform.position).normalized : (transform.position - body.transform.position).normalized;
            Rigidbody2D bulletRb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * bulletVelocity;
            Destroy(bulletRb.gameObject, bulletLiveTime);
            cam.DoScreenShake(0.1f, 0.13f, 0.3f);
            shootTimer = shootCooldown;
        }
    }
}