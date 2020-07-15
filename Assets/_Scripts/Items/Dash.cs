using SantriptaSharma.Breakpoint.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Items
{ 
    //A power which makes the player 'dash'
    public class Dash : Item
    {
        public DashType dashType;
        public float forceMagnitude;

        private Player player;

        public override void Use()
        {
            if (currentTime > 0) return;
            player.AddForce(dashType == DashType.Aim ? player.aimDirection * forceMagnitude : player.moveDirection * forceMagnitude);
            currentTime = cooldown;
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

        public enum DashType
        { 
            Aim, Move
        }
    }
}