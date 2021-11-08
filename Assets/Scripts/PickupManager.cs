using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public class PickupTimeleft
    {
        public Pickup.PickupType pickup;
        public float timeLeft;

        public PickupTimeleft(Pickup.PickupType pickupType)
        {
            pickup = pickupType;
            timeLeft = 0;
        }
    }
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material speedPowerupMaterial;
    [SerializeField] Material jumpPowerupMaterial;

    [SerializeField] int healthIncrease = 20;
    Health playerHealth;

    [SerializeField] float speedMultilpier = 2f;
    [SerializeField]float speedPowerupTime = 3;
    public PickupTimeleft speedTimeLeft = new PickupTimeleft(Pickup.PickupType.SpeedPickup);
    //float speedTimeLeft = 0;

    [SerializeField] float jumpMultilpier = 1.5f;
    [SerializeField] float jumpPowerupTime = 5;
    public PickupTimeleft jumpTimeLeft = new PickupTimeleft(Pickup.PickupType.SpeedPickup);

    RoverController playerController;
    PickupGenerator pickupGenerator;
    GameObject roverBody;

    private void Start()
    {
        roverBody = GameObject.Find("RoverBody");

        defaultMaterial = roverBody.GetComponent<Renderer>().material;

        pickupGenerator = FindObjectOfType<PickupGenerator>();
    }

    // If player hits a Pickup, depending on its type defined via Pickup.CS, a different function will be called
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
            playerHealth.GainHealth(healthIncrease);  
        }
    }

    // If a powerup is not active when speed powerup is collected,
    // then player speed is increased briefly and hazard objects are ignored
    private void SpeedPickup()
    {
        if (!PickupActive(jumpTimeLeft) && !PickupActive(speedTimeLeft))
        {
            pickupGenerator.ChangeSpeedChance(-1);

            speedTimeLeft.timeLeft += speedPowerupTime;
            playerController = GetComponent<RoverController>();

            float newSpeed = playerController.DefaultMovementSpeed * speedMultilpier;
            playerController.SetCurrentSpeed(newSpeed);
            roverBody.GetComponent<Renderer>().material = speedPowerupMaterial;

            StartCoroutine(ReduceTimer(speedTimeLeft));
  
        }
        
    }

    // if a powerup is not active when jump powerup is collected, player jump power will be doubled 
    private void JumpPickup()
    {
        if (!PickupActive(jumpTimeLeft) && !PickupActive(speedTimeLeft))
        {
            // picking up a jump pickup reduces the chance that it will spawn again for a while
            pickupGenerator.ChangeJumpChance(-1);

            jumpTimeLeft.timeLeft += jumpPowerupTime;
            playerController = GetComponent<RoverController>();

            float newJumpPower = playerController.DefaultJumpPower * jumpMultilpier;
            playerController.SetCurrentJumpPower(newJumpPower);

            roverBody.GetComponent<Renderer>().material = jumpPowerupMaterial;

            StartCoroutine(ReduceTimer(jumpTimeLeft));
        }
    }


    // reduces time of power up and if time runs out, player returns to normal state
    IEnumerator ReduceTimer(PickupTimeleft pickupTimeleft)
    {
        yield return new WaitForSeconds(1f);

        if (PickupActive(pickupTimeleft))
        {
            pickupTimeleft.timeLeft--;
            StartCoroutine(ReduceTimer(pickupTimeleft));
        }
        else
        {
            if (pickupTimeleft.pickup == Pickup.PickupType.SpeedPickup)
            {
                StopSpeedPowerup();
            }
            else if (pickupTimeleft.pickup == Pickup.PickupType.SpeedPickup)
            {
                StopJumpPowerup();
            }
        }


    }

    // Checks if passed powerup is still active
    public bool PickupActive(PickupTimeleft pickup)
    {
        return pickup.timeLeft > 0;
    }
    

    // if the powerup is no longer active, the player's original values are assumed 


    private void StopSpeedPowerup()
    {
        playerController = GetComponent<RoverController>();

        playerController.SetCurrentSpeed(playerController.DefaultMovementSpeed);

        roverBody.GetComponent<Renderer>().material = defaultMaterial;
    }

    
    private void StopJumpPowerup()
    {
        playerController = GetComponent<RoverController>();

        playerController.SetCurrentJumpPower(playerController.DefaultJumpPower);

        roverBody.GetComponent<Renderer>().material = defaultMaterial;
    }
}
