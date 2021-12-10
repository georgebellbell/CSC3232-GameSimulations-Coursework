using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [SerializeField] Planet currentPlanet;
    private Transform myTransform;

    public float objectMass = 10;

    

    void Start()
    {
        currentPlanet = MainToolbox.planet;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.useGravity = false;
        myTransform = transform;
    }

    // Every FixedUpdate the Attract function on the planet that the object this script is attached to will be called
    // Creates the effect of gravity on a sphere
    void FixedUpdate()
    {
        currentPlanet.Attract(myTransform, objectMass);
    }

    // When instantiating objects
    public void SetCurrentAttractor(Planet newGravityAttractor)
    {
        currentPlanet = newGravityAttractor;
    }
}
