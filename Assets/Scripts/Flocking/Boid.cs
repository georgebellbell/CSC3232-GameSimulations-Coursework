using System.Collections.Generic;
using UnityEngine;

// Implemented some advanced realtime AI in the form of flocking with allignment, seperation and cohesion
public class Boid : MonoBehaviour
{
    [SerializeField] float allignmentRange;
    [SerializeField] float separationRange;
    [SerializeField] float cohesionRange;

    [SerializeField] float movementSpeed = 5;
    Vector3 movementDirection;
    Rigidbody rb;

    BoidGenerator generator;
    List<GameObject>  boids = new List<GameObject>();
    int numberOfBoids;

    void Start()
    {
        generator = FindObjectOfType<BoidGenerator>();
        rb = GetComponent<Rigidbody>();

        UpdateBoids();

        //movementDirection = new Vector3(0, 0, 1).normalized;
    }

    // Called via a broadcast message from BoidGenerator to update when a boid has died
    public void UpdateBoids()
    {
        boids = generator.GetBoids();
        numberOfBoids = boids.Count;

    }

    void Update()
    {
        movementDirection = FlockingBehaviour();
        CheckIfOnPlanet();
    }

    // Should never need to be completely ran, but ensures level can be beaten in case a boid flies off
    private void CheckIfOnPlanet()
    {
        float distance = Vector3.Distance(this.transform.position, MainToolbox.planetTransform.position);
        if (distance >= MainToolbox.planetRadius * 2.5)
        {
            GameObject.FindObjectOfType<BoidGenerator>().RemoveBoid(this);
            GameObject.Destroy(this.gameObject);
        }
    }

    // same movement technique as player but movement direction is determined through flocking
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position +
            transform.TransformDirection(movementDirection) *
            movementSpeed * Time.fixedDeltaTime);
    }


    Vector3 FlockingBehaviour()
    {
        Vector3 cohesionVector = new Vector3();
        Vector3 separateVector = new Vector3();
        Vector3 allignmentVector = new Vector3();

        int numberInRange = 0;
        int numInAllignRange = 0, numInSeparationRange = 0, numInCohesionRange = 0;
        for (int i = 0; i < numberOfBoids - 1; i++)
        {
            Transform currentEnemy = boids[i].transform;

            // skips to next boid if the current boid is itself
            if (transform.position == currentEnemy.position)
            {
                continue;
            }

            float distance = MainToolbox.CalculateArcLength(transform.position, currentEnemy.position);


            if (distance > 0)
            {
                // if within the allignment range, all the boids forward direction are averaged
                if (distance < allignmentRange)
                {
                    allignmentVector += currentEnemy.transform.forward;
                    numInAllignRange++;
                }
                // if within the cohesion range, average position of all boids is calculated
                if (distance < cohesionRange)
                {
                    cohesionVector += currentEnemy.transform.position;
                    numInCohesionRange++;
                }
                // if withing the seperation range, average position away from each of those boids is calculated
                if (distance < separationRange)
                {
                    separateVector += (currentEnemy.transform.position - transform.position);
                    numInSeparationRange++;
                }
                numberInRange++;
            }
        }

        // if their are no boids in range, the boid will nust move forward
        if (numberInRange == 0)
        {
            return Vector3.forward;
        }

        // calculating the averages
        separateVector /= numInSeparationRange;
        separateVector *= -1;

        allignmentVector /= numInAllignRange;

        cohesionVector /= numInCohesionRange;

        cohesionVector = (cohesionVector - transform.position);

        Vector3 flockingVector = separateVector.normalized + cohesionVector.normalized + allignmentVector.normalized;

        flockingVector = new Vector3(flockingVector.x, 0, 1);
        return flockingVector;
    }

    
   
}
