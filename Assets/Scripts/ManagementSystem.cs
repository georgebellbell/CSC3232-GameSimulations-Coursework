using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagementSystem : MonoBehaviour
{
    [SerializeField] GameObject pausedMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    [SerializeField] int currentScene;

    bool gameIsPaused;

    void Start()
    {
        Time.timeScale = 1;
        pausedMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void TogglePause()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void WinGame()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0;
        
    }

    public void LoseGame()
    {
        loseMenu.SetActive(true);
        Time.timeScale = 0;
        
    }

    private void PauseGame()
    {
        pausedMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(currentScene);
    }

    public void LoadNextLevel()
    {
        int nextScene = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }



}
