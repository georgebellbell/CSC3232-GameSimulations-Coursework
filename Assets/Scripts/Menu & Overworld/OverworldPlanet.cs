using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlanet : MonoBehaviour
{
    [SerializeField] GameObject planetSelectedOverlay;

    [SerializeField] int sceneIndex;

    [SerializeField] string planetName;
    [SerializeField] string planetMass;
    [SerializeField] string planetType;

    bool planetSelected = false;

    OverworldNavigator overworldNavigator;
    GameObject currentOverlay;
    PathfindingNode planetNode;
    AStar aStar;

    LevelManager levelManager;

    private void Start()
    {
        planetNode = GetComponent<PathfindingNode>();
        aStar = FindObjectOfType<AStar>();
        overworldNavigator = FindObjectOfType<OverworldNavigator>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // When mouse is over an object, green overlay object appears to highlight and information about it
    private void OnMouseEnter()
    {
        if (!planetSelected)
        {
            currentOverlay = Instantiate(planetSelectedOverlay, transform.position, Quaternion.identity);
            currentOverlay.transform.localScale = this.transform.localScale * 1.2f;

            Color color = currentOverlay.GetComponent<Renderer>().material.color;
            color.a = color.a / 2;
            currentOverlay.GetComponent<Renderer>().material.color = color;
        }

        overworldNavigator.SetPlanetStats(planetName, planetType, planetMass, transform);
    }

    // clears everything made in OnMouseEnter
    private void OnMouseExit()
    {
        if (!planetSelected)
        {
            Destroy(currentOverlay.gameObject);   
        }

        overworldNavigator.SetPlanetStats();
    }

    // If player clicks on that planet, the game will load a level
    private void OnMouseUp()
    {
        ToggleSelectPlanet();
        //LoadPlanet();
    }

    public void ToggleSelectPlanet()
    {
        
        planetSelected = !planetSelected;

        if (!planetSelected)
        {
            aStar.RemoveTarget();
            Destroy(currentOverlay.gameObject);
        }
        else
        {
            aStar.SetTarget(planetNode);

            if (currentOverlay != null)
            {
                Color color = currentOverlay.GetComponent<Renderer>().material.color;
                color.a = color.a * 2;
                currentOverlay.GetComponent<Renderer>().material.color = color;
            }
            else
            {
                currentOverlay = Instantiate(planetSelectedOverlay, transform.position, Quaternion.identity);
                currentOverlay.transform.localScale = this.transform.localScale * 1.2f;
            }

        }
    }

    public int GetPlanetIndex()
    {
        return sceneIndex;
    }




}
