using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float DefaultMovementSpeed = 8f;
    float currentMovementSpeed;

    public float DefaultJumpPower = 350f;
    float currentJumpPower;

    private Vector3 movementDirection;
    private Rigidbody rigidbody;
    ManagementSystem managementSystem;
    PickupManager pickupManager;

    public bool isJumping = false;
    public bool startJumping = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        managementSystem = FindObjectOfType<ManagementSystem>();
        pickupManager = FindObjectOfType<PickupManager>();

        currentMovementSpeed = DefaultMovementSpeed;
        currentJumpPower = DefaultJumpPower;
    }

    void Update()
    {
        
        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 1).normalized;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && pickupManager.IsSpeedPickupActive())
        {
            isJumping = true;
            startJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            managementSystem.TogglePause();
        }
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(movementDirection) * currentMovementSpeed * Time.fixedDeltaTime);
        //rigidbody.AddForce(movementDirection * movementSpeed * Time.fixedDeltaTime);

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

        if (collision.gameObject.CompareTag("Crater"))
        {
            currentMovementSpeed = currentMovementSpeed - 3.5f;
        }
    }


    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("Crater"))
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
