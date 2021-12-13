using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Material readyToAttackMaterial, coolingDownMaterial;
    [SerializeField] float nodeRadius;
    [SerializeField] float attackRange;
    [SerializeField] GameObject target;
    [SerializeField] float speed;

    List<Vector3> path = new List<Vector3>();
    PlanetPathfinding planetPathfinding;

    Rigidbody rigidbody;

    float distanceBetween;

    float damage = 100 / 3;
    int cooldownTime = 3;
    bool coolingDown;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        planetPathfinding = FindObjectOfType<PlanetPathfinding>();
        RecalculatePath();
    }

    // if enemy is close enough to rover, it will attack and deal some damage
    private void Update()
    {
        if (!coolingDown)
        {
            distanceBetween = MainToolbox.CalculateArcLength(transform.position, target.transform.position);
            if (distanceBetween <= attackRange)
            {
                Attack();
            }
        }
       

    }

    // After attacking, enemy enters a cool down state where they must wait a certain amount of time before attacking again
    private void Attack()
    {
        Health playerHealth = target.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            GetComponent<Renderer>().material = coolingDownMaterial;
            coolingDown = true;
            StartCoroutine(RechargeAttack());
        }

    }

    // Coroutine that waits a certain amount of time before allowing another attack
    IEnumerator RechargeAttack()
    {
        int i = 0;

        while (i != cooldownTime)
        {
            yield return new WaitForSeconds(1f);
            i++;
        }

        coolingDown = false;
        GetComponent<Renderer>().material = readyToAttackMaterial;
    }

    // every fixed update, the enemy calculates a new path to the player
    void FixedUpdate()
    {
        RecalculatePath();
    }

    // Calls pathfinding script for planet and sets it as new path for enemy to follow
    void RecalculatePath()
    {
        Vector3[] potentialPath = planetPathfinding.GetNewPath(transform.position, target.transform.position);

        if (potentialPath == null)
        {
            return;
        }

        List<Vector3> newPath = new List<Vector3>();
        newPath = potentialPath.ToList();
        StopCoroutine(FollowPath());
        
        path.Clear();
        path = newPath;
       
        StartCoroutine(FollowPath());
    }

    // Moves enemy across points in path, but if certain distance across, will move towards point after current target for smoother movement
    IEnumerator FollowPath()
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = path[i];

            
            if (MainToolbox.CalculateArcLength(startPos, endPos) < nodeRadius)
            {
                endPos = path[i + 1];
            }

            Vector3 position = Vector3.MoveTowards(rigidbody.position, endPos, speed * Time.fixedDeltaTime);
            rigidbody.MovePosition(position);
            yield return new WaitForFixedUpdate();

        }
       
    }
}
