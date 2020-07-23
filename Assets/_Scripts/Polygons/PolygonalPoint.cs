using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Polygons
{
    public class PolygonalPoint : MonoBehaviour
    {
        public PolygonalBody body;
        public Rigidbody2D rb;
        public int position;

        bool isDying;

        private void Start()
        {
            isDying = false;
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("PlayerBullet"))
            {
                Destroy(collision.gameObject);
                RemoveSelf();
            }
        }

        public void RemoveSelf()
        {
            if (isDying) return;

            isDying = true;

            if(body.sides == 1)
            {
                body.Kill();
                return;
            }

            body.OnRemoved(position);

            if(position == 0)
            {
                body.points[position + 1].GetComponent<SpringJoint2D>().connectedBody = body.points[body.sides-1].GetComponent<Rigidbody2D>();
                body.points.RemoveAt(position);
                body.sides--;
                for (int i = position; i < body.sides; i++)
                {
                    body.points[i].position--;
                }
                Destroy(gameObject);
                return;
            }

            if(position == body.sides-1)
            {
                body.points[0].GetComponent<SpringJoint2D>().connectedBody = body.points[position - 1].GetComponent<Rigidbody2D>();
                body.points.RemoveAt(position);
                body.sides--;
                for (int i = position; i < body.sides; i++)
                {
                    body.points[i].position--;
                }
                Destroy(gameObject);
                return;
            }

            body.points[position + 1].GetComponent<SpringJoint2D>().connectedBody = body.points[position - 1].GetComponent<Rigidbody2D>();
            body.points.RemoveAt(position);
            body.sides--;
            for(int i = position; i < body.sides; i++)
            {
                body.points[i].position--;
            }

            Destroy(gameObject);
        }
    }
}