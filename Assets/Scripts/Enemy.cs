using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    PlanetPathfinding planetPathfinding;

    [SerializeField] float nodeRadius;
    [SerializeField] float attackRange;
    [SerializeField] GameObject target;
    [SerializeField] float speed;

    [SerializeField] Material readyToAttackMaterial, coolingDownMaterial;

    Rigidbody rigidbody;

    float distanceBetween;

    float damage = 100 / 3;

    int cooldownTime = 3;

    bool coolingDown;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        planetPathfinding = FindObjectOfType<PlanetPathfinding>();
        RecalculatePath();
    }

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

    IEnumerator RechargeAttack()
    {
        int i = 0;

        while (i != cooldownTime)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1f);
            Debug.Log("I WAITED");
            i++;
        }

        coolingDown = false;
        GetComponent<Renderer>().material = readyToAttackMaterial;
    }

    void FixedUpdate()
    {
        
        RecalculatePath();
    }

    
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
