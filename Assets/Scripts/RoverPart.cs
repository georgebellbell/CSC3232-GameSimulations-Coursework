using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverPart : MonoBehaviour
{
    GameObject parent;
    PickupManager pickupManager;
    public RoverParts thisPart;

    Color originalColor;
    [SerializeField] Color damagedColor = Color.red;

    void Start()
    {
        pickupManager = GetComponentInParent<PickupManager>();
        parent = transform.parent.gameObject;
        originalColor = GetComponent<Renderer>().material.color;
    }

    void OnTriggerEnter(Collider collision)
    {
        
        
        if (collision.gameObject.CompareTag("Crater"))
        {
            if (!pickupManager.SpeedPickupActive())
            {

                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 16f);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Meteor"))
        {
            if (!pickupManager.SpeedPickupActive())
            {
                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 40f);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!pickupManager.SpeedPickupActive() && collision.gameObject.CompareTag("Meteor") || thisPart == RoverParts.Wheel)
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!pickupManager.SpeedPickupActive() && other.gameObject.CompareTag("Crater") || thisPart == RoverParts.Wheel)
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }
}
