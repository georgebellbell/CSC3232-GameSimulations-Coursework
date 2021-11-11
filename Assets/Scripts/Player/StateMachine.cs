using System.Collections;
using UnityEngine;

// Base state machine that allows an object to switch between states
public class StateMachine : MonoBehaviour
{
    public BaseState currentState;


    // The default methods are called but will work differently depending on the current state active

    void Start()
    {
        currentState = GetFirstState();
        if (currentState != null)
        {
            currentState.EnterState();
        }
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.LogicUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.PhysicsFixedUpdate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != null)
        {
            currentState.OnCollisionEnter(collision);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState != null)
        {
            currentState.OnTriggerEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentState != null)
        {
            currentState.OnTriggerExit(other);
        }
    }

    protected virtual BaseState GetFirstState()
    {
        return null;
    }

    // When entering a new state, the exit function for that state is called, new state is applied and enter for the new state is called
    public void EnterNewState(BaseState newState)
    {
        
        currentState.ExitState();
        
        currentState = newState;
        currentState.EnterState();
    }

        
}
