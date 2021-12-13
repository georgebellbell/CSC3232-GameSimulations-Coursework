using System.Collections;
using UnityEngine;

// For Meteor Generation I implemented the design pattern of an open list to generate a set amount of meteors and then reuse them
public class MeteorGenerator : MonoBehaviour
{
    [SerializeField] float spawnRate = 0.5f;
    [SerializeField] GameObject meteorPrefab;
    [SerializeField] GameObject meteorParent;
    [SerializeField, Range(0, 25)] int poolSize = 5; 

    GameObject[] meteorPool;


    private void Awake()
    {
        
        meteorParent = GameObject.Find("MeteorParent");
        PopulatePool();
    }

    // When game starts, SpawnMeteor Coroutine will start
    public void Start()
    {

        StartCoroutine(SpawnMeteor());
    }
    
    // creates the initial pool of meteors
    void PopulatePool()
    {
        meteorPool = new GameObject[poolSize];

        for (int i = 0; i < meteorPool.Length; i++)
        {
            meteorPool[i] = Instantiate(meteorPrefab, meteorParent.transform);
            meteorPool[i].GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
            meteorPool[i].SetActive(false);

        }
    }

    // loops through list of meteors and enables the ones that are not already active, depending on spawn rate

    IEnumerator SpawnMeteor()
    {
        while (true)
        {

            EnableMeteorInPool();
            yield return new WaitForSeconds(spawnRate);
        }

    }

    void EnableMeteorInPool()
    {
        for (int i = 0; i < meteorPool.Length; i++)
        {
            if (meteorPool[i].activeInHierarchy == false)
            {
                Vector3 pos = UnityEngine.Random.onUnitSphere * MainToolbox.planetRadius * 3;
                meteorPool[i].transform.position = pos;
                meteorPool[i].SetActive(true);

                return;
            }
        }
    }

    

   
}
