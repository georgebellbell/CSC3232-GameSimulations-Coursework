using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public MinMaxState(RoverStateMachine stateMachine) : base("MinMaxState", stateMachine)
    {
        rover_sm = stateMachine;
    }
}
