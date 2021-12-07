using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// default state for the player while in survival mode
public class NoPowerupState : SurvivalState
{
    private RoverStateMachine rover_sm;
    public NoPowerupState(RoverStateMachine stateMachine) : base("NoPowerupState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // if player hits a crater, their movement will be partially reduced
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("Crater"))
        {
            rover_sm.currentMovementSpeed = Mathf.Max(rover_sm.currentMovementSpeed * 0.8f, rover_sm.DefaultMovementSpeed / 2);
        }
    }

    // upon exiting the collision with that crater, their speed is returned to normal
    public override void OnTriggerExit(Collider collision)
    {
        base.OnTriggerExit(collision);
        if (collision.gameObject.CompareTag("Crater"))
        {
            rover_sm.currentMovementSpeed = rover_sm.DefaultMovementSpeed;
        }
    }

    // while in the no powerup state, the player can pickup a powerup and will transition to either
    // the jump or speed powerup state depending on the pickup
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Pickup"))
        {
            Pickup pickup = collision.gameObject.GetComponent<Pickup>();
            Material pickupMaterial = collision.transform.GetComponent<Renderer>().material;

            GameObject.Destroy(collision.gameObject);

            //upon hitting a powerup the colour of the rover will change to reflect this
            if (pickup.ActivePickupType != Pickup.PickupType.HealthPickup)
            {
                rover_sm.roverBody.GetComponent<Renderer>().material = pickupMaterial;
            }

            switch (pickup.ActivePickupType)
            {
                case Pickup.PickupType.SpeedPickup:
                    {
                        // when the pickup is collected, the probability of that pickup spawning next will be reduced
                        rover_sm.pickupGenerator.ChangeSpeedChance(-1);
                        stateMachine.EnterNewState(rover_sm.speedPowerupState);
                    }
                    break;
                case Pickup.PickupType.JumpPickup:
                    {
                        rover_sm.pickupGenerator.ChangeJumpChance(-1);
                        stateMachine.EnterNewState(rover_sm.jumpPowerupState);     
                    }
                    break;
            }
        }
        if (collision.gameObject.CompareTag("ForceField"))
        {
            Debug.Log("You hit your tail and died");
        }
    }
}
