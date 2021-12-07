using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGent : MonoBehaviour
{
    NavMeshAgent agent;
    RoverStateMachine rover;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rover = FindObjectOfType<RoverStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(rover.transform.position);
    }
}
