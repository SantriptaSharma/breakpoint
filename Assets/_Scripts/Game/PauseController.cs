using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            isPaused = false;
            fixedDeltaTime = Time.fixedDeltaTime;
            pausePanel.gameObject.SetActive(isPaused);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }

            if(isPaused && Input.GetKeyDown(KeyCode.Backspace))
            {
                SceneManager.LoadScene(0);
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

            isPaused = false;
            pausePanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Resume();
        }

        public void Toggle()
        {
            if (isPaused) Resume(); else Pause();
        }
    }
}