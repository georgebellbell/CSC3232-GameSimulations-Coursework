using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    //public GravityAttractor[] planets;
    //public int currentPlanet = 0;
    [SerializeField] GravityAttractor currentAttractor;
    private Transform myTransform;

    public float objectMass = 10;

    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;
        myTransform = transform;
    }


    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.T) && gameObject.tag == "Player")
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
            if (currentPlanet == planets.Length - 1)
            {
                currentPlanet = 0;
            }
            else
            {
                currentPlanet++;
            }
        }
        */
    }
    void FixedUpdate()
    {
        /*
        if (gameObject.tag == "Player")
            currentAttractor = planets[currentPlanet];
            */
        currentAttractor.Attract(myTransform, objectMass);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet") && gameObject.tag == "Player")
        {
            PlayerController playerController = gameObject.GetComponent<PlayerController>();
            if(playerController.enabled == false)
            {
                playerController.enabled = true;
            }
        }
    }
    public void SetCurrentAttractor(GravityAttractor newGravityAttractor)
    {
        currentAttractor = newGravityAttractor;
    }
}
