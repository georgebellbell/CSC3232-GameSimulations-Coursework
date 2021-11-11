using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherits from survival state as it allows for use of the forward movement in menu
public class MenuState : SurvivalState
{
    private RoverStateMachine rover_sm;
    public MenuState(RoverStateMachine stateMachine) : base("MenuState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // clears use of the escape button from pausing the game as it was not needed
    public override void EscapePressed()
    {
        
    }

}
