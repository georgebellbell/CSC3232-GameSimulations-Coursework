using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created as the base of my states for the player, will be developed and used for my advanced enemies during part 2
public class BaseState
{
    public string stateName;
    protected StateMachine stateMachine;

    //constructor
    public BaseState(string stateName, StateMachine stateMachine)
    {
        this.stateName = stateName;
        this.stateMachine = stateMachine;
    }

    // A set of virtual methods that are overwritten depending on the needs of a given state

    // method called when statemachine enters that state
    public virtual void EnterState()
    {

    }

    // similar to monobehaviour's update, but for that state
    public virtual void LogicUpdate()
    {
       

    }

    // similar to monobehaviour's fixed update, but for that state
        public virtual void PhysicsFixedUpdate()
    {

    }

    // method called when statemachine leaves that state
    public virtual void ExitState()
    {

    }


    // Virtual colliders for the monobheviour collision methods
    public virtual void OnTriggerCollision()
    {

    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        
    }

    public virtual void OnTriggerExit(Collider collision)
    {

        
    }

}
