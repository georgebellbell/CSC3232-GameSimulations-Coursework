using UnityEngine;

public class OverworldPlanet : MonoBehaviour
{
    [SerializeField] OverworldPlanet[] prerequesitPlanets;
    [SerializeField] GameObject planetSelectedOverlay;
    [SerializeField] int sceneIndex;

    [SerializeField] public string planetName;
    [SerializeField] string planetMass;
    [SerializeField] string planetType;
    [SerializeField] bool planetUnlocked;
    bool planetCompleted = false;

    bool planetSelected = false;

    OverworldNavigator overworldNavigator;
    GameObject currentOverlay;
    OverworldNode planetNode;
    OverworldPathfinding aStar;

    private void Awake()
    {
        planetNode = GetComponent<OverworldNode>();
        aStar = FindObjectOfType<OverworldPathfinding>();
        overworldNavigator = FindObjectOfType<OverworldNavigator>();

        // Game checks for saved data for this planet, and if so, whether this planet has been beaten
        PlanetData data = SavingSystem.LoadPlanet(planetName);
        if (data != null)
        {
            planetCompleted = data.planetCompleted;
        }
    }

    // Added gameplay progression in the form of prequesit planets that must be beaten before the player can player this one
    // if a planet's list of uncompleted planets, its functionality is limited and cannot be selected or played until other planets have been beaten
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

    // When mouse is over an unlocked planet, green overlay object appears to highlight and information about it
    private void OnMouseEnter()
    {
        if (!planetSelected &&  planetUnlocked)
        {
            currentOverlay = Instantiate(planetSelectedOverlay, transform.position, Quaternion.identity);
            currentOverlay.transform.localScale = this.transform.localScale * 1.2f;

            Color color = currentOverlay.GetComponent<Renderer>().material.color;
            color.a = color.a / 2;
            currentOverlay.GetComponent<Renderer>().material.color = color;
        }

        overworldNavigator.SetPlanetStats(planetName, planetType, planetMass, transform, planetCompleted, !planetUnlocked);
    }

    // Clears everything made in OnMouseEnter, unless it is a locked planet
    private void OnMouseExit()
    {
        if (!planetSelected && planetUnlocked)
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
    }

    // able to toggle on and off if you want to select a level you want to play
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

            // Creates an overlay for the planet, giving a feedback effect
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
