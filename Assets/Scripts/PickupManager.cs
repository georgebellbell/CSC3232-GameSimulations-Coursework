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
    [SerializeField] float speedTimeLeft = 0;
    float speedPowerupTime = 5;

    [SerializeField] float jumpMultilpier = 1.5f;
    [SerializeField] float jumpTimeLeft = 0;
    float jumpPowerupTime = 5;

    RoverController playerController;
    PickupGenerator pickupGenerator;
    GameObject roverBody;

    private void Start()
    {
        roverBody = GameObject.Find("RoverBody");

        defaultMaterial = roverBody.GetComponent<Renderer>().material;

        pickupGenerator = FindObjectOfType<PickupGenerator>();
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
                        HealthPickup();
                    }
                    break;
                case Pickup.PickupType.SpeedPickup:
                    {
                        SpeedPickup();
                        
                    }
                    break;
                case Pickup.PickupType.JumpPickup:
                    {
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
        if (!JumpPickupActive())
        {
            pickupGenerator.ChangeSpeedChance(-1);
            if (SpeedPickupActive())
            {
                speedTimeLeft += speedPowerupTime;
                return;  
            }

            speedTimeLeft += speedPowerupTime;
            playerController = GetComponent<RoverController>();

            float newSpeed = playerController.DefaultMovementSpeed * speedMultilpier;
            playerController.SetCurrentSpeed(newSpeed);
            roverBody.GetComponent<Renderer>().material = speedPowerupMaterial;

            StartCoroutine(ReduceSpeedTimer());
           
           
        }
        
    }

    
    IEnumerator ReduceSpeedTimer()
    {
        yield return new WaitForSeconds(1f);

        if (SpeedPickupActive())
        {
            speedTimeLeft = speedTimeLeft - 1;
            StartCoroutine(ReduceSpeedTimer());
        }
        else
        {
            StopSpeedPowerup();
        }
    }

    public bool SpeedPickupActive()
    {
        return speedTimeLeft > 0.0f;
    }


    private void StopSpeedPowerup()
    {
        playerController = GetComponent<RoverController>();

        playerController.SetCurrentSpeed(playerController.DefaultMovementSpeed);

        roverBody.GetComponent<Renderer>().material = defaultMaterial;
    }

    // JUMP POWERUP
    private void JumpPickup()
    {
        if (!SpeedPickupActive())
        {
            pickupGenerator.ChangeSpeedChance(1);
            if (JumpPickupActive())
            {
                jumpTimeLeft += jumpPowerupTime;
                return;      
            }

            jumpTimeLeft += jumpPowerupTime;
            playerController = GetComponent<RoverController>();

            float newJumpPower = playerController.DefaultJumpPower * jumpMultilpier;
            playerController.SetCurrentJumpPower(newJumpPower);

            roverBody.GetComponent<Renderer>().material = jumpPowerupMaterial;

            StartCoroutine(ReduceJumpTimer());  
        }
    }

    IEnumerator ReduceJumpTimer()
    {
        yield return new WaitForSeconds(1f);

        if (JumpPickupActive())
        {
            jumpTimeLeft = jumpTimeLeft - 1;
            StartCoroutine(ReduceJumpTimer());
        }
        else
        {
            StopJumpPowerup();
        }


    }
    public bool JumpPickupActive()
    {
        return jumpTimeLeft > 0.0f;
    }

    private void StopJumpPowerup()
    {
        playerController = GetComponent<RoverController>();

        playerController.SetCurrentJumpPower(playerController.DefaultJumpPower);

        roverBody.GetComponent<Renderer>().material = defaultMaterial;
    }
}
