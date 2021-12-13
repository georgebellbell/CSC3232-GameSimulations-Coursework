// Basic state for the puzzle gamemode planets
public class DefaultPuzzleState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public DefaultPuzzleState(RoverStateMachine stateMachine) : base("DefaultPuzzleState", stateMachine)
    {
        rover_sm = stateMachine;
    }
    
}
