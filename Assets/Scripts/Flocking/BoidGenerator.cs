using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoidGenerator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI enemyCount;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int numberOfEnemies = 50;

    List<GameObject> boids = new List<GameObject>();
    ManagementSystem managementSystem;

    GameObject boidParent;

    void Start()
    {
        managementSystem = FindObjectOfType<ManagementSystem>();
        boidParent = GameObject.Find("EnemyParent");
        GenerateBoids();
        UpdateBoidCount(numberOfEnemies);
    }

    private void GenerateBoids()
    {
        for (int i = 0; i < numberOfEnemies - 1; i++)
        {
            Vector3 randomPosition = UnityEngine.Random.onUnitSphere * MainToolbox.planetRadius ;

            boids.Add(Instantiate(enemyPrefab, randomPosition, Quaternion.identity, boidParent.transform));
            boids[i].GetComponent<GravityBody>().SetCurrentAttractor(gameObject.GetComponent<Planet>());
        }
        
    }

    // Updates the UI with number of boids remaining
    public void UpdateBoidCount(int currentNumber)
    {
        enemyCount.text = currentNumber + "/" + numberOfEnemies;
    }

    // if a boid is hit by the player, or leaves region of planet, they will be destroyed and removed from list
    public void RemoveBoid(Boid boid)
    {
        boids.Remove(boid.gameObject);

        // This makes sure the update is carried to all other boids
        boidParent.BroadcastMessage("UpdateBoids");
        UpdateBoidCount(boids.Count);

        //if there are no boids left, player win
        if (boids.Count == 0)
        {
            managementSystem.WinGame();
        }
    }

    public List<GameObject> GetBoids()
    {
        return boids;
    }

    

    

    
}
