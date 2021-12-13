using System.Collections.Generic;
using UnityEngine;

// For the advanced pathfinding, I created my own NavMesh like system across a sphere that uses the A* Algorithm
public class PlanetPathfinding : MonoBehaviour
{
    [SerializeField] float scaling = 32;
    [SerializeField] int numberOfPoints = 128;
    [SerializeField] GameObject node;

    Vector3[] positions;

    List<GameObject> sphereNodes;
    List<Node> path = new List<Node>();

    private void Start()
    {
        Vector3[] points = PointsOnSphere(numberOfPoints);
        sphereNodes = new List<GameObject>();
        int i = 0;

        foreach (Vector3 value in points)
        {
            sphereNodes.Add(Instantiate(node));
            sphereNodes[i].transform.parent = transform;
            sphereNodes[i].transform.position = value * scaling;
            i++;
        }
    }

    // This script instatiates nodes at runtime and equally distributes them across the sphere
    Vector3[] PointsOnSphere(int n)
    {
        List<Vector3> spherePoints = new List<Vector3>();
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2.0f / n;
        float x = 0;
        float y = 0;
        float z = 0;
        float r = 0;
        float phi = 0;

        for (int i = 0; i < n; i++)
        {
            y = i * off - 1 + (off / 2);
            r = Mathf.Sqrt(1 - y * y);
            phi = i * inc;
            x = Mathf.Cos(phi) * r;
            z = Mathf.Sin(phi) * r;

            spherePoints.Add(new Vector3(x, y, z));
        }

        Vector3[] points = spherePoints.ToArray();
        return points;
    }

    // Called by the Enemy to calculate path towards the player
    public Vector3[] GetNewPath(Vector3 startingPos, Vector3 destinationPos)
    {
        if (FindPath(startingPos, destinationPos))
        {
            BuildPath();

        }
        
        return positions;
    }

    // Similar to the OverworldPathfinding, this uses an adapted version of Rich Davison's A* algorithm but across a sphere
    bool FindPath(Vector3 startingPos, Vector3 destinationPos)
    {
        path.Clear();

        Node startNode = GetNodeForPosition(startingPos);
        Node destinationNode = GetNodeForPosition(destinationPos);

        if (!startNode || !destinationNode || destinationNode.IsImpassable())
        {

            return false;
        }

        startNode.parent = null;
        startNode.g = 0.0f;
        startNode.h = CalculateHeuristic(startNode, destinationPos);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node node = openList[0];
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].GetFScore() < node.GetFScore())
                {
                    node = openList[i];
                }
            }
            if (node == destinationNode)
            {
                while (node)
                {
                    path.Add(node);
                    node = node.parent;
                }
                return true;
            }

            foreach (Node n in node.neighbours)
            {
                if (closedList.Contains(n))
                {
                    continue;
                }
                if (n.IsImpassable())
                {
                    closedList.Add(node);
                    continue;
                }
                float newH = CalculateHeuristic(n, destinationPos);
                float newG = node.GetFScore() + n.traversalCost;
                float newF = newG + newH;

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

    // In order to determine which node is closed to the transform being checked, all the nodes are checked
    Node GetNodeForPosition(Vector3 pos)
    {
        Node currentClosestNode = null;
        float currentClosestDistance = Mathf.Infinity;

        foreach (GameObject node in sphereNodes)
        {
            float distance = (node.transform.position - pos).magnitude;
            if (distance < currentClosestDistance)
            {
                currentClosestDistance = distance;
                currentClosestNode = node.GetComponent<Node>();
            }
        }
        return currentClosestNode;
    }

    float CalculateHeuristic(Node n, Vector3 destination)
    {
        return MainToolbox.CalculateArcLength(n.transform.position, destination);
    }

    void BuildPath()
    {
        positions = new Vector3[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 upDirection = (path[i].transform.position - MainToolbox.planetTransform.position).normalized;
            positions[i] = path[i].transform.position + upDirection;
        }

       
    }

}