using UnityEngine;

public class RoverController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 750;
    [SerializeField, Range(5, 14)] public float DefaultMovementSpeed = 8f;
    float currentMovementSpeed;

    public float DefaultJumpPower = 350f;
    float currentJumpPower;
    bool isJumping = false;
    bool startJumping = false;

    Vector3 movementDirection;
    Rigidbody rigidbody;
    ManagementSystem managementSystem;
    PickupManager pickupManager;

    Planet currentPlanet;

    void Start()
    {
        Time.timeScale = 1;
        rigidbody = GetComponent<Rigidbody>();
        managementSystem = FindObjectOfType<ManagementSystem>();
        pickupManager = FindObjectOfType<PickupManager>();
        currentPlanet = FindObjectOfType<Planet>();

        currentMovementSpeed = DefaultMovementSpeed;
        currentJumpPower = DefaultJumpPower;
    }

    void Update()
    {
        // If planet state is a puzzle planet then player can move in all four directions and rotate
        if (currentPlanet.thisPlanetType == Planet.PlanetType.puzzle)
        {
            movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            ChangeRotation();
        }
        // If the planet state is either menu or survival, player cannot rotate or move backwards and is forced to move forwards at all times
        else 
        {
            movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 1).normalized;   
        }               
        
        // If player is in a grounded state and not in a speed powerup state then they can jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !pickupManager.PickupActive(pickupManager.speedTimeLeft))
        {
            isJumping = true;
            startJumping = true;
        }

        // if planet state is not a menu state then game state can transition to paused state
        if (Input.GetKeyDown(KeyCode.Escape) && currentPlanet.thisPlanetType != Planet.PlanetType.menu)
        {
            managementSystem.TogglePause();
        }
    }

    // uses mouse input to rotate the player while in puzzle state to give more directional control
    private void ChangeRotation()
    {
        float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, xRotation, 0, Space.Self);
        
    }

    // every fixed update the player is moved depending on changes during update
    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(movementDirection) * currentMovementSpeed * Time.fixedDeltaTime);
       
        if (startJumping)
        {
            startJumping = false;
            Jump();
        }
    }

    // Impulse force added if player jumps to cause instantaneous and relative height increase
    void Jump()
    {
        rigidbody.AddForce(transform.up * currentJumpPower * Time.deltaTime, ForceMode.Impulse);
    }

    // If player hits the planet then it means they have stopped jumping and code is updated to reflect this
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            isJumping = false;
        }
    }

    // If player interacts with a crater, their speed will be temporarily slowed until they exit (As seen in OnTrigger Exit)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crater") && !pickupManager.PickupActive(pickupManager.speedTimeLeft))
        {
            currentMovementSpeed = Mathf.Max(currentMovementSpeed * 0.8f, DefaultMovementSpeed / 2);
        }
    }


    private void OnTriggerExit(Collider collision)
    {

        if (collision.gameObject.CompareTag("Crater") && !pickupManager.PickupActive(pickupManager.speedTimeLeft))
        {
            currentMovementSpeed = DefaultMovementSpeed;
        }
    }

    //Public setters called via PickupManager.cs when a pickup is collected
    public void SetCurrentSpeed(float newSpeed)
    {
        currentMovementSpeed = newSpeed;
    }

    public void SetCurrentJumpPower(float newJumpPower)
    {
        currentJumpPower = newJumpPower;
    }
}
