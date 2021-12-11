using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public BoidsState(RoverStateMachine stateMachine) : base("BoidsState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Boid"))
        {
            GameObject.FindObjectOfType<FlockingEnemyGenerator>().RemoveBoid(collision.gameObject.GetComponent<FlockingEnemy>());
            GameObject.Destroy(collision.gameObject);
        }
    }
}
