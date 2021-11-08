using UnityEngine;

public class RoverPart : MonoBehaviour
{
    [SerializeField] Color damagedColor = Color.red;
    Color originalColor;

    GameObject parent;
    PickupManager pickupManager;
    public RoverParts thisPart;
    
    void Start()
    {
        pickupManager = GetComponentInParent<PickupManager>();
        parent = transform.parent.gameObject;
        originalColor = GetComponent<Renderer>().material.color;
    }

    // If player hits a crater, the specific part that did will highlight red and damage done will be passed to Health.CS
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Crater"))
        {
            if (!pickupManager.PickupActive(pickupManager.speedTimeLeft))
            {

                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 16f);
            }
        }
    }

    // If meteor hits player, the specific part that did will highlight red and damage done will be passed to Health.CS
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Meteor"))
        {
            if (!pickupManager.PickupActive(pickupManager.speedTimeLeft))
            {
                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 40f);
            }
        }
    }

    // when exiting collision with either of these, if player is not in a speed pickup state, they item will return to normal

    private void OnCollisionExit(Collision collision)
    {
        if (!(pickupManager.PickupActive(pickupManager.speedTimeLeft) || thisPart == RoverParts.Wheel) && collision.gameObject.CompareTag("Meteor") )
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((!pickupManager.PickupActive(pickupManager.speedTimeLeft) || thisPart == RoverParts.Wheel) && other.gameObject.CompareTag("Crater"))
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }
}
