using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    public int levelCount;

    private int currentLevel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Start()
    {
        levelCount += 1;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void GoToNextLevel()
    {
        if (currentLevel + 1 >= levelCount)
        {
            GoToMainMenu();
            return;
        }

        SceneManager.LoadScene(++currentLevel);
    }

    public void GoToMainMenu()
    {
        currentLevel = 0;
        SceneManager.LoadScene(0);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
}
