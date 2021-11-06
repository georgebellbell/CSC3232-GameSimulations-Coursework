using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    //public GravityAttractor[] planets;
    //public int currentPlanet = 0;
    [SerializeField] Planet currentPlanet;
    private Transform myTransform;

    public float objectMass = 10;

    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;
        myTransform = transform;
    }



    void FixedUpdate()
    {
        currentPlanet.Attract(myTransform, objectMass);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Planet") && gameObject.tag == "Player")
        {
            RoverController playerController = gameObject.GetComponent<RoverController>();
            if(playerController.enabled == false)
            {
                playerController.enabled = true;
            }
        }
    }
    public void SetCurrentAttractor(Planet newGravityAttractor)
    {
        currentPlanet = newGravityAttractor;
    }
}
