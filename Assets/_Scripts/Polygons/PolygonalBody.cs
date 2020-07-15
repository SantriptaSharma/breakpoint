using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SantriptaSharma.Breakpoint.Polygons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(LineRenderer))]
    public class PolygonalBody : MonoBehaviour
    {
        public GameObject pointObject;

        public int sides = 3;
        public float length = 1.0f;
        public float lineWidth = 0.3f;
        [Range(0,1)]
        public float damping = 0.5f;
        [Range(0, 1)]
        public float interDamping = 0.5f;
        [System.NonSerialized]
        public List<PolygonalPoint> points;

        private new LineRenderer renderer;
        private Rigidbody2D rb;

        private float radius;

        void Awake()
        {
            points = new List<PolygonalPoint>();
            renderer = GetComponent<LineRenderer>();
            rb = GetComponent<Rigidbody2D>();

            renderer.widthCurve = AnimationCurve.Constant(0, 1, lineWidth);

            //Formulae I got off the internet, radius & central angle between two adjacent points on a regular polygon
            radius = length / (2 * Mathf.Sin(180 / sides * Mathf.Deg2Rad));
            float angle = 360 / sides;

            for (int i = 0; i < sides; i++)
            {
                points.Add(Instantiate(pointObject).GetComponent<PolygonalPoint>());
                points[i].body = this;
                points[i].position = i;

                Quaternion rot = Quaternion.AngleAxis(angle * i, Vector3.back);
                Vector3 pointPos = transform.position + rot * Vector3.up * radius * 0.1f;

                points[i].transform.position = pointPos;

                points[i].transform.parent = transform;
                AddSpringyToPoint(i);

                if (i == 0)
                {
                    SpringJoint2D joint = points[0].GetComponent<SpringJoint2D>();
                    joint.distance = length;
                    joint.dampingRatio = interDamping;
                    continue;
                }
                
                SpringJoint2D sJoint = points[i].GetComponent<SpringJoint2D>();
                sJoint.dampingRatio = interDamping;
                sJoint.distance = length;
                sJoint.connectedBody = points[i - 1].GetComponent<Rigidbody2D>();

                if(i == sides-1)
                { 
                    points[0].GetComponent<SpringJoint2D>().connectedBody = sJoint.GetComponent<Rigidbody2D>();
                    continue;
                }

            }
        }

        void AddSpringyToPoint(int i)
        {
            SpringJoint2D sj = gameObject.AddComponent<SpringJoint2D>();
            sj.autoConfigureDistance = false;
            sj.distance = radius;
            sj.connectedBody = points[i].GetComponent<Rigidbody2D>();
            sj.dampingRatio = damping;
            sj.frequency = 4;
        }

        public void OnRemoved(int i)
        {
            PolygonalPoint point = points[i];
            foreach(SpringJoint2D s in GetComponents<SpringJoint2D>())
            {
                if(s.connectedBody.gameObject == point.gameObject)
                {
                    Destroy(s);
                    break;
                }
            }
        }

        void Update()
        {
            renderer.positionCount = sides;
            for(int i = 0; i < sides; i++)
            {
                renderer.SetPosition(i, points[i].transform.position);
            }

            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    points[0].RemoveSelf();
            //}

            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    points[sides - 1].RemoveSelf();
            //}

            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    AddPoint();
            //}
        }

        public void AddPoint()
        {
            Vector3 nPos = (points[0].transform.position + points[sides-1].transform.position) / 2;
            PolygonalPoint point =  Instantiate(pointObject).GetComponent<PolygonalPoint>();
            point.position = sides;
            points.Add(point);
            AddSpringyToPoint(sides);
            sides += 1;

            point.transform.position = nPos;
            point.transform.parent = transform;
            
            point.body = this;
            
            SpringJoint2D sJoint = point.GetComponent<SpringJoint2D>();
            sJoint.distance = length;
            sJoint.connectedBody = points[sides - 2].GetComponent<Rigidbody2D>();
            points[0].GetComponent<SpringJoint2D>().connectedBody = point.GetComponent<Rigidbody2D>();
        }

        public void Kill()
        {
            //Temp
            Destroy(gameObject);
        }
    }
}