using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToComplete;
    [SerializeField] TextMeshProUGUI timerText;

    ManagementSystem managementSystem;

    PickupGenerator pickupGenerator;

    Planet.PlanetType planetType;
    // Start is called before the first frame update
    void Start()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        pickupGenerator = GetComponent<PickupGenerator>();
        planetType = GetComponent<Planet>().thisPlanetType;

        if (planetType == Planet.PlanetType.puzzle)
        {
            StartCoroutine(PuzzleCountdown());
        }
        else if (planetType == Planet.PlanetType.survival)
        {
            StartCoroutine(SurvivalCountdown());
        }
        
    }

    IEnumerator PuzzleCountdown()
    {
        timerText.text = "Time left: " + timeToComplete;

        yield return new WaitForSeconds(1f);

        timeToComplete -= 1;

        if (timeToComplete < 0)
        { 
            StartCoroutine(LoseGame());     
        }
        else
        {
            StartCoroutine(PuzzleCountdown());
        }
    }

    IEnumerator SurvivalCountdown()
    {
        timerText.text = "Time left: " + timeToComplete;

        yield return new WaitForSeconds(1f);

        pickupGenerator.ChangeJumpChance(0.1);
        pickupGenerator.ChangeSpeedChance(0.05);

        timeToComplete -= 1;

        if (timeToComplete < 0)
        {
            managementSystem.WinGame();
        }
        else
        {
            StartCoroutine(SurvivalCountdown());
        }
    }


    IEnumerator LoseGame()
    {
        // death animation
        // death sound
        FindObjectOfType<RoverController>().enabled = false;
        yield return new WaitForSeconds(1f);
        FindObjectOfType<ManagementSystem>().LoseGame();

    }

    public float GetGameTime()
    {
        return timeToComplete;
    }
}
