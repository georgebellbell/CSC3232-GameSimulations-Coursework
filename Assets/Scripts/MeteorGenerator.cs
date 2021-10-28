using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{
    public GameObject meteorPrefab;
    GameObject meteorParent;
    [SerializeField] float distanceFromPlanet = 5;
    [SerializeField] float spawnRate = 0.5f;
    void Start()
    {
        meteorParent = GameObject.Find("MeteorParent");
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 pos = Random.onUnitSphere * GetComponentInChildren<SphereCollider>().radius * distanceFromPlanet;
        
        GameObject newMeteor = Instantiate(meteorPrefab);

        newMeteor.transform.position = pos;
        newMeteor.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<GravityAttractor>());

        newMeteor.transform.parent = meteorParent.transform;

        yield return new WaitForSeconds(spawnRate);

        StartCoroutine(SpawnMeteor());
    }
}
