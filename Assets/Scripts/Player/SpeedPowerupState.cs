using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerupState : PowerupState
{
    private RoverStateMachine rover_sm;

    bool sizeIncreased;
    public SpeedPowerupState(RoverStateMachine stateMachine) : base("SpeedPowerupState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // overrides the use of the spacebar so that when the player uses it, the rover will toggle between two sizes
    // allows player to destroy more craters due to colliders dynamically changing over time but also adds a gameplay 
    // challenge where if they don't return back to normal by the time the powerup is deactivated, the chance of them
    // taking damage is easier
    public override void SpacebarPressed()
    {
        if (sizeIncreased)
        {
            ReduceSize();
            sizeIncreased = false;
        }
        else
        {
            IncreaseSize();
            sizeIncreased = true;
        }
       
    }

    private void IncreaseSize()
    {
        //rover_sm.transform.localScale = rover_sm.transform.localScale  * 2;
        rover_sm.jumpAnimator.SetTrigger("IncreaseSize");
    }

    private void ReduceSize()
    {
        //rover_sm.transform.localScale = rover_sm.transform.localScale * 0.5f;
        rover_sm.jumpAnimator.SetTrigger("DecreaseSize");
    }

    // Sets the time for this powerup
    public override float SetPowerupTime()
    {
        return rover_sm.speedPowerupTime;
    }

    // during the enterstate function of powerup this is called and increases the player speed
    public override void ActivatePowerup()
    {
        rover_sm.currentMovementSpeed = rover_sm.DefaultMovementSpeed * rover_sm.speedMultilpier;
    }

    // player speed is reduced to normal
    public override void ExitState()
    {
        base.ExitState();
        rover_sm.currentMovementSpeed = rover_sm.DefaultMovementSpeed;
    }
}
