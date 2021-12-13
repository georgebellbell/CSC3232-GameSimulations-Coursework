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
        planetType = MainToolbox.planetType;
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
            if (planetType == Planet.PlanetType.puzzle || planetType == Planet.PlanetType.boids)
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

 
    IEnumerator LoseGame()
    {
        RoverStateMachine rover_sm = FindObjectOfType<RoverStateMachine>();
        rover_sm.explosionPS.Play();
        AudioSource.PlayClipAtPoint(rover_sm.explosionSound, transform.position);
        rover_sm.enabled = false;

        RoverPart[] parts = FindObjectsOfType<RoverPart>();

        foreach (RoverPart part in parts)
        {
            part.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(2f);
        FindObjectOfType<ManagementSystem>().LoseGame();

    }

    public float GetGameTime()
    {
        return timeToComplete;
    }
}
