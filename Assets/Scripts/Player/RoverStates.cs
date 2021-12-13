using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inherits from BaseState and is the basis for all rover states
// two states directly inherit from this: Puzzle and Survival
public class RoverStates : BaseState
{
    private RoverStateMachine rover_sm;

    public RoverStates(string name, RoverStateMachine stateMachine) : base(name, stateMachine)
    {
        rover_sm = stateMachine;
    }

    // function is overriden to include checks for specific key presses and user actions that are present in each main sub state
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CalculateDirection();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpacebarPressed();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapePressed();
        }

    }

    // virtual method to determine movement of player, overriden in SurvivalState, PuzzleState and JumpPowerup State
    public virtual void CalculateDirection()
    {
        
    }

    // by default, this allows the player to jump but is overriden in SpeedPowerupState for an alternate use of the function as described there
    public virtual void SpacebarPressed()
    {
        if (!rover_sm.isJumping)
        {
            rover_sm.isJumping = true;
            rover_sm.startJumping = true;

            // Added a special effect and sound for the rover jumping in the form of an animation and and a boing sound effect that plays at the rover's location
            rover_sm.roverAnimator.SetTrigger("StartJumping");
            AudioSource.PlayClipAtPoint(rover_sm.jumpSound, rover_sm.transform.position);
        }
    }

    // by default, the fixed update of the rover state will try to change poistion based on changes and check if it should be jumping
    public override void PhysicsFixedUpdate()
    {
        base.PhysicsFixedUpdate();
        rover_sm.rigidbody.MovePosition(rover_sm.rigidbody.position +
            rover_sm.transform.TransformDirection(rover_sm.movementDirection) *
            rover_sm.currentMovementSpeed * Time.fixedDeltaTime);


        // implemented an animation effect where the wheels will turn with the direction the rover is moving in
        if(rover_sm.movementDirection.x < 0)
        {
            rover_sm.roverAnimator.SetTrigger("MoveLeft");
        }
        else if (rover_sm.movementDirection.x > 0)
        {
            rover_sm.roverAnimator.SetTrigger("MoveRight");
        }
        else
        {
            rover_sm.roverAnimator.SetTrigger("NotTurning");
        }
        
        

        // if player is about to start jumping, they will jump and then set value to false
        if (rover_sm.startJumping)
        {
            rover_sm.startJumping = false;
            rover_sm.rigidbody.AddForce(rover_sm.transform.up * rover_sm.currentJumpPower * Time.deltaTime, ForceMode.Impulse);
        }

        Vector3 gravityDirection = (rover_sm.transform.position - MainToolbox.planetTransform.position).normalized;

        float dotProduct = Vector3.Dot(gravityDirection, rover_sm.transform.up);

        // if the rover has reached max height, new animation effect will play that returns it to its default size
        if (rover_sm.isJumping && dotProduct == 1f)
        {
            rover_sm.roverAnimator.SetTrigger("JumpHeightReached");
        }


    }

    // when in a rover state, game can be paused and UI appear. Overwritten by MenuState due to it not being needed there
    public virtual void EscapePressed()
    {
        rover_sm.managementSystem.TogglePause();
    }

    // player will not be able to jump again until isJumping returns to being false
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Planet"))
        {
            rover_sm.isJumping = false;
            rover_sm.roverAnimator.SetTrigger("JumpHeightReached");
        }
    }


    public override void ExitState()
    {
        base.ExitState();
        rover_sm.previousState = this;
    }
}
