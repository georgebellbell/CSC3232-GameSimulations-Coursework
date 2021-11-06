using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlanet : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    [SerializeField] GameObject planetSelectedOverlay;
    [SerializeField] string planetName;
    [SerializeField] string planetMass;
    [SerializeField] string planetType;

    OverworldNavigator overworldNavigator;

    GameObject currentOverlay;

    private void Start()
    {
        overworldNavigator = FindObjectOfType<OverworldNavigator>();
    }

    private void OnMouseEnter()
    {
        currentOverlay = Instantiate(planetSelectedOverlay);
        currentOverlay.transform.position = this.transform.position;
        currentOverlay.transform.localScale = this.transform.localScale * 1.2f;

        overworldNavigator.SetPlanetStats(planetName, planetType, planetMass, transform);
    }

    private void OnMouseExit()
    {
        Destroy(currentOverlay.gameObject);
        overworldNavigator.SetPlanetStats();
    }

    private void OnMouseUp()
    {
        LoadPlanet();
    }

    void LoadPlanet()
    {
        SceneManager.LoadScene(sceneIndex);
    }



}
