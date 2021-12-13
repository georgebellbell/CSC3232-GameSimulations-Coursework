using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Provides some support for the player and positive feedback for moving the cubes in the right direction of the delivery points
public class CubePointer : MonoBehaviour
{
    [SerializeField] DeliveryPoint[] deliveryPoints;
    public List<DeliveryPoint> InactivePoints;
    float currentClosestDistance;
    Transform currentClosestPoint;
    Planet planet;
    Renderer boxRenderer;

    void Start()
    {
        planet = FindObjectOfType<Planet>();
        deliveryPoints = FindObjectsOfType<DeliveryPoint>();
        boxRenderer = GetComponentInParent<Renderer>();

        currentClosestDistance = Vector3.Distance(deliveryPoints[0].transform.position, transform.position); ;
        currentClosestPoint = deliveryPoints[0].transform;

    }

    void Update()
    {
        CalculateNearestPoint();

        LookAtNearestPoint();

        ChangeObjectColour();

    }

    // searches through all of the delivery points on planet and determines finds the closest one to the pointer
    private void CalculateNearestPoint()
    {
        // if the current closet point has just been activated by another block, the distance will be made unreachable so next point will be looked at
        currentClosestDistance = Vector3.Distance(currentClosestPoint.transform.position, transform.position);
        if (currentClosestPoint.GetComponent<DeliveryPoint>().HasObject())
        {
            currentClosestDistance = Mathf.Infinity;
        }

        foreach (DeliveryPoint point in deliveryPoints)
        {
            if (!point.HasObject())
            {
                float potentialDistance = Vector3.Distance(point.transform.position, transform.position);
                if (potentialDistance < currentClosestDistance)
                {
                    currentClosestDistance = potentialDistance;
                    currentClosestPoint = point.transform;

                }
            }

        }
    }

    // Uses LookRotation only on the Y Axis to rotate the arrow towards the nearest point
    // gives some feedback to player and what direction they should push the object in
    private void LookAtNearestPoint()
    {
        Vector3 upGravity = (currentClosestPoint.position - planet.transform.position).normalized;
        Vector3 relativePos = (currentClosestPoint.position + upGravity * 5) - transform.position;

        Quaternion LookAtRotation = Quaternion.LookRotation(relativePos, GetComponentInParent<Transform>().up);

        Debug.DrawLine(transform.position, currentClosestPoint.position + upGravity * 5, Color.green);

        Quaternion LookAtRotationOnlyY = Quaternion.Euler(transform.localRotation.eulerAngles.x, LookAtRotation.eulerAngles.y, transform.localRotation.eulerAngles.x);

        transform.localRotation = LookAtRotationOnlyY;
    }

    // Positive feedback loop given to the player as the closer they get to the closest
    // available point, the greener they get, indicating they are pushing it in the right direction 
    private void ChangeObjectColour()
    {
        float planetDiameter = planet.planetRadius * 2;

        Color color = Color.Lerp(Color.green, Color.red, currentClosestDistance / planetDiameter);

        boxRenderer.material.color = color;
    }
}
