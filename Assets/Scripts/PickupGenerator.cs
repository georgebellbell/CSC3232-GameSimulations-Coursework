using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGenerator : MonoBehaviour
{
    private class Entry
    {
        public Pickup.PickupType pickup;
        public double probability;
        
    }

    List<Entry> entries; 
    [SerializeField] double totalAccumulatedWeight;
    System.Random rand = new System.Random();

    [SerializeField] GameObject pickupHealth;
    [SerializeField] GameObject pickupJump;
    [SerializeField] GameObject pickupSpeed;

    [SerializeField] double[] pickupProbs = new double[3];
    GameObject pickupParent;

    [SerializeField] float distanceFromPlanet = 8f;
    [SerializeField] float spawnRate = 5;


    void Start()
    {
        pickupParent = GameObject.Find("PickupParent");
        GenerateProbabilities();

        StartCoroutine(SpawnPickup());

    }

    private void GenerateProbabilities()
    {
        entries = new List<Entry>();
        totalAccumulatedWeight = 0;
        AddEntry(Pickup.PickupType.HealthPickup, pickupProbs[0]);
        AddEntry(Pickup.PickupType.JumpPickup, pickupProbs[1]);
        AddEntry(Pickup.PickupType.SpeedPickup, pickupProbs[2]);
    }

    void AddEntry(Pickup.PickupType newPickup, double pickupChance)
    {
        totalAccumulatedWeight += pickupChance;
        Entry newEntry = new Entry();
        newEntry.pickup = newPickup;
        newEntry.probability = totalAccumulatedWeight;
        entries.Add(newEntry);
    }

    IEnumerator SpawnPickup()
    {
        yield return new WaitForSeconds(spawnRate);

        Vector3 pos = UnityEngine.Random.onUnitSphere * GetComponentInChildren<SphereCollider>().radius * transform.GetChild(0).transform.lossyScale.x * 2;

        GameObject newPickup = null;

        switch (GetRandomPickup())
        {
            case Pickup.PickupType.HealthPickup:
                {
                    newPickup = Instantiate(pickupHealth);
                    ChangeJumpChance(-5);
                }
                break;

            case Pickup.PickupType.JumpPickup:
                {
                    newPickup = Instantiate(pickupJump);
                    ChangeJumpChance(-5);
                }
                break;

            case Pickup.PickupType.SpeedPickup:
                {
                    newPickup = Instantiate(pickupSpeed);
                    ChangeSpeedChance(-1);
                }
                break;

        }

        newPickup.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
        newPickup.transform.position = pos;
        newPickup.transform.parent = pickupParent.transform;
        ChangeRotation(newPickup.transform);

        StartCoroutine(SpawnPickup());
    }


    Pickup.PickupType GetRandomPickup()
    {
        double randomProb = rand.NextDouble() * totalAccumulatedWeight;

        Debug.Log("randomProb!: " + randomProb);

        foreach (Entry entry in entries)
        {
            if (entry.probability >= randomProb)
            {
                Debug.Log(entry.pickup);
                return entry.pickup;
            }
        }
        return 0; // only if no values
    }

    private void ChangeRotation(Transform pickup)
    {
        Vector3 upGravity = (pickup.position - transform.position).normalized;
        Vector3 upBody = pickup.up;

        Quaternion targetRotation = Quaternion.FromToRotation(upBody, upGravity) * pickup.rotation;
        pickup.rotation = Quaternion.Slerp(pickup.rotation, targetRotation, 50 + Time.deltaTime);
    }

    void ChangePickupChance(int pickupToChange, double probChange)
    {

        pickupProbs[pickupToChange] = Mathf.Max((float)pickupProbs[pickupToChange] + (float)probChange, 0);
        
        GenerateProbabilities();
    }

    public void ChangeHealthChance(double probChange)
    {
        ChangePickupChance(0, probChange);
    }

    public void ChangeJumpChance(double probChange)
    {
        ChangePickupChance(1, probChange);
    }

    public void ChangeSpeedChance(double probChange)
    {
        ChangePickupChance(2, probChange);
    }

    
    

   

    

    
}
