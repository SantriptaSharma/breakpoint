using SantriptaSharma.Breakpoint.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Items
{ 
    public class Peashooter : Weapon
    {
        public float kickForce;
        private Player player;

        public override void Use()
        {
            if (currentTime > 0) return;

            Vector3 dir = player.aimDirection;
            var o = Instantiate(projectile, transform.position, Quaternion.identity);
            o.GetComponent<Rigidbody2D>().velocity = dir * shootVelocity;
            player.AddForce(dir * kickForce * -1);
            PlayerCamera.instance.DoScreenShake(0.2f, 0.03f, 0.5f);
            currentTime = cooldown;
            Destroy(o, 12);
        }

        protected override void Start()
        {
            base.Start();
            player = Player.instance;
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}