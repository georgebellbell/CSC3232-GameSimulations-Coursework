using UnityEngine;


// Allows for specific collision between player and hazards on world
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

        else if (collision.gameObject.CompareTag("Meteor"))
        {
            if (roverStateMachine.currentState == roverStateMachine.noPowerupState)
            {
                GetComponent<Renderer>().material.color = damagedColor;
                parent.GetComponent<Health>().OnChildCollisionEnter(thisPart, 40f);
            }
        }
    }

    // when exiting collision if player has no powerup active(or it is a wheel), their appearance will return completely to normal
    private void OnTriggerExit(Collider other)
    {
        if (((roverStateMachine.currentState == roverStateMachine.noPowerupState || thisPart == RoverParts.Wheel) && (other.gameObject.CompareTag("Crater") || other.gameObject.CompareTag("Meteor"))))
        {
            GetComponent<Renderer>().material.color = originalColor;
        }
    }
   
}
