using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

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

    public PostProcessVolume ppVolume;
    public ColorGrading temperatureGradient;

    float excludeRange;

    public void Start()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        planetCollider = GetComponentInChildren<SphereCollider>();
        
        excludeRange = deliveryPoint.GetComponent<SphereCollider>().radius * deliveryPoint.transform.lossyScale.x * 5f;

        AdjustNumberOfPoints();

        GenerateItems();

        CheckAllPointsCovered();

        InitialiseTemperatureChangeValues();
    }

    public void Update()
    {
        ReducePlanetTemperature();
    }

    // Number of points set in inspector but if number of points is too great for size of planet, the number is reduced to max planet can hold
    private void AdjustNumberOfPoints()
    {
        int maxNumberOfPoints = Mathf.RoundToInt((MainToolbox.planetRadius) / excludeRange);
        if (totalNumberOfPoints > maxNumberOfPoints)
        {
            totalNumberOfPoints = maxNumberOfPoints;
        }
    }

    // Loops through number of points and generates a pair of delivery points and objects
    void GenerateItems()
    {
        for (int i = 0; i < totalNumberOfPoints; i++)
        {
            GeneratePoint();
            GenerateObject();
        }
    }

    void GenerateObject()
    {
        Vector3 randomPosition = CreateUniquePosition();
        // every time a random position is aquired, that transform is added to an array for checking later and avoiding
        objectPositions.Add(randomPosition);

        GameObject newObject = Instantiate(deliveryObject, randomPosition, Quaternion.identity);
        newObject.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
    }

    void GeneratePoint()
    {
        Vector3 randomPosition = CreateUniquePosition();
        objectPositions.Add(randomPosition);

        deliveryPoints.Add(Instantiate(deliveryPoint, randomPosition, Quaternion.identity));
    }

    // Called in start and when any delivery point is activated
    // Loops through all delivery points and checks if they are active, if so, they win
    public void CheckAllPointsCovered()
    {
        numberOfActivePoints = 0;
        foreach (GameObject currentDeliveryPoint in deliveryPoints)
        {
            if (currentDeliveryPoint.GetComponent<DeliveryPoint>().HasObject())
                numberOfActivePoints++;
        }

        numberOfActivePointsText.text = numberOfActivePoints + "/" + totalNumberOfPoints;

        if (numberOfActivePoints == totalNumberOfPoints)
        {
            StartCoroutine(Win());
            
        }
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(0.5f);
        managementSystem.WinGame();
    }

    // Position of points and objects created with an element of randomness
    // But checks are done to ensure they spawn a certain distance from eachother
    private Vector3 CreateUniquePosition()
    {

        Vector3 potentialPosition = UnityEngine.Random.onUnitSphere * MainToolbox.planetRadius;

        if (excludeRange > MainToolbox.planetRadius * 2)
        {
            excludeRange = MainToolbox.planetRadius;
        }

        // if it's the first item to be spawned no checks are needed
        if (objectPositions.Capacity == 0)
        {
            return potentialPosition;
        }

        // checks through each of the existing item points
        foreach (Vector3 planetObject in objectPositions)
        {
            // if random point is too close to another point, it will try again
            float distbetween = Vector3.Distance(potentialPosition, planetObject);
            if (distbetween < excludeRange * 2f)
            {
                return CreateUniquePosition();
            }

        }

        return potentialPosition;
    }

    // Assigning and creation of variables and components
    private void InitialiseTemperatureChangeValues()
    {
        planetPhysicsMaterial = new PhysicMaterial();
        planetPhysicsMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        planetCollider.material = planetPhysicsMaterial;

        currentPlanetTemperature = DefaultPlanetTemperature;
        currentPlanetFriction = planetPhysicsMaterial.dynamicFriction;

        timeToChange = GetComponent<Timer>().GetGameTime();

        ppVolume.profile.TryGetSettings(out temperatureGradient);

        // using time the level will be played, the rate at which friction and temp should be changed are calculated.
        changePerSecondTemp = (0 - currentPlanetTemperature) / timeToChange;
        changePerSecondFriction = (0 - currentPlanetFriction) / timeToChange;
    }

    // Dynamically changes the dynamicFriction of planet physics material as well as planet temperature and colour
    private void ReducePlanetTemperature()
    {
        // Called every frame and tends towards 0
        currentPlanetFriction = Mathf.Clamp(currentPlanetFriction + changePerSecondFriction * Time.deltaTime, 0, 1);
        planetCollider.sharedMaterial.dynamicFriction = currentPlanetFriction;
        currentPlanetTemperature = Mathf.Clamp(currentPlanetTemperature + changePerSecondTemp * Time.deltaTime, 0, DefaultPlanetTemperature);
        SetTemperatureEffect(currentPlanetTemperature - 40);

        float roundedTemp = Mathf.Round(currentPlanetTemperature * 10f) * 0.1f;
        
        planetTemperatureText.text = roundedTemp.ToString();

        Color tempColour = Color.Lerp(coldColor, hotColor, (currentPlanetTemperature / DefaultPlanetTemperature));
        planetTemperatureText.color = tempColour;
        planetTemperatureSymbol.color = tempColour;
        GetComponentInChildren<Renderer>().material.color = tempColour;
    }

    // Creates a special effect by changing the overall colour of the scene, makes it look colder
    void SetTemperatureEffect(float value)
    {
        temperatureGradient.temperature.value = value;
    }

}
