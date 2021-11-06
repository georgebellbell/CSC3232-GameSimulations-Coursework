using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PuzzleMode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numberOfActivePointsText;
    [SerializeField] GameObject deliveryPoint;
    [SerializeField] GameObject deliveryObject;
    [SerializeField, Range(1,5)] int totalNumberOfPoints;
    List <GameObject> deliveryPoints = new List<GameObject>();
    List<Vector3> objectPositions = new List<Vector3>();
    int numberOfActivePoints;

    [SerializeField] Color hotColor = Color.red, coldColor = Color.blue;
    [SerializeField] TextMeshProUGUI planetTemperatureText;
    [SerializeField] TextMeshProUGUI planetTemperatureSymbol;
    [SerializeField] float DefaultPlanetTemperature;
    float currentPlanetTemperature;
    float currentPlanetFriction;
    float timeToChange;
    float changePerSecondTemp, changePerSecondFriction;

    ManagementSystem managementSystem;

    SphereCollider planetCollider;
    PhysicMaterial planetPhysicsMaterial;
    float planetRadius;
    float excludeRange;



    private void Start()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        planetCollider = GetComponentInChildren<SphereCollider>();

        planetPhysicsMaterial = new PhysicMaterial();
        planetPhysicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        planetCollider.material = planetPhysicsMaterial;

        planetRadius = planetCollider.radius * transform.GetChild(0).transform.lossyScale.x ;
        excludeRange = deliveryPoint.GetComponent<SphereCollider>().radius * deliveryPoint.transform.lossyScale.x * 4f;


        int maxNumberOfPoints = Mathf.RoundToInt((planetRadius ) / excludeRange);
        if (totalNumberOfPoints > maxNumberOfPoints)
        {
            totalNumberOfPoints = maxNumberOfPoints;
        }

        GenerateObjects();
        GeneratePoints();
        CheckAllPointsCovered();
        CalculateRateOfChange();
    }

    private void CalculateRateOfChange()
    {
        currentPlanetTemperature = DefaultPlanetTemperature;
        currentPlanetFriction = planetPhysicsMaterial.dynamicFriction;

        timeToChange = GetComponent<Timer>().GetGameTime();

        changePerSecondTemp = (0 - currentPlanetTemperature) / timeToChange;
        changePerSecondFriction = (0 - currentPlanetFriction) / timeToChange;
    }

    private void Update()
    {
        ReducePlanetTemperature();
    }

    private void ReducePlanetTemperature()
    {
        currentPlanetFriction = Mathf.Clamp(currentPlanetFriction + changePerSecondFriction * Time.deltaTime, 0, 1);
        planetCollider.sharedMaterial.dynamicFriction = currentPlanetFriction;
        currentPlanetTemperature = Mathf.Clamp(currentPlanetTemperature + changePerSecondTemp * Time.deltaTime, 0, DefaultPlanetTemperature);

        Color tempColour = Color.Lerp(coldColor, hotColor, (currentPlanetTemperature / DefaultPlanetTemperature));


        float roundedTemp = Mathf.Round(currentPlanetTemperature * 10f) * 0.1f;
        planetTemperatureText.text = roundedTemp.ToString();
        planetTemperatureText.color = tempColour;
        planetTemperatureSymbol.color = tempColour;

        GetComponentInChildren<Renderer>().material.color = tempColour;
    }

    private void GenerateObjects()
    {
        for (int i = 0; i < totalNumberOfPoints; i++)
        {
            Vector3 randomPosition = CreateUniquePosition(10);
            objectPositions.Add(randomPosition);
            GameObject newObject = Instantiate(deliveryObject);
            newObject.transform.position = randomPosition;
            newObject.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
        }
    }

    void GeneratePoints()
    {
        for (int i = 0; i < totalNumberOfPoints; i++)
        {
            Vector3 randomPosition = CreateUniquePosition(10);
            objectPositions.Add(randomPosition);
            deliveryPoints.Add(Instantiate(deliveryPoint, randomPosition, Quaternion.identity));
        }
    }

    private Vector3 CreateUniquePosition(int attemptsLeft)
    {
        int attempts = attemptsLeft;
        Vector3 potentialPosition = new Vector3();

        if (excludeRange > planetRadius * 2)
        {
            excludeRange = planetRadius;
        }
       
        potentialPosition = UnityEngine.Random.onUnitSphere * planetRadius;

        if (objectPositions.Capacity == 0)
        {
            return potentialPosition;
        }

        foreach (Vector3 planetObject in objectPositions)
        {
            if (!(Vector3.Distance(potentialPosition, planetObject) >= excludeRange) && attempts != 0)
            {
                CreateUniquePosition(attempts--);
            }
               
        }

        return potentialPosition;   
    }

    public void CheckAllPointsCovered()
    {
        numberOfActivePoints = 0;
        foreach(GameObject currentDeliveryPoint in deliveryPoints)
        {
            if (currentDeliveryPoint.GetComponent<DeliveryPoint>().HasObject())
                numberOfActivePoints++;
        }

        numberOfActivePointsText.text = numberOfActivePoints + "/" + totalNumberOfPoints;

        if (numberOfActivePoints == totalNumberOfPoints)
        {
            managementSystem.WinGame();
        }

        
    }
}
