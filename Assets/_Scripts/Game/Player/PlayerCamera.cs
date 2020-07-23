using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance = null;

        public float radius, settleRadius;
        public float timeToMoveMax;

        private Camera cam;
        private float maxMoveDelta;

        public Vector3 MousePositionFromPlayerPOV()
        {
            Vector3 actualCameraPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Transform player = transform.parent;
            return (actualCameraPosition + (player.position - transform.position));
        }

        public void DoScreenShake(float duration, float posStrength = 3, float rotStrength = 90)
        {
            cam.DOShakePosition(duration, posStrength);
        }

        private void Awake()
        {
            if(instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        void Start()
        {
            cam = GetComponent<Camera>();
            maxMoveDelta = radius / timeToMoveMax;
        }

        void Update()
        {
            Vector3 mousePosition = MousePositionFromPlayerPOV();
            Vector3 target = mousePosition - transform.parent.position;
            target = target.normalized;
            float mouseDistance = Vector3.Distance(transform.parent.position, mousePosition);
            target *= Mathf.Min(radius, mouseDistance);
            Debug.DrawLine(transform.parent.position, transform.parent.position + target);

            if (mouseDistance <= settleRadius)
                target *= 0;

            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position + target, maxMoveDelta * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.parent.position, radius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.parent.position, settleRadius);
        }
    }
}