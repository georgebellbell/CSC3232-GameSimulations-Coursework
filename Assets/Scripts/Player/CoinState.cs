using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A state for when the rover is on  a pathfinding planet
public class CoinState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public CoinState(RoverStateMachine stateMachine) : base("CoinState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // if the rover hits a coin, that coin will be destroyed and removed from the manager for this gamemode
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Coin"))
        {
            GameObject.Destroy(collision.gameObject);
            GameObject.FindObjectOfType<CoinHuntGameMode>().RemoveCoin(collision.gameObject.GetComponent<Coin>());
        }
    }

}
