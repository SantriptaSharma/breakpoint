using SantriptaSharma.Breakpoint.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Items
{ 
    public abstract class Weapon : Item
    {
        public GameObject projectile;
        public float shootVelocity;
        public override abstract void Use();

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}