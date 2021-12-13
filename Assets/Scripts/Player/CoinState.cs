using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public CoinState(RoverStateMachine stateMachine) : base("CoinState", stateMachine)
    {
        rover_sm = stateMachine;
    }

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
