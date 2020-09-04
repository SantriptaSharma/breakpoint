using SantriptaSharma.Breakpoint.Polygons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{ 
    public class Destroy : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.GetComponent<PolygonalPoint>().body.Kill();
            Destroy(collision.gameObject.GetComponent<PolygonalPoint>().body.gameObject, 0.3f);
        }
    }
}