using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class DoorPolygonActivator : MonoBehaviour
    {
        public PolygonalBody[] bodies;
        public ActivationEvent onActivate;

        private int number;

        void Start()
        {
            number = bodies.Length;
            for(int i = 0; i < number; i++)
            {
                bodies[i].onDie.AddListener(RemovePolygon);
            }
        }

        void RemovePolygon(PolygonalBody body)
        {
            if (--number <= 0) Activate();
        }

        void Activate()
        {
            onActivate.Invoke();
        }
    }
}