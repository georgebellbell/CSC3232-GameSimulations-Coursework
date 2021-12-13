using UnityEngine;
using System.Collections.Generic;

// Part of the advanced pathinding I implemented was nodes generated at run time. These are those nodes
public class Node : MonoBehaviour
{
    [SerializeField] float detectRadius, isObstacleRadius;
    [SerializeField] NodeType nodeType = NodeType.Passable;

    public List<Node> neighbours = new List<Node>();

    public float traversalCost;

    public float g, h;

    public Node parent;

    enum NodeType
    {
        Passable,
        Rough,
        Impassable,
        Max
    }

    private void Start()
    {
        FindNeighbours();
        IsObstacle();
        SetupNode();
    }

    // Uses the function OverlapSphere to find all nodes within a certain range which are labelled as this nodes neighbours
    void FindNeighbours()
    {
        neighbours.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectRadius);

        foreach (var hitCollider in hitColliders)
        {
            Node newNode = hitCollider.gameObject.GetComponent<Node>();

            if (newNode)
            {
                neighbours.Add(newNode);
            }

        }
    }

    // similar to FindNeighbours, Overlap sphere is used to check nearby colliders, but specifically Obstacles.
    // If there is any nearby, this object will be labelled as impassible to prevent enemy from trying to get through them
    private void IsObstacle()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, isObstacleRadius);

        foreach (var hitCollider in hitColliders)
        {
            GameObject obstacle = hitCollider.gameObject;

            if (obstacle.CompareTag("Obstacle"))
            {
                nodeType = NodeType.Impassable;
            }

        }
    }

    void SetupNode()
    {
        switch (nodeType)
        {
            case NodeType.Passable:
                traversalCost = 1;
                break;
            case NodeType.Impassable:
                traversalCost = -1;
                break;
            case NodeType.Rough:
                traversalCost = 2;
                break;
        }
    }

    public bool IsImpassable()
    {
        return nodeType == NodeType.Impassable;
    }


    public float GetFScore()
    {
        return g + h;
    }

    

    

    

    
}
