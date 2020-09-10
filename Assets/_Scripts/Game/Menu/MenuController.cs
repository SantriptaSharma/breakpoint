using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SantriptaSharma.Breakpoint.Game
{
    [System.Serializable]
    public class OnSelected : UnityEvent { };

    
    [System.Serializable]
    public struct MenuButton
    {
        public RectTransform caretPosition;
        public OnSelected onSelect;
    }

    public class MenuController : MonoBehaviour
    {
        [Header("Buttons")]
        public MenuButton[] menuButtons;

        [Space]
        public RectTransform caret;

        private int selected;

        void Start()
        {
            selected = 0;
            SetCaret();
        }

        void SetCaret()
        {
            caret.anchoredPosition = menuButtons[selected].caretPosition.anchoredPosition;
        }

        void Update()
        {
            // TODO: Use get axis instead
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if(++selected >= menuButtons.Length)
                {
                    selected = 0;
                }
                SetCaret();
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(--selected < 0)
                {
                    selected = menuButtons.Length - 1;
                }
                SetCaret();
            }
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                menuButtons[selected].onSelect.Invoke();
            }
        }

        public void Init()
        {
            LevelController.instance.GoToNextLevel();
        }

        public void Break()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}