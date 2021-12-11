using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlockingEnemyGenerator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyCount;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int numberOfEnemies = 50;

    List<GameObject> boids = new List<GameObject>();
    ManagementSystem managementSystem;

    GameObject enemyParent;
    // Start is called before the first frame update
    void Awake()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        enemyParent = GameObject.Find("EnemyParent");
        GenerateEnemies();
        UpdateEnemyCount(numberOfEnemies);
    }

    private void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemies - 1; i++)
        {
            Vector3 randomPosition = UnityEngine.Random.onUnitSphere * MainToolbox.planetRadius;

            boids.Add(Instantiate(enemyPrefab, randomPosition, Quaternion.identity, enemyParent.transform));
            boids[i].GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
        }
        
    }

    public void RemoveBoid(FlockingEnemy boid)
    {
        boids.Remove(boid.gameObject);
        enemyParent.BroadcastMessage("UpdateBoids");
        UpdateEnemyCount(boids.Count);

        if (boids.Count == 0)
        {
            managementSystem.WinGame();
        }
    }

    public List<GameObject> GetBoids()
    {
        return boids;
    }
    public void UpdateEnemyCount(int currentNumber)
    {
        enemyCount.text = currentNumber + "/" + numberOfEnemies;
    }

    

    
}
