using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class ShootingOneByOne : ShootingBehaviourHook
    {
        public float offset, cooldown;

        protected override void Modify(PolygonalPoint[] points)
        {
            for(int i = 0, n = points.Length; i < n; i++)
            {
                PointShooter shooter = points[i].GetComponent<PointShooter>();
                shooter.shootCooldown = cooldown;
                shooter.firstShotOffset = offset * i;
                shooter.Reset();
            }
        }
    }
}