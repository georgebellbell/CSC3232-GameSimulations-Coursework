using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Will be developed in part 2 to include pathfinding to create path of planets
public class OverworldNavigator : MonoBehaviour
{
    [SerializeField] GameObject selectedCanvas;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI massText;
    [SerializeField] TextMeshProUGUI gameTypeText;
    [SerializeField] GameObject stats;

    [SerializeField] GameObject camera;

    float yChange;
    float xChange;
    float zChange;

    private void Update()
    {
        ChangeCameraPosition();
    }

    // Every frame the game checks if player moving the mouse around to navigate the overworld
    // and updates the camera position occordingly
    private void ChangeCameraPosition()
    {
        Vector3 originalPosition = camera.transform.position;

        // Player can drag across overworld while mousewheel is clicked
        // Limited within a certain range to prevent player getting lost
        if (Input.GetKey(KeyCode.Mouse2))
        {
            xChange = Mathf.Clamp(originalPosition.x - Input.GetAxisRaw("Mouse X"), -10, 10);
            zChange = Mathf.Clamp(originalPosition.z - Input.GetAxisRaw("Mouse Y"), -4, 4);
        }

        // Scrolling mousewheel will move camera closer or further away
        // Clamped within certain range so player can't zoom in or out too much
        yChange = Mathf.Clamp(-Input.GetAxisRaw("Mouse ScrollWheel") + originalPosition.y, 5, 10);
        Vector3 newPosition = new Vector3(xChange, yChange, zChange);

        camera.transform.position = newPosition;
    }

    // Called via OverworldPlanet.cs and will show some information about that planet
    // and enable an aspect of UI that is translated to that position on the screen
    public void SetPlanetStats(string planetName, string planetType, string planetMass, Transform transform)
    {
        nameText.text = planetName;
        massText.text = planetMass;
        gameTypeText.text = planetType;

        selectedCanvas.SetActive(true);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        stats.transform.position = screenPos;

    }

    // Override of above function and called from same object when mouse is no longer over the
    // planet, clears the UI element and hides it
    public void SetPlanetStats()
    {
        nameText.text = "";
        massText.text = "";
        gameTypeText.text = "";

        selectedCanvas.SetActive(false);
    }

    // Attached to button on UI
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }


}
