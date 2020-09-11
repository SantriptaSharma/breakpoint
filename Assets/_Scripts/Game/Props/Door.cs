using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class Door : MonoBehaviour
    {
        public bool isLocked;

        private bool open;
        private Animator anim;
        
        public void Unlock()
        {
            isLocked = false;
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void Open()
        {
            if (!isLocked && !open)
            {
                anim.SetTrigger("open");
                open = true;
            }
        }

        void Start()
        {
            anim = GetComponent<Animator>();
        }
    }
}