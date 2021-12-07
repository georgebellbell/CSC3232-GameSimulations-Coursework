using UnityEngine;


public class Planet : MonoBehaviour
{
    public enum PlanetType
    {
        survival,
        puzzle,
        menu,
        smart
    };

    [SerializeField] float gravity = -10f;
    [SerializeField] float distance;
    [SerializeField] float maximumGravity = -10;
    [SerializeField] float minimumGravity = -1f;
    [SerializeField] float thisPlanetMass = 160;

    public PlanetType thisPlanetType = PlanetType.survival;

    public float planetRadius;

    
    private void Start()
    {
        planetRadius = GetComponentInChildren<SphereCollider>().radius * transform.GetChild(0).transform.lossyScale.x;

        if (thisPlanetType == PlanetType.survival)
        {
            GetComponent<MeteorGenerator>().StartMeteors();
            GetComponent<PickupGenerator>().StartPickups();
        }
        else if (thisPlanetType == PlanetType.puzzle)
        {
            GetComponent<PuzzleMode>().StartPuzzle();
        }
    }


    private void Update()
    {
        if (thisPlanetType == PlanetType.puzzle)
        {
            GetComponent<PuzzleMode>().UpdatePuzzlePlanet();
        }
    }
    

    // Referenced by GravityBody.cs and pulls the object towards the transform of
    // the object this is attached to, i.e the center of the planet
    public void Attract(Transform player, float playerMass)
    {
        // direction of upward gravity on player calculated
        Vector3 upGravity = (player.position - transform.position).normalized;
        Vector3 upBody = player.up;

        // gravity calculated using function below, negative so to pull them inwards
        gravity = Mathf.Clamp(-GravitationalCalculation(player, playerMass), maximumGravity, minimumGravity);

        // force is applied in direction of gravity
        player.GetComponent<Rigidbody>().AddForce(upGravity * gravity);

        // player rotation is changed to mirror this change in gravity, or when the player moves
        Quaternion targetRotation = Quaternion.FromToRotation(upBody, upGravity) * player.rotation;
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, 50 + Time.deltaTime);

       
    }

    // Used Newton's universal law of gravitation to calculate the force of gravity on the player that can be changed easily from planet to planet
    // uses the distance between player and planet center and both their masses
    float GravitationalCalculation(Transform player, float playerMass)
    {
        float massProduct = thisPlanetMass * playerMass;

        distance = Vector3.Distance(this.transform.position, player.transform.position);

        float gravitationalForce = massProduct / (distance * distance);

        return gravitationalForce;
    }

}
