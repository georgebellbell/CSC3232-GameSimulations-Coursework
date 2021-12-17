using UnityEngine;

public class JumpPowerupState : PowerupState
{
    private RoverStateMachine rover_sm;
    public JumpPowerupState(RoverStateMachine stateMachine) : base("JumpPowerupState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // This function is overriden to ensure that player can only move forward and jump, a caveat to using the jump powerup
    public override void CalculateDirection()
    {
        rover_sm.movementDirection = Vector3.forward.normalized;
    }

    // the time active for the jump powerup is returned, specified on the state machine
    public override float SetPowerupTime()
    {
        return rover_sm.jumpPowerupTime;
    }

    // one of the functions called during on enter for the Powerupstate, increases the players jump power
    public override void ActivatePowerup()
    {
        base.ActivatePowerup();
        rover_sm.currentJumpPower = rover_sm.DefaultJumpPower * rover_sm.jumpMultilpier;
    }

    // upon leaving the powerup state, the jump power of the player is also returned to normal
    public override void ExitState()
    {
        base.ExitState();
        rover_sm.currentJumpPower = rover_sm.DefaultJumpPower;
    }
}
