using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SantriptaSharma.Breakpoint.Game
{
    [RequireComponent(typeof(LineRenderer))]
    public class Trapper : MonoBehaviour
    {
        public float radius, detectionUpdateTime, leaveTime, leaveGraceTime, maxForce, constantForceScale, launchScale;
        public AnimationCurve forceScale;
        public LayerMask mask;

        private LineRenderer rendererer;
        private Rigidbody2D target;
        private bool hasTarget;

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        void Start()
        {
            rendererer = GetComponent<LineRenderer>();
            target = null;
            hasTarget = false;
            StartCoroutine(FindTarget());
        }

        IEnumerator FindTarget()
        {
            while(true)
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, radius, mask.value);
                
                if(colls.Length != 0)
                {
                    Collider2D coll = colls[0];
                    float min = (transform.position - coll.transform.position).sqrMagnitude;

                    for (int i = 1; i < colls.Length; i++)
                    {
                        var c = colls[i];
                        var sqrDist = (transform.position - c.transform.position).sqrMagnitude;

                        if (sqrDist < min)
                        {
                            min = sqrDist;
                            coll = c;
                        }
                    }

                    target = coll.GetComponent<Rigidbody2D>();
                    if (target != null)
                    {
                        hasTarget = true;
                        StartCoroutine(LeaveTarget());
                        break;
                    }
                }

                yield return new WaitForSeconds(detectionUpdateTime);
            }
        }

        IEnumerator LeaveTarget()
        {
            yield return new WaitForSeconds(leaveTime);
            if(hasTarget)
            {
                Vector2 dir = target.transform.position - transform.position;
                if(dir.sqrMagnitude <= radius * radius * 1.4f) target.AddForce(dir.normalized * maxForce * launchScale);
                target = null;
                hasTarget = false;
                yield return new WaitForSeconds(leaveGraceTime);
            }
            StartCoroutine(FindTarget());
        }

        void Update()
        {
            if (target == null & hasTarget)
            {
                StopAllCoroutines();
                hasTarget = false;
                StartCoroutine(FindTarget());
            }    

            if (!hasTarget)
            {
                rendererer.positionCount = 0;
                return;
            }

            rendererer.positionCount = 2;
            Vector3[] positions = { transform.position, target.transform.position }; 
            rendererer.SetPositions(positions);

            Vector2 distance = transform.position - target.transform.position;
            float scaledDist = distance.magnitude / radius;
            float magnitude = maxForce * forceScale.Evaluate(scaledDist) * constantForceScale;
            target.AddForce(distance.normalized * magnitude);
        }
    }
}