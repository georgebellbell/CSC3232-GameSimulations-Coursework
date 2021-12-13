using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A state for when the rover is on a boid planet
public class BoidsState : PuzzleState
{
    private RoverStateMachine rover_sm;
    public BoidsState(RoverStateMachine stateMachine) : base("BoidsState", stateMachine)
    {
        rover_sm = stateMachine;
    }

    // if the rover hits a boid, that boid will be destroyed and removed from the list of all other boids
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Boid"))
        {
            GameObject.FindObjectOfType<BoidGenerator>().RemoveBoid(collision.gameObject.GetComponent<Boid>());
            GameObject.Destroy(collision.gameObject);
        }
    }
}
