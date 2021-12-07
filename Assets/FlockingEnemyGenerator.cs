using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingEnemyGenerator : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int numberOfEnemies = 50;

    SphereCollider planetCollider;
    float planetRadius;

    GameObject enemyParent;
    // Start is called before the first frame update
    void Awake()
    {
        planetCollider = GetComponentInChildren<SphereCollider>();
        planetRadius = planetCollider.radius * transform.GetChild(0).transform.lossyScale.x;
        enemyParent = GameObject.Find("EnemyParent");
        GenerateEnemies();
    }

    private void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemies - 1; i++)
        {
            Vector3 randomPosition = UnityEngine.Random.onUnitSphere * planetRadius;

            GameObject newObject = Instantiate(enemyPrefab, randomPosition, Quaternion.identity, enemyParent.transform);
            newObject.GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
        }
    }

    public int GetTotalNumberOfEnemies()
    {
        return numberOfEnemies;
    }

    public float GetRadius()
    {
        return planetRadius;
    }
}
