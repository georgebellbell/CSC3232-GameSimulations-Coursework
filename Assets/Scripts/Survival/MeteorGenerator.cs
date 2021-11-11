using System.Collections;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{
    [SerializeField] float spawnRate = 0.5f;
    [SerializeField] GameObject meteorPrefab;

    GameObject meteorParent;
    
    // When game starts, SpawnMeteor Coroutine will start
    public void StartMeteors()
    {
        meteorParent = GameObject.Find("MeteorParent");
        StartCoroutine(SpawnMeteor());
    }

    //  Instantiates meteor object in orbit around planet and has an attractor set to it so that it falls to the planet
    IEnumerator SpawnMeteor()
    {
        float radius = GetComponentInChildren<SphereCollider>().radius * transform.GetChild(0).transform.lossyScale.x;
        Vector3 pos = UnityEngine.Random.onUnitSphere * radius * 3;

        GameObject newMeteor = Instantiate(meteorPrefab, pos, Quaternion.identity, meteorParent.transform);
        newMeteor.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());

        yield return new WaitForSeconds(spawnRate);

        StartCoroutine(SpawnMeteor());
    }

   
}
