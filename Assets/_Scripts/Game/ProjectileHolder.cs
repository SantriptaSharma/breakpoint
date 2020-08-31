using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class ProjectileHolder : MonoBehaviour
    {
        public static Transform instance = null;

        private void Awake()
        {
            instance = gameObject.transform;
        }
    }
}