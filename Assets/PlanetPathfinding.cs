using System.Collections.Generic;
using UnityEngine;

public class PlanetPathfinding : MonoBehaviour
{
    [SerializeField] float scaling = 32;
    [SerializeField] int numberOfPoints = 128;

    Vector3[] positions;

    List<GameObject> uspheres;

    [SerializeField] LineRenderer lines;

    

    [SerializeField] float lineWidth;

    [SerializeField] GameObject node;

    private void Start()
    {
        lines.startWidth = lineWidth;
        lines.endWidth = lineWidth;

        Vector3[] points = PointsOnSphere(numberOfPoints);
        uspheres = new List<GameObject>();
        int i = 0;

        foreach (Vector3 value in points)
        {
            uspheres.Add(Instantiate(node));
            uspheres[i].transform.parent = transform;
            uspheres[i].transform.position = value * scaling;
            i++;
        }
       

    }
    Vector3[] PointsOnSphere(int n)
    {
        List<Vector3> upoints = new List<Vector3>();
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

            upoints.Add(new Vector3(x, y, z));
        }

        Vector3[] points = upoints.ToArray();
        return points;
    }


    List<Node> path = new List<Node>();

    

    public Vector3[] GetNewPath(Vector3 startingPos, Vector3 destinationPos)
    {
        if (FindPath(startingPos, destinationPos))
        {
            BuildPathGraphics();

        }
        else
        {
            Debug.Log("NO PATH");
        }
        return positions;
    } 
    Node GetNodeForPosition(Vector3 pos)
    {
        Node currentClosestNode = null;
        float currentClosestDistance = Mathf.Infinity;

        foreach (GameObject node in uspheres)
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
        //replace with arc
        return MainToolbox.CalculateArcLength((n.transform.position - destination).magnitude);
    }

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

    void BuildPathGraphics()
    {
        positions = new Vector3[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 upDirection = (path[i].transform.position - MainToolbox.planetTransform.position).normalized;
            positions[i] = path[i].transform.position + upDirection;
        }

        lines.positionCount = path.Count;
        lines.SetPositions(positions);
    }

    




}