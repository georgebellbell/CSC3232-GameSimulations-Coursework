using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagementSystem : MonoBehaviour
{
    [SerializeField] GameObject pausedMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    Scene currentScene;

    bool gameIsPaused;
    bool gameEnded;

    void Start()
    {
        Time.timeScale = 1;
        pausedMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        currentScene = SceneManager.GetActiveScene();
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
        FinishGame();
    }

    public void LoseGame()
    {
        loseMenu.SetActive(true);
        FinishGame();
    }

    private void FinishGame()
    {
        Time.timeScale = 0;
        gameEnded = true;
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
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadNextLevel()
    {
        // TO BE IMPLEMENTED
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }



}
