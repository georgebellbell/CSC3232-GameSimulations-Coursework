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

    [SerializeField] GameObject pickupHealth;
    [SerializeField] GameObject pickupJump;
    [SerializeField] GameObject pickupSpeed;
    [SerializeField] float spawnRate = 5;

    [SerializeField] double[] pickupProbs = new double[3];
    double totalAccumulatedProb;

    System.Random rand = new System.Random();
    List<Entry> entries;

    GameObject pickupParent;
    
    public void Start()
    {
        pickupParent = GameObject.Find("PickupParent");
        GenerateProbabilities();

        StartCoroutine(SpawnPickup());

    }

    // When a probability value for one of the pickups is changed, or the game starts,
    // a probability list is created with an accumulating value
    private void GenerateProbabilities()
    {
        entries = new List<Entry>();
        totalAccumulatedProb = 0;
        AddEntry(Pickup.PickupType.HealthPickup, pickupProbs[0]);
        AddEntry(Pickup.PickupType.JumpPickup, pickupProbs[1]);
        AddEntry(Pickup.PickupType.SpeedPickup, pickupProbs[2]);
    }

    // Creates a new "entry" of a entry and its probability and adds that to the total probability
    void AddEntry(Pickup.PickupType newPickup, double pickupChance)
    {
        totalAccumulatedProb += pickupChance;
        Entry newEntry = new Entry();
        newEntry.pickup = newPickup;
        newEntry.probability = totalAccumulatedProb;
        entries.Add(newEntry);
    }

    // A looping coroutine that will spawn a random pickup, taking into account the probabilities via the list, adding a stochastic element
    IEnumerator SpawnPickup()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            float radius = GetComponentInChildren<SphereCollider>().radius * transform.GetChild(0).transform.lossyScale.x;
            Vector3 pos = UnityEngine.Random.onUnitSphere * radius * 2;

            GameObject newPickup = null;

            // Random pickup is chosen and created and the probability of the
            // two non health pickups spawining is significantly lowered
            switch (GetRandomPickup())
            {
                case Pickup.PickupType.HealthPickup:
                    {
                        newPickup = Instantiate(pickupHealth);
                    }
                    break;

                case Pickup.PickupType.JumpPickup:
                    {
                        newPickup = Instantiate(pickupJump);
                        ChangeJumpChance(-1);
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
        }
        

        
    }


    // Randomly creates a double and then multiplies by total prob to find value within range
    // Due to each pickup having a different weighting, the value is more likely to coinside
    // with the pickup with highest probability
    Pickup.PickupType GetRandomPickup()
    {
        double randomProb = rand.NextDouble() * totalAccumulatedProb;

        foreach (Entry entry in entries)
        {
            if (entry.probability >= randomProb)
            {
                return entry.pickup;
            }
        }
        return 0; // only if no values
    }

    // Player actions and pickup generation will change the chance of a pickup being spawned
    // public functions below call this for each pickup type, and they are called in several places
    void ChangePickupChance(int pickupToChange, double probChange)
    {

        pickupProbs[pickupToChange] = Mathf.Max((float)pickupProbs[pickupToChange] + (float)probChange, 0);

        // Every time there is a probability change, the overall probabilities need to be recalculated
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
