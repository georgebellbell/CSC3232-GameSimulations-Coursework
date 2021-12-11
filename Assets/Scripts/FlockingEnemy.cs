using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlockingEnemy : MonoBehaviour
{
    
    
    Rigidbody rb;
    Vector3 movementDirection;
    [SerializeField] float movementSpeed = 5;


    int numberOfEnemies;

    [SerializeField] float allignmentRange;
    [SerializeField] float separationRange;

    [SerializeField] float cohesionRange;
    [SerializeField] List<GameObject>  flockingEnemies = new List<GameObject>();

    FlockingEnemyGenerator generator;


    // Start is called before the first frame update
    void Start()
    {

        generator = FindObjectOfType<FlockingEnemyGenerator>();
        rb = GetComponent<Rigidbody>();

        UpdateBoids();

        movementDirection = new Vector3(0, 0, 1).normalized;



    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = FlockingBehaviour();
    }

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
        for (int i = 0; i < numberOfEnemies - 1; i++)
        {
            Transform currentEnemy = flockingEnemies[i].transform;

            if (transform.position == currentEnemy.position)
            {
                break;
            }

            float distance = MainToolbox.CalculateArcLength(transform.position, currentEnemy.position);

            if (distance > 0)
            {
                if (distance < allignmentRange)
                {
                    
                    allignmentVector += currentEnemy.transform.forward;
                    numInAllignRange++;
                }
                if (distance < cohesionRange)
                {
                    cohesionVector += currentEnemy.transform.position;
                    numInCohesionRange++;
                }
                if (distance < separationRange)
                {
          
                    separateVector += (currentEnemy.transform.position - transform.position);
                    numInSeparationRange++;
                }
                
                
                

                numberInRange++;
            }

        }

        if (numberInRange == 0)
        {
            return Vector3.forward;
        }

        separateVector /= numInSeparationRange;
        separateVector *= -1;

        allignmentVector /= numInAllignRange;

        cohesionVector /= numInCohesionRange;

        cohesionVector = (cohesionVector - transform.position);

        Vector3 flockingVector = separateVector.normalized + cohesionVector.normalized + allignmentVector.normalized;

        flockingVector = new Vector3(flockingVector.x, 0, 1);
        return flockingVector;
    }


    public void UpdateBoids()
    {
        flockingEnemies = generator.GetBoids();
        numberOfEnemies = flockingEnemies.Count;

    }
   
}
