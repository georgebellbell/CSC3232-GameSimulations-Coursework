using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverStateMachine : StateMachine
{
    // A set of variables that help control the players movement, jumping and interaction with objects
    [Header("Different Rover States")]
    public NoPowerupState noPowerupState;
    public JumpPowerupState jumpPowerupState;
    public SpeedPowerupState speedPowerupState;
    public PuzzleState puzzleState;
    public MenuState menuState;

    public Rigidbody rigidbody;

    [Header("Rover Speed Attributes")]
    [Range(5, 14)] public float DefaultMovementSpeed = 8f;
    public float currentMovementSpeed;
    public float speedMultilpier = 2f;
    public float speedPowerupTime = 3f;

    public float rotationSpeed = 750;

    [Header("Rover Jump Attributes")]
    public float DefaultJumpPower = 350f;
    public float currentJumpPower;
    public float jumpMultilpier = 1.5f;
    public float jumpPowerupTime = 4f;
    public Animator jumpAnimator;

    public bool isJumping = false;
    public bool startJumping = false;

    public BaseState previousState;

    public ManagementSystem managementSystem;
    public PickupGenerator pickupGenerator;

    public Vector3 movementDirection;

    public int healthIncrease = 20;
    public Health playerHealth;

    public Material defaultMaterial;
    public GameObject roverBody;

    //public Transform planetTransform;

    // initialises all the states the player can be in and finds certain variables that will be needed later
    private void Awake()
    {
        noPowerupState = new NoPowerupState(this);
        jumpPowerupState = new JumpPowerupState(this);
        speedPowerupState = new SpeedPowerupState(this);
        puzzleState = new PuzzleState(this);
        menuState = new MenuState(this);
       

        Time.timeScale = 1;
        rigidbody = GetComponent<Rigidbody>();
        managementSystem = FindObjectOfType<ManagementSystem>();
        pickupGenerator = FindObjectOfType<PickupGenerator>();
        playerHealth = GetComponent<Health>();
        roverBody = GameObject.Find("RoverBody");
        //planetTransform = FindObjectOfType<Planet>().gameObject.transform;
        defaultMaterial = roverBody.GetComponent<Renderer>().material;
        jumpAnimator = GetComponent<Animator>();

        currentMovementSpeed = DefaultMovementSpeed;
        currentJumpPower = DefaultJumpPower;

    }

    // depending on the planet the player is currently on, their initial staes will be set as seen below
    protected override BaseState GetFirstState()
    {
        Planet currentPlanet = FindObjectOfType<Planet>();

        if (currentPlanet.thisPlanetType == Planet.PlanetType.survival)
        {
            return noPowerupState;
        }

        else if (currentPlanet.thisPlanetType == Planet.PlanetType.puzzle)
        {
            return puzzleState;
        }

        else
        {
            return menuState;
        }
    }
}
