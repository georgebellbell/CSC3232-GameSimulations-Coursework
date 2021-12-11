using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPuzzleState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public DefaultPuzzleState(RoverStateMachine stateMachine) : base("DefaultPuzzleState", stateMachine)
    {
        rover_sm = stateMachine;
    }
    
}
