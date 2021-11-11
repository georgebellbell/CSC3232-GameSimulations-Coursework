using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToComplete;
    [SerializeField] TextMeshProUGUI timerText;

    ManagementSystem managementSystem;
    PickupGenerator pickupGenerator;
    Planet.PlanetType planetType;

    void Start()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        planetType = GetComponent<Planet>().thisPlanetType;
        pickupGenerator = GetComponent<PickupGenerator>();
        timerText.text = "Time left: " + timeToComplete;
        StartCoroutine(Countdown());
    }

    // Called at start of game and by itself if timer has not run 
    IEnumerator Countdown()
    {

        if (planetType == Planet.PlanetType.survival)
        {
            // As time progresses, the chance of a certain pickup spawining increases
            pickupGenerator.ChangeHealthChance(0.2);
            pickupGenerator.ChangeJumpChance(0.15);
            pickupGenerator.ChangeSpeedChance(0.1);
        }
        

        yield return new WaitForSeconds(1f);

        timeToComplete -= 1;

        timerText.text = "Time left: " + timeToComplete;

        // Depending on planet state, game will enter a win or lose state when timer runs out
        if (timeToComplete <= 0)
        {
            if (planetType == Planet.PlanetType.puzzle)
            {
                StartCoroutine(LoseGame());
            }
            else if (planetType == Planet.PlanetType.survival)
            {
                managementSystem.WinGame();
            }         
        }
        else
        {
            StartCoroutine(Countdown());
        }
    }

    // Death state is a coroutine that will be developed in part 2
    IEnumerator LoseGame()
    {
        // death animation
        // death sound
        FindObjectOfType<RoverStateMachine>().enabled = false;
        yield return new WaitForSeconds(1f);
        FindObjectOfType<ManagementSystem>().LoseGame();

    }

    public float GetGameTime()
    {
        return timeToComplete;
    }
}
