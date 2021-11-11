using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// State used when player is on a puzzle planet
public class PuzzleState : RoverStates
{
    private RoverStateMachine rover_sm;
    public PuzzleState(RoverStateMachine stateMachine) : base("PuzzleState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // same update logic is used as above but also a rotation vector is calculated so player can rotate
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        ChangeRotation();
    }

    // unlike SurvivalState's overriding of CalculateDirection, here the player can move in all four directions
    public override void CalculateDirection()
    {
        rover_sm.movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    // Takes in player mouse input and uses it to rotate the player, and camera, locally
    private void ChangeRotation()
    {
        float xRotation = Input.GetAxis("Mouse X") * rover_sm.rotationSpeed * Time.deltaTime;
        rover_sm.transform.Rotate(0, xRotation, 0, Space.Self);

    }

}
