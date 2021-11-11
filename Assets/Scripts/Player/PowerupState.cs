using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from parent state and has two child states: jump and speed
public class PowerupState : SurvivalState
{
    private RoverStateMachine rover_sm;
    float powerupTimeLeft;
    public PowerupState(string name, RoverStateMachine stateMachine) : base(name, stateMachine)
    {
        rover_sm = stateMachine;
    }

    // when transitioning from nopowerup state to powerup state, the below methods are called
    public override void EnterState()
    {
        base.EnterState();
        ActivatePowerup();
        powerupTimeLeft = SetPowerupTime();
        rover_sm.StartCoroutine(Countdown());
    }

    // virtual method that is overridden by the jump and speed states with the action that will be performed
    public virtual void ActivatePowerup()
    {

    }

    // overriden by child states with the time that powerup will be available for
    public virtual float SetPowerupTime()
    {
        return 0f;
    }

    
    // Looping coroutine that counts down the time that powerup has left active, if it runs out, the statemachine of the player returns to noPowerup
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1f);

        if (PowerupActive())
        {
            powerupTimeLeft--;
            rover_sm.StartCoroutine(Countdown());
        }
        else
        {
            stateMachine.EnterNewState(rover_sm.noPowerupState);
        }
    }

    // when leaving that state, the player returns to its default colour
    public override void ExitState()
    {
        base.ExitState();
        rover_sm.roverBody.GetComponent<Renderer>().material = rover_sm.defaultMaterial;
    }

    bool PowerupActive()
    {
        return powerupTimeLeft > 0;
    }

    // Collider around entire rover will destroy any Meteor objects that collide with it if the rover has a powerup active (immunity)

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Meteor"))
        {
            GameObject.Destroy(collision.gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Pickup"))
        {
            Pickup.PickupType pickup = collision.gameObject.GetComponent<Pickup>().ActivePickupType;
            if (pickup != Pickup.PickupType.HealthPickup)
            {
                GameObject.Destroy(collision.gameObject);
            }
        }
    }


    // Similar to OnCollisionEnter but for the Crater objects
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crater"))
        {
            GameObject.Destroy(other.gameObject);
        }
    }
}
