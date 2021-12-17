using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RoverStateMachine : StateMachine
{
    // A set of variables that help control the players movement, jumping and interaction with objects
    [Header("Different Rover States")]
    public NoPowerupState noPowerupState;
    public JumpPowerupState jumpPowerupState;
    public SpeedPowerupState speedPowerupState;
    public PuzzleState puzzleState;
    public MenuState menuState;
    public BoidsState boidsState;
    public CoinState coinState;
    public BaseState previousState;

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
    public Animator roverAnimator;

    public bool isJumping = false;
    public bool startJumping = false;

    public ManagementSystem managementSystem;
    public PickupGenerator pickupGenerator;

    public PostProcessVolume ppVolume;
    public ChromaticAberration powerupEffect;

    public Vector3 movementDirection;

    public int healthIncrease = 20;
    public Health playerHealth;

    public Material defaultMaterial;
    public GameObject roverBody;

    [Header("Sound Effects")]
    public AudioClip jumpSound;
    public AudioClip explosionSound;

    public ParticleSystem explosionPS;


    // initialises all the states the player can be in and finds certain variables that will be needed later
    private void Awake()
    {
        noPowerupState = new NoPowerupState(this);
        jumpPowerupState = new JumpPowerupState(this);
        speedPowerupState = new SpeedPowerupState(this);
        puzzleState = new DefaultPuzzleState(this);
        menuState = new MenuState(this);
        boidsState = new BoidsState(this);
        coinState = new CoinState(this);
       

        Time.timeScale = 1;
        rigidbody = GetComponent<Rigidbody>();
        managementSystem = FindObjectOfType<ManagementSystem>();
        pickupGenerator = FindObjectOfType<PickupGenerator>();
        ppVolume.profile.TryGetSettings(out powerupEffect);
        playerHealth = GetComponent<Health>();
        roverBody = GameObject.Find("RoverBody");
        defaultMaterial = roverBody.GetComponent<Renderer>().material;
        roverAnimator = GetComponent<Animator>();

        currentMovementSpeed = DefaultMovementSpeed;
        currentJumpPower = DefaultJumpPower;

    }

    // depending on the planet the player is currently on, their initial staes will be set as seen below
    protected override BaseState GetFirstState()
    {

        if (MainToolbox.planetType == Planet.PlanetType.survival)
        {
            return noPowerupState;
        }
        else if (MainToolbox.planetType == Planet.PlanetType.puzzle)
        {
            return puzzleState;
        }
        else if (MainToolbox.planetType == Planet.PlanetType.boids)
        {
            return boidsState;
        }
        else if (MainToolbox.planetType == Planet.PlanetType.coin)
        {
            return coinState;
        }
        else
        {
            return menuState;
        }
    }
}
