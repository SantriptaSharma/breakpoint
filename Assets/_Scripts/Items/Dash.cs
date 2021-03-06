﻿using SantriptaSharma.Breakpoint.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Items
{ 
    //A power which makes the player 'dash'
    public class Dash : Item
    {
        public DashType dashType;
        public float forceMagnitude, dashTime;
        public float dashControlFactor;
        [Header("Screenshake")]
        public float screenShakeDuration;
        public float posStrength;
        public int vibrato;

        public override void Use()
        {
            if (currentTime > 0) return;
            player.AddForce(dashType == DashType.Aim ? player.aimDirection * forceMagnitude : player.voluntaryMoveDirection * forceMagnitude);
            player.StopLimitingVelocityForSeconds(dashTime);
            player.SetControlFactorForSeconds(new Vector2(dashControlFactor, dashControlFactor), dashTime + player.timeToResetSpeed);
            PlayerCamera.instance.DoScreenShake(screenShakeDuration, posStrength, vibrato);
            currentTime = cooldown;
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