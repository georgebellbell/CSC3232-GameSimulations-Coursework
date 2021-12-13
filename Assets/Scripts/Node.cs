using UnityEngine;
using System.Collections.Generic;
using System;

public class Node : MonoBehaviour
{
    public List<Node> neighbours = new List<Node>();

    public float traversalCost;

    public float g, h;

    public Node parent;

    [SerializeField] float detectRadius, isObstacleRadius;

    enum NodeType
    {
        Passable,
        Rough,
        Impassable,
        Max
    }
    [SerializeField] NodeType nodeType = NodeType.Passable;

    public bool IsImpassable()
    {
        return nodeType == NodeType.Impassable;
    }

    public float GetFScore()
    {
        return g + h;
    }

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

    private void Start()
    {
        FindNeighbours();
        IsObstacle();
        SetupNode();
    }

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
}
