using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

// Will be developed in part 2 to include pathfinding to create path of planets
public class OverworldNavigator : MonoBehaviour
{
    [SerializeField] GameObject selectedCanvas;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI massText;
    [SerializeField] TextMeshProUGUI gameTypeText;
    [SerializeField] TextMeshProUGUI completedText;
    [SerializeField] GameObject stats;

    

    [SerializeField] GameObject camera;

    GameObject navigationPath;
    LineRenderer lineRenderer;
    [SerializeField] Material lineMaterial;
    [SerializeField] float lineWidth;

    List<int> planetIndexes = new List<int>();
    List<OverworldPlanet> planets = new List<OverworldPlanet>();

    float yChange;
    float xChange;
    float zChange;

    LevelManager levelManager;

    

    private void Start()
    {
        navigationPath = Instantiate(new GameObject());

        lineRenderer = navigationPath.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = lineMaterial;

        levelManager = FindObjectOfType<LevelManager>();
    }
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
        if (Input.GetKey(KeyCode.Mouse0))
        {
            xChange = Mathf.Clamp(originalPosition.x - Input.GetAxisRaw("Mouse X"), -15, 15);
            zChange = Mathf.Clamp(originalPosition.z - Input.GetAxisRaw("Mouse Y"), -8, 8);
        }

        // Scrolling mousewheel will move camera closer or further away
        // Clamped within certain range so player can't zoom in or out too much
        yChange = Mathf.Clamp(-Input.GetAxisRaw("Mouse ScrollWheel") + originalPosition.y, 20, 40);
        Vector3 newPosition = new Vector3(xChange, yChange, zChange);

        camera.transform.position = newPosition;
    }

    // Called via OverworldPlanet.cs and will show some information about that planet
    // and enable an aspect of UI that is translated to that position on the screen
    public void SetPlanetStats(string planetName, string planetType, string planetMass, Transform transform, bool isCompleted)
    {
        nameText.text = planetName;
        massText.text = planetMass;
        gameTypeText.text = planetType;
        completedText.enabled = isCompleted;
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

    public void NewPlanetAdded(OverworldPlanet newPlanet)
    {

        int newPlanetIndex = newPlanet.GetPlanetIndex();

        if (planetIndexes.Capacity != 0)
        {
            foreach (int planetIndex in planetIndexes)
            {
                if (planetIndex == newPlanetIndex)
                {
                    Debug.Log(planetIndex);
                    planetIndexes.Remove(planetIndex);
                    planets.Remove(newPlanet);
                    CheckNumberOfSelectedPlanets();
                    
                    return;
                }
            }
        }

        planetIndexes.Add(newPlanetIndex);
        planets.Add(newPlanet);

        CheckNumberOfSelectedPlanets();
    }

    private void CheckNumberOfSelectedPlanets()
    {
       
        
            
        lineRenderer.positionCount = planets.Count;
        for (int i = 1; i < planets.Count; i++)
        {
            Debug.Log(planets[i].transform.position);
            lineRenderer.SetPosition(i, planets[i].transform.position);
            lineRenderer.SetPosition(i - 1, planets[i - 1].transform.position);
        }

        bool enoughLevelsSelected = planetIndexes.Count >= 2;
        
        
    }

    
}
