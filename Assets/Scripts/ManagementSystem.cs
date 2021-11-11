using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the state of the game, stopping it when player wins, loses or pauses it
public class ManagementSystem : MonoBehaviour
{
    [SerializeField] GameObject pausedMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    [SerializeField] GameObject survivalControls;
    [SerializeField] GameObject puzzleControls;


    int currentScene;
    bool gameIsPaused;

    bool gameFinished;

    
    void Start()
    {
        Time.timeScale = 1;
        pausedMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);

        if (FindObjectOfType<Planet>().thisPlanetType == Planet.PlanetType.survival)
        {
            survivalControls.SetActive(true);
        }
        else if (FindObjectOfType<Planet>().thisPlanetType == Planet.PlanetType.puzzle)
        {
            puzzleControls.SetActive(true);
        }

        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Called in rover controller and toggles the game between a play and paused state
    public void TogglePause()
    {
        if (gameFinished)
            return;

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

    // For Puzzle levels, this function is called via PuzzlePlanet.CS when all points are activated
    // For Survival levels, this function is called via Timer.CS when the timer finishes

    public void WinGame()
    {
        winMenu.SetActive(true);
        Time.timeScale = 0;
        
    }

    // For Puzzle levels, this is called via Timer.CS when timer runs out before all points are activated
    // For Survival levels, this is called via the player's Health.CS when player health drops below 0
    public void LoseGame()
    {
        loseMenu.SetActive(true);
        Time.timeScale = 0;
        
    }

    // Brings up Paused UI and stops time in game
    // Retains previous state of mouse as it differs for survival and puzzle
    private void PauseGame()
    {
        pausedMenu.SetActive(true);
        Time.timeScale = 0;
    }


    // All the functions below are attached to buttons in seperate canvases in the scenes

    // Disables Paused UI and resets time
    public void ResumeGame()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Reload the current scene
    public void RestartLevel()
    {
        SceneManager.LoadScene(currentScene);
    }

    // Starts next level
    // Currently not attached to an object as I intend to develop the overworld scene in part 2 to include pathfinding to find a set
    // of planets to traverse between
    public void LoadNextLevel()
    {
        int nextScene = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }

    // Returns player back to the main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // If game were to be built, there is functionality to quit
    public void QuitGame()
    {
        Application.Quit();
    }



}
