using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SantriptaSharma.Breakpoint.Game
{
    public class OutcomeController : MonoBehaviour
    {
        public static OutcomeController instance;
        public RectTransform outcomePanel;
        public TextMeshProUGUI outcomeText;
        [System.NonSerialized]
        public bool outcomeReached;

        private int polygons;
        private bool win;

        public void RemovePolygon()
        {
            if(--polygons <= 0 && !outcomeReached) Win();
            Debug.Log(polygons);
        }

        public void PlayerDie()
        {
            if (!outcomeReached) Lose();
        }

        private void Win()
        {
            outcomeReached = true;
            win = true;
            outcomePanel.gameObject.SetActive(true);
            outcomeText.text = "Level Completed";
            PauseController.isPaused = true;
        }

        private void Lose()
        {
            outcomeReached = true;
            win = false;
            outcomePanel.gameObject.SetActive(true);
            outcomeText.text = "Level Failed";
            PauseController.isPaused = true;
        }

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            polygons = FindObjectsOfType<PolygonBehaviour>().Length;
            outcomeReached = false;
            win = false;
            outcomePanel.gameObject.SetActive(false);
            Debug.Log($"Polygons: {polygons}");
        }

        private void Update()
        {
            if(Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.LeftBracket)) Lose();
                if (Input.GetKeyDown(KeyCode.RightBracket)) Win();
            }

            if (!outcomeReached) return;
            
            if(win)
            {
                if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    LevelController.instance.GoToNextLevel();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                {
                    LevelController.instance.ReloadLevel();
                }
            }
        }
    }
}