using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingEnemy : MonoBehaviour
{
    [SerializeField] Transform planetTransform;
    
    public bool primeAlligner = false;
    Rigidbody rb;
    Vector3 movementDirection;
    [SerializeField] float movementSpeed = 5;

    float flockingValue;

    float alignmentRotation, separationValue, avoidanceValue;

    int numberOfEnemies;

    [SerializeField] float flockingAllignmentRange;
    [SerializeField] float flockingSeparationRange;
    [SerializeField] float flockingAvoidanceRange;



    [SerializeField] float sepForce = 5, avoidanceForce = 10;

    [SerializeField] bool allign, seperate, avoid, cohesion;


    [SerializeField] FlockingEnemy[] flockingEnemies;

    FlockingEnemyGenerator generator;


    [SerializeField] GameObject obstacle;

    // Start is called before the first frame update
    void Start()
    {

        generator = FindObjectOfType<FlockingEnemyGenerator>();
        rb = GetComponent<Rigidbody>();

        numberOfEnemies = generator.GetTotalNumberOfEnemies();
        
        flockingEnemies = FindObjectsOfType<FlockingEnemy>();

        flockingValue = 1;
        movementDirection = new Vector3(0, 0, 1).normalized;



    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(0, 0, 0);
        if (allign) Allignment();
        if (seperate) Separation();
        if (avoid) Avoidance();
        




    }

   

    private void Allignment()
    {
        

        Vector3 forwardVelocity = new Vector3();
       
        int numberInRange = 0;
        for (int i = 0; i < numberOfEnemies - 1; i++)
        {
            Transform currentEnemy = flockingEnemies[i].transform;
             
            float distanceBetweenPoints = Vector3.Distance(this.transform.localPosition, currentEnemy.localPosition);
            float arcLength = CalculateArcLength(distanceBetweenPoints);

            if (arcLength <= flockingAllignmentRange)
            {
                numberInRange++;
                
                forwardVelocity += currentEnemy.transform.position;
                

                
            }
            
        }

        
        forwardVelocity /= numberInRange;

        forwardVelocity.Normalize();

        Vector3 newForward = new Vector3(forwardVelocity.x, 0, 1);
        movementDirection = newForward;
        
       
    }

    private float CalculateArcLength(float distanceBetweenPoints)
    {
        float degree = 2 * Mathf.Asin(distanceBetweenPoints / (2 * MainToolbox.planetRadius));

        return degree * MainToolbox.planetRadius;
    }

    private void Separation()
    {

        for (int i = 0; i < numberOfEnemies - 1; i++)
        {
            // exclude self
            Transform currentEnemy = flockingEnemies[i].transform;

            if (transform.position == currentEnemy.position)
            {
                break;
            }


            float distanceBetweenPoints = Vector3.Distance(this.transform.localPosition, currentEnemy.localPosition);
            float arcLength = CalculateArcLength(distanceBetweenPoints);

            if (primeAlligner)
            {
                Debug.Log(arcLength);

            }
            //Vector3 directionBetweenPoints =  (transform.position - currentEnemy.position);

            if (arcLength < flockingSeparationRange)
            {
                Vector3 directionBetweenPoints = ((transform.position - currentEnemy.position)).normalized;
                
                float weighting = flockingSeparationRange / arcLength;
                
                //seperationForce += directionBetweenPoints * weighting * 5f;
                rb.AddForce(directionBetweenPoints * weighting * sepForce);
            }

            
        }
    }

    private void Avoidance()
    {
        float distanceFromObstacle = Vector3.Distance(this.transform.localPosition, obstacle.transform.position);
        float arcDistance = CalculateArcLength(distanceFromObstacle);

        if (primeAlligner)
        {
            Debug.Log(arcDistance);
            
        }

        if (arcDistance < flockingAvoidanceRange)
        {
            Vector3 directionBetweenPoints = ((transform.position - obstacle.transform.position)).normalized;
            float weighting = flockingAvoidanceRange / arcDistance;
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            Vector3 forceDirection;

            float directionDifference = Vector3.Dot(-directionBetweenPoints, this.transform.forward);

            if (primeAlligner)
            {
                Debug.Log(directionDifference);
            }

            if (directionDifference > 0 && directionDifference < 1)
            {
                Debug.Log(gameObject.name + " is about to hit obstacle head on!");
                forceDirection = this.transform.right;
            }
            else
            {
                forceDirection = directionBetweenPoints;
            }

            rb.AddForce(forceDirection * weighting * avoidanceForce);
        }

    }

    

    

    private void FixedUpdate()
    {
        
        rb.MovePosition(rb.position +
            transform.TransformDirection(movementDirection) *
            movementSpeed * Time.fixedDeltaTime);
    }
}
