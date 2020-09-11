using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SantriptaSharma.Breakpoint.Polygons;

namespace SantriptaSharma.Breakpoint.Game
{ 
    public class ShootingActivated : ShootingBehaviourHook
    {
        List<PointShooter> shooters;
        bool prevActivated = false;

        protected override void Modify(PolygonalPoint[] points)
        {
            for(int i = 0; i < points.Length; i++)
            {
                var shooter = points[i].GetComponent<PointShooter>();
                if (shooter == null) continue;
                shooters.Insert(i, shooter);
                shooter.shootingEnabled = false;
            }
        }

        public void OnActivate()
        {
            if (prevActivated) return;
 
            prevActivated = true;
            for(int i = 0; i < shooters.Count; i++)
            {
                shooters[i].shootingEnabled = true;
            }
        }

        protected override void Start()
        {
            shooters = new List<PointShooter>();
            base.Start();
        }
    }
}