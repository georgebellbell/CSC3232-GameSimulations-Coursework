using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour
{

    [SerializeField, Range(5, 14)] public float DefaultMovementSpeed = 8f;
    float currentMovementSpeed;

    public float DefaultJumpPower = 350f;
    float currentJumpPower;

    private Vector3 movementDirection;
    private Rigidbody rigidbody;
    ManagementSystem managementSystem;
    PickupManager pickupManager;

    Planet currentPlanet;

    public bool isJumping = false;
    public bool startJumping = false;

    [SerializeField] float rotationSpeed;

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
        if (currentPlanet.thisPlanetType == Planet.PlanetType.puzzle)
        {
            movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
           
            ChangeRotation();
            

        }
        else
        {
            movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 1).normalized;   
        }
        
        
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !pickupManager.SpeedPickupActive())
        {
            isJumping = true;
            startJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && currentPlanet.thisPlanetType != Planet.PlanetType.menu)
        {
            managementSystem.TogglePause();
        }
    }

    private void ChangeRotation()
    {
        float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, xRotation, 0, Space.Self);
        
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(movementDirection) * currentMovementSpeed * Time.fixedDeltaTime);
       

        if (startJumping)
        {
            startJumping = false;
            Jump();
        }
    }

    void Jump()
    {
        rigidbody.AddForce(transform.up * currentJumpPower * Time.deltaTime, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crater") && !pickupManager.SpeedPickupActive())
        {
            currentMovementSpeed = Mathf.Max(currentMovementSpeed * 0.8f, DefaultMovementSpeed / 2);
        }
    }


    private void OnTriggerExit(Collider collision)
    {

        if (collision.gameObject.CompareTag("Crater") && !pickupManager.SpeedPickupActive())
        {
            currentMovementSpeed = DefaultMovementSpeed;
        }
    }

    public void SetCurrentSpeed(float newSpeed)
    {
        currentMovementSpeed = newSpeed;
    }

    public void SetCurrentJumpPower(float newJumpPower)
    {
        currentJumpPower = newJumpPower;
    }
}
