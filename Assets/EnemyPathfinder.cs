using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPathfinder : MonoBehaviour
{
    List<Vector3> path = new List<Vector3>();
    PlanetPathfinding planetPathfinding;

    [SerializeField] float nodeRadius;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        planetPathfinding = FindObjectOfType<PlanetPathfinding>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (target.transform.position - transform.position).magnitude;

        float arcDistance = MainToolbox.CalculateArcLength(distance);

        RecalculatePath();
    }

    
    void RecalculatePath()
    {
        List<Vector3> newPath = new List<Vector3>();
        newPath = planetPathfinding.GetNewPath(transform.position, target.position).ToList();

        if (newPath.Count == 0)
        {
            return;
        }
        StopAllCoroutines();
        path.Clear();
        path = newPath;
        Debug.Log(path.Count);
       
            StartCoroutine(FollowPath());
        
        Debug.Log(path.Count);
        

        

    }

    IEnumerator FollowPath()
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = path[i];

            if (MainToolbox.CalculateArcLength((endPos - startPos).magnitude) < nodeRadius)
            {
                endPos = path[i + 1];
            }

            float travelPercent = Time.deltaTime * speed;

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }

        }
    }
}
