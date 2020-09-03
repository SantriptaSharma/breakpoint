using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{ 
    public abstract class ShootingBehaviourHook : MonoBehaviour
    {
        PolygonalBody body;

        protected virtual void Start()
        {
            body = GetComponent<PolygonalBody>();
            Debug.Log("called");
            Modify(body.points.ToArray());
        }

        protected abstract void Modify(PolygonalPoint[] points);
    }
}