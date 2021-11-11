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

    OverworldNavigator overworldNavigator;
    GameObject currentOverlay;

    private void Start()
    {
        overworldNavigator = FindObjectOfType<OverworldNavigator>();
    }

    // When mouse is over an object, green overlay object appears to highlight and information about it
    private void OnMouseEnter()
    {
        currentOverlay = Instantiate(planetSelectedOverlay, transform.position, Quaternion.identity);
        currentOverlay.transform.localScale = this.transform.localScale * 1.2f;

        overworldNavigator.SetPlanetStats(planetName, planetType, planetMass, transform);
    }

    // clears everything made in OnMouseEnter
    private void OnMouseExit()
    {
        Destroy(currentOverlay.gameObject);
        overworldNavigator.SetPlanetStats();
    }

    // If player clicks on that planet, the game will load a level
    private void OnMouseUp()
    {
        LoadPlanet();
    }

    // in seperate function that will be changed into a coroutine later to allow for effects beforehand like scene transitions
    void LoadPlanet()
    {
        SceneManager.LoadScene(sceneIndex);
    }



}
