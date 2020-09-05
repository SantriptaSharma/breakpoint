using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    [RequireComponent(typeof(PolygonalBody))]
    public class PolygonBehaviour : MonoBehaviour
    {
        PolygonalBody body;

        void Start()
        {
            body = GetComponent<PolygonalBody>();
            body.onDie.AddListener(Died);
        }

        void Update()
        {
        
        }

        void Died(PolygonalBody deadBody)
        {
            Destroy(gameObject);
        }
    }
}