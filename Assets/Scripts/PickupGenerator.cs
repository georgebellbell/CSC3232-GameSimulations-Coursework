using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] pickups;
    GameObject pickupParent;
    bool pickupsSpawning = false;
    [SerializeField] float distanceFromPlanet = 8f;
    [SerializeField] float spawnRate = 5;
    void Start()
    {
        pickupParent = GameObject.Find("PickupParent");
    }

    void Update()
    {
            if (!pickupsSpawning)
            {
                pickupsSpawning = true;
                StartCoroutine(SpawnPickup());
            }      
    }

    IEnumerator SpawnPickup()
    {
        yield return new WaitForSeconds(spawnRate);

        Vector3 pos = UnityEngine.Random.onUnitSphere * GetComponentInChildren<SphereCollider>().radius * distanceFromPlanet;

        int randomPickupIndex = UnityEngine.Random.Range(0, pickups.Length);

        GameObject newPickup =  Instantiate(pickups[randomPickupIndex]); 

        newPickup.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<GravityAttractor>());
        newPickup.transform.position = pos;
        newPickup.transform.parent = pickupParent.transform;
        ChangeRotation(newPickup.transform);

        StartCoroutine(SpawnPickup());
    }

    private void ChangeRotation(Transform pickup)
    {
        Vector3 upGravity = (pickup.position - transform.position).normalized;
        Vector3 upBody = pickup.up;

        Quaternion targetRotation = Quaternion.FromToRotation(upBody, upGravity) * pickup.rotation;
        pickup.rotation = Quaternion.Slerp(pickup.rotation, targetRotation, 50 + Time.deltaTime);
    }
}
