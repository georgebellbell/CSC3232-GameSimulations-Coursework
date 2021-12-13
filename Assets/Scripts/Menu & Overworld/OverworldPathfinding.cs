using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverworldPathfinding : MonoBehaviour
{
    [SerializeField] OverworldNode startPlanet, targetPlanet;
    List<OverworldNode> path = new List<OverworldNode>();

    GameObject[] planets;
    string[] planetNames;
    int[] planetIndexes;

    [SerializeField] LineRenderer lines;
    [SerializeField] float lineWidth;

    [SerializeField] Button playButton;

    LevelManager levelManager;



    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();

        lines.enabled = false;
        lines.startWidth = lineWidth;
        lines.endWidth = lineWidth;
    }

    // called whenever one of the planets in the overworld is selected, calculating a path towards
    public void SetTarget(OverworldNode newTarget)
    {
        // if their is already a planet selected, remove that one as the target
        if (targetPlanet != null)
            targetPlanet.GetComponent<OverworldPlanet>().ToggleSelectPlanet();

        targetPlanet = newTarget;
        DeterminePath();

    }

    // similar to SetTarget, but for when an overworld planet is selected that has already been selected
    public void RemoveTarget()
    {
        targetPlanet = null;
        DeterminePath();
    }

    private void DeterminePath()
    {
        // if a path can be found, play button is enabled and line is enabled to show the path
        if (FindPath())
        {
            lines.enabled = true;
            BuildPath();
            playButton.gameObject.SetActive(true);
        }
        // otherwise play button is disabled
        else
        {
            playButton.gameObject.SetActive(false);
            lines.positionCount = 0;
        }
    }

    // Uses an adapted version of Rich Davison's code for A* algorithm to navigate nodes to find best path
    // No check for impassible nodes, as all nodes are planets that you should be able to play
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

        List<OverworldNode> openList = new List<OverworldNode>();
        List<OverworldNode> closedList = new List<OverworldNode>();

        openList.Add(startPlanet);


        while (openList.Count > 0)
        {
            OverworldNode node = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].GetFScore() < node.GetFScore())
                {
                    node = openList[i];
                }
            }
            if (node == targetPlanet)
            {
                while (node)
                {
                    path.Add(node);
                    node = node.parent;
                }
                return true;
            }
            foreach (OverworldNode n in node.allNeighbours)
            {
                if (closedList.Contains(n))
                {
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

    float CalculateHeuristic(OverworldNode node)
    {
        return (node.transform.position - targetPlanet.transform.position).magnitude;
    }

    
    // After a path has been found, values of the Overworld planets are stored for the line renderer as well as levelManager
    void BuildPath()
    {
        Vector3[] positions = new Vector3[path.Count];
        planets = new GameObject[path.Count];
        planetIndexes = new int[path.Count];
        planetNames = new string[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            positions[i] = path[i].transform.position;
            planets[i] = path[i].gameObject;
            planetIndexes[i] = path[i].gameObject.GetComponent<OverworldPlanet>().GetPlanetIndex();
            planetNames[i] = path[i].gameObject.GetComponent<OverworldPlanet>().planetName;
        }

        System.Array.Reverse(planetIndexes);
        System.Array.Reverse(planetNames);

        lines.positionCount = path.Count;
        lines.SetPositions(positions);
    }

    // If there is a valid path, button with this function attached will be enabled
    // Uploads data to a non destructable object to maintain path of planets
    public void BeginGame()
    {
        levelManager.SetPlanetPath(planetIndexes, planetNames);
 
        SceneManager.LoadScene(planetIndexes[0]);
    }

    
}
