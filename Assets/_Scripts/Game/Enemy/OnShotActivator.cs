using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SantriptaSharma.Breakpoint.Polygons;
using UnityEngine.Events;

namespace SantriptaSharma.Breakpoint.Game
{
    [System.Serializable]
    public class PointShotEvent : UnityEvent { };

    public class OnShotActivator : ShootingBehaviourHook
    {
        public PointShotEvent onPointShot;

        protected override void Modify(PolygonalPoint[] points)
        {
            for(int i = 0; i < points.Length; i++)
            {
                points[i].GetComponent<Entity>().onTakeDamage.AddListener(Shot);
            }
        }

        private void Shot(float a, float b)
        {
            onPointShot.Invoke();
        }
    }
}