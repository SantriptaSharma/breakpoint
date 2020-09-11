using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance = null;

        public float radius, settleRadius, transitionOrthSize;
        public float timeToMoveMax;
        public Camera cam;

        private float maxMoveDelta, initialOrthSize;

        public Vector3 MousePositionFromPlayerPOV()
        {
            Vector3 actualCameraPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Transform player = transform.parent;
            return (actualCameraPosition + (player.position - transform.position));
        }

        public void DoScreenShake(float duration, float posStrength = 3, int rotStrength = 10)
        {
            cam.DOShakePosition(duration, posStrength, rotStrength);
        }

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        void Start()
        {
            cam = GetComponent<Camera>();
            maxMoveDelta = radius / timeToMoveMax;
            initialOrthSize = cam.orthographicSize;
            cam.orthographicSize = transitionOrthSize;
            StartCoroutine(ChangeFov(initialOrthSize, 0.2f, 100));
        }

        public IEnumerator ChangeFov(float newFov, float time = 0.2f, float steps = 10)
        {
            float fov = cam.orthographicSize;
            float diff = Mathf.Abs(newFov - fov);
            float deltaPerStep = diff / steps, timePerStep = time / steps;

            for (int i = 0; i < steps; i++)
            {
                cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, newFov, deltaPerStep);
                yield return new WaitForSecondsRealtime(timePerStep);
            }
        }

        public void LevelEnd(float time)
        {
            StartCoroutine(ChangeFov(transitionOrthSize, time, 100));
        }

        void Update()
        {
            if (PauseController.isPaused) return;

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