using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State used when player is on a survival planet
// has three directly inheriting states: Menu, NoPowerup, Powerup
public class SurvivalState : RoverStates
{
    private RoverStateMachine rover_sm;

    public SurvivalState(string name, RoverStateMachine stateMachine) : base(name, stateMachine)
    {
        rover_sm = stateMachine;
    }

    // forces player to always be moving forward, but they can move left and right
    public override void CalculateDirection()
    {
        rover_sm.movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 1).normalized;
    }

    // when in survial mode, when the player interacts with a health pickup they will gain some health, and that object will be destroyed
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Pickup"))
        {
            Pickup pickup = collision.gameObject.GetComponent<Pickup>();

            if (pickup.ActivePickupType == Pickup.PickupType.HealthPickup)
            {
                rover_sm.playerHealth.GainHealth(rover_sm.healthIncrease);
                rover_sm.pickupGenerator.ChangeHealthChance(-1);
            }

            GameObject.Destroy(collision.gameObject);
        }

        
    }
}
