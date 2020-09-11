using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SantriptaSharma.Breakpoint.Game
{
    [System.Serializable]
    public class ActivationEvent : UnityEvent { };

    public class DistanceActivator : MonoBehaviour
    {
        public float activationRadius;
        public float updateDelay;
        public ActivationEvent onActivate;
        private float squaredActivationRadius;
        private Player player;

        void Start()
        {
            player = Player.instance;
            squaredActivationRadius = activationRadius * activationRadius;
            StartCoroutine(Check());
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, activationRadius);
        }

        private IEnumerator Check()
        {
            for (; ; )
            {
                if (PauseController.isPaused) yield return new WaitForSeconds(0.0001f); //Resume on unpause
                Vector3 myPos = transform.position; Vector3 playerPos = player.transform.position;
                myPos.z = 0; playerPos.z = 0;
                if ((playerPos - myPos).sqrMagnitude <= squaredActivationRadius)
                {
                    onActivate.Invoke();
                    Debug.Log("activated");
                }
                yield return new WaitForSeconds(updateDelay);
            }
        }
    }
}