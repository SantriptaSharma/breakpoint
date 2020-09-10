using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SantriptaSharma.Breakpoint.Game
{
    [System.Serializable]
    public class PauseEvent : UnityEvent { };

    public class PauseController : MonoBehaviour
    {
        public static bool isPaused;
        public static PauseController instance;

        public float slowInTime, slowInDivisions;
        [Space]
        public RectTransform pausePanel;
        public PauseEvent onPaused;
        public PauseEvent onResume;

        private float lastTimeScale, fixedDeltaTime;
        private bool realPause;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            isPaused = false;
            realPause = false;
            fixedDeltaTime = Time.fixedDeltaTime;
            pausePanel.gameObject.SetActive(isPaused);
            lastTimeScale = 1;
        }

        private void Update()
        {
            if (OutcomeController.instance.outcomeReached) return;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }

            if(realPause && Input.GetKeyDown(KeyCode.Backspace))
            {
                LevelController.instance.GoToMainMenu();
            }

            if(realPause && Input.GetKeyDown(KeyCode.R))
            {
                LevelController.instance.ReloadLevel();
            }
        }

        public void Pause()
        {
            if(!isPaused)
            {
                onPaused.Invoke();
                lastTimeScale = Time.timeScale;
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
            }

            realPause = true;
            isPaused = true;
            pausePanel.gameObject.SetActive(true);
        }

        public void Resume()
        {
            if (isPaused)
            {
                onResume.Invoke();
                Time.timeScale = lastTimeScale;
                Time.fixedDeltaTime = fixedDeltaTime * lastTimeScale;
            }

            realPause = false;
            isPaused = false;
            pausePanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Resume();
        }

        public void Toggle()
        {
            if (isPaused && !realPause) isPaused = false;
            if (isPaused) Resume(); else Pause();
        }
    }
}