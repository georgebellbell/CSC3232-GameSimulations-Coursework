using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlanet : MonoBehaviour
{
    [SerializeField] GameObject planetSelectedOverlay;

    [SerializeField] int sceneIndex;

    [SerializeField] public string planetName;
    [SerializeField] string planetMass;
    [SerializeField] string planetType;
    [SerializeField] bool planetCompleted = false;

    bool planetSelected = false;

    [SerializeField] bool startNode = false;

    OverworldNavigator overworldNavigator;
    GameObject currentOverlay;
    PathfindingNode planetNode;
    AStar aStar;

    [SerializeField] OverworldPlanet[] prerequesitPlanets;

    [SerializeField] bool planetUnlocked;

    

    private void Awake()
    {
        planetNode = GetComponent<PathfindingNode>();
        aStar = FindObjectOfType<AStar>();
        overworldNavigator = FindObjectOfType<OverworldNavigator>();
        PlanetData data = SavingSystem.LoadPlanet(planetName);

        if (data != null)
        {
            planetCompleted = data.planetCompleted;
        }

        
       
    }

    private void Start()
    {
        if (prerequesitPlanets.Length > 0)
        {
            foreach (OverworldPlanet item in prerequesitPlanets)
            {
                if(item.IsCompleted() == false)
                {
                    return;
                }
            }     
        }

        planetUnlocked = true;
    }

    // When mouse is over an object, green overlay object appears to highlight and information about it
    private void OnMouseEnter()
    {
        if (!planetSelected && !startNode && planetUnlocked)
        {
            currentOverlay = Instantiate(planetSelectedOverlay, transform.position, Quaternion.identity);
            currentOverlay.transform.localScale = this.transform.localScale * 1.2f;

            Color color = currentOverlay.GetComponent<Renderer>().material.color;
            color.a = color.a / 2;
            currentOverlay.GetComponent<Renderer>().material.color = color;
        }

        overworldNavigator.SetPlanetStats(planetName, planetType, planetMass, transform, planetCompleted, !planetUnlocked);
    }

    // clears everything made in OnMouseEnter
    private void OnMouseExit()
    {
        if (!planetSelected && !startNode && planetUnlocked)
        {
            Destroy(currentOverlay.gameObject);   
        }

        overworldNavigator.SetPlanetStats();
    }

    // If player clicks on that planet, the game will load a level
    private void OnMouseUp()
    {
        if (planetUnlocked)
        {
            ToggleSelectPlanet();

        }
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

    public void SetCompletion(bool completed)
    {
        planetCompleted = completed;
    }

    public bool IsCompleted()
    {
        return planetCompleted;
    }




}
