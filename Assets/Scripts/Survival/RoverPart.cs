using UnityEngine;

public class RoverPart : MonoBehaviour
{
    [SerializeField] Color damagedColor = Color.red;
    Color originalColor;

    GameObject parent;
    RoverStateMachine roverStateMachine;
    public RoverParts thisPart;
    
    void Start()
    {
        roverStateMachine = GetComponentInParent<RoverStateMachine>();
        parent = transform.parent.gameObject;
        originalColor = GetComponent<Renderer>().material.color;
    }

    // If player hits a crater, the specific part that did will highlight red and damage done will be passed to Health.CS
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Crater"))
        {
            if (roverStateMachine.currentState == roverStateMachine.noPowerupState)
            {

                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 16f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((roverStateMachine.currentState == roverStateMachine.noPowerupState || thisPart == RoverParts.Wheel) && other.gameObject.CompareTag("Crater")))
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }

    // If meteor hits player, the specific part that did will highlight red and damage done will be passed to Health.CS
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Meteor"))
        {
            if (roverStateMachine.currentState == roverStateMachine.noPowerupState)
            {
                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 40f);
            }
        }
    }

    // when exiting collision if player has no powerup active (or it is a wheel), their appearance will return completely to normal

    private void OnCollisionExit(Collision collision)
    {
        if (!(roverStateMachine.currentState == roverStateMachine.noPowerupState || thisPart == RoverParts.Wheel) && collision.gameObject.CompareTag("Meteor") )
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }

    
}
