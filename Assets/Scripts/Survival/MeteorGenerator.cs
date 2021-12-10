using System.Collections;
using UnityEngine;

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
    public void StartMeteors()
    {
        
        
        StartCoroutine(SpawnMeteor());
    }

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

    IEnumerator SpawnMeteor()
    {
        while (true)
        {

            EnableMeteorInPool();
            yield return new WaitForSeconds(spawnRate);
        }
        
    }

   
}
