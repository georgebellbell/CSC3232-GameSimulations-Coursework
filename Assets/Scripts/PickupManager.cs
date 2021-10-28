using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{

    [SerializeField] Material defaultMaterial;
    [SerializeField] Material speedPowerupMaterial;
    [SerializeField] Material jumpPowerupMaterial;


    [SerializeField] int amountOfHealth = 20;
    Health playerHealth;

    [SerializeField] float speedMultilpier = 2f;
    [SerializeField] float speedPowerupTimeLeft;
    float speedPowerupTime = 5;

    [SerializeField] float jumpMultilpier = 1.5f;
    [SerializeField] float jumpPowerupTimeLeft;
    float jumpPowerupTime = 5;

    PlayerController playerController;

    private void Start()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            Pickup pickup = other.gameObject.GetComponent<Pickup>();

            switch (pickup.ActivePickupType)
            {
                case Pickup.PickupType.HealthPickup:
                    {
                        Debug.Log("HealthPickup");
                        HealthPickup();
                    }
                    break;
                case Pickup.PickupType.SpeedPickup:
                    {
                        Debug.Log("SpeedPickup");
                        SpeedPickup();
                        
                    }
                    break;
                case Pickup.PickupType.JumpPickup:
                    {
                        Debug.Log("JumpPickup");
                        JumpPickup();
                        
                    }
                    break;
            }

            Destroy(other.gameObject);


        }
    }
  

    private void HealthPickup()
    {
        playerHealth = GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.GainHealth(amountOfHealth);
            
        }
    }

    // SPEED POWERUP
    private void SpeedPickup()
    {
        speedPowerupTimeLeft = speedPowerupTime;

        playerController = GetComponent<PlayerController>();

        float newSpeed = playerController.DefaultMovementSpeed * speedMultilpier;
        playerController.SetCurrentSpeed(newSpeed);
        gameObject.GetComponent<Renderer>().material = speedPowerupMaterial;

        StartCoroutine(ReduceSpeedTimer());
    }

    
    IEnumerator ReduceSpeedTimer()
    {
        yield return new WaitForSeconds(1f);

        if (!IsSpeedPickupActive())
        {
            speedPowerupTimeLeft = speedPowerupTimeLeft - 1;
            StartCoroutine(ReduceSpeedTimer());
        }
        else
        {
            StopSpeedPowerup();
        }
    }

    public bool IsSpeedPickupActive()
    {
        return speedPowerupTimeLeft <= 0;
    }


    private void StopSpeedPowerup()
    {
        playerController = GetComponent<PlayerController>();

        playerController.SetCurrentSpeed(playerController.DefaultMovementSpeed);

        gameObject.GetComponent<Renderer>().material = defaultMaterial;
    }

    // JUMP POWERUP
    private void JumpPickup()
    {
        jumpPowerupTimeLeft = jumpPowerupTime;

        playerController = GetComponent<PlayerController>();

        float newJumpPower = playerController.DefaultJumpPower * jumpMultilpier;
        playerController.SetCurrentJumpPower(newJumpPower);

        gameObject.GetComponent<Renderer>().material = jumpPowerupMaterial;

        StartCoroutine(ReduceJumpTimer());
    }


    


    IEnumerator ReduceJumpTimer()
    {
        yield return new WaitForSeconds(1f);

        if (!IsJumpPickupActive())
        {
            jumpPowerupTimeLeft = jumpPowerupTimeLeft - 1;
            StartCoroutine(ReduceJumpTimer());
        }
        else
        {
            StopJumpPowerup();
        }


    }
    public bool IsJumpPickupActive()
    {
        return jumpPowerupTimeLeft <= 0;
    }

    private void StopJumpPowerup()
    {
        playerController = GetComponent<PlayerController>();

        playerController.SetCurrentJumpPower(playerController.DefaultJumpPower);

        gameObject.GetComponent<Renderer>().material = defaultMaterial;
    }
}
