using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AStar : MonoBehaviour
{
    [SerializeField] LineRenderer lines;

    [SerializeField] PathfindingNode startPlanet, targetPlanet;
    [SerializeField] float lineWidth;
    [SerializeField]
    GameObject[] planets;

    [SerializeField] Button playButton;

    LevelManager levelManager;


    List<PathfindingNode> path = new List<PathfindingNode>();
    int[] planetIndexes;

    public void SetTarget(PathfindingNode newTarget)
    {
        if (targetPlanet != null)
            targetPlanet.GetComponent<OverworldPlanet>().ToggleSelectPlanet();

        targetPlanet = newTarget;
        DeterminePath();

    }

    private void DeterminePath()
    {
        if (FindPath())
        {
            BuildPath();
            playButton.gameObject.SetActive(true);
        }
        else
        {
            playButton.gameObject.SetActive(false);
            lines.positionCount = 0;
            Debug.Log("NO VALID PATH");
        }
    }

    public void RemoveTarget()
    {
        targetPlanet = null;
        DeterminePath();

    }
    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        lines.startWidth = lineWidth;
        lines.endWidth = lineWidth;
    }

    float CalculateHeuristic(PathfindingNode node)
    {
        return (node.transform.position - targetPlanet.transform.position).magnitude;
    }

    bool FindPath()
    {
        path.Clear();

        if (!startPlanet || !targetPlanet)
        {
            return false;
        }

        startPlanet.parent = null;
        startPlanet.g = 0.0f;
        startPlanet.h = CalculateHeuristic(startPlanet);

        List<PathfindingNode> openList = new List<PathfindingNode>();
        List<PathfindingNode> closedList = new List<PathfindingNode>();

        openList.Add(startPlanet);


        while (openList.Count > 0)
        {
            PathfindingNode node = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {       
                if (openList[i].GetFScore() < node.GetFScore())
                {
                    node = openList[i];
                }
            }
            if(node == targetPlanet)
            {
                while (node)
                {
                    path.Add(node);
                    node = node.parent;
                }
                return true;
            }
            foreach (PathfindingNode n in node.allNeighbours)
            {
                if (closedList.Contains(n))
                {
                    continue;
                }
                if (n.IsImpassible() || !n.isWanted)
                {
                    closedList.Add(node);
                    continue;
                }

                float newH = CalculateHeuristic(n);
                float newG = node.GetFScore() + n.traversalCost;
                float newF = newH + newG;
                bool inList = openList.Contains(n);

                if (newF < node.GetFScore() || !inList)
                {
                    if (!inList)
                    {
                        n.h = newH;
                        openList.Add(n);
                    }
                    n.g = newG;
                    n.h = newH;
                    n.parent = node;
                }
            }

            openList.Remove(node);
            closedList.Add(node);
        }
        

        return false;
    }

    void BuildPath()
    {


        Vector3[] positions = new Vector3[path.Count];
        planets = new GameObject[path.Count];
        planetIndexes = new int[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            positions[i] = path[i].transform.position;
            planets[i] = path[i].gameObject;
            planetIndexes[i] = path[i].gameObject.GetComponent<OverworldPlanet>().GetPlanetIndex();

        }

        lines.positionCount = path.Count;
        lines.SetPositions(positions);
    }

    public void BeginGame()
    {
        levelManager.SetPlanetPath(planetIndexes);
        SceneManager.LoadScene(planetIndexes[0]);
    }
}
