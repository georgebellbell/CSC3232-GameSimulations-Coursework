using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingNode : MonoBehaviour
{

    public List<PathfindingNode> allNeighbours = new List<PathfindingNode>();

    public float traversalCost;
    float defaultTraversalCost;

    public float g, h;
    public PathfindingNode parent;

    [SerializeField] float edgeRange;

    enum NodeType
    {
        Passable,
        Rough,
        Impassable,
        MAX
           
    }
    NodeType nodeType = NodeType.Passable;

    public bool isWanted;

    public bool IsImpassible()
    {
        return nodeType == NodeType.Impassable;
    }

    public float GetFScore()
    {
        return g + h;
    }
    // Start is called before the first frame update
    void Start()
    {
        defaultTraversalCost = traversalCost;
        FindNeighbours();
        //SetupNode();
    }

    private void FindNeighbours()
    {
        foreach (PathfindingNode node in FindObjectsOfType<PathfindingNode>())
        {
            if (node.transform == transform)
            {
                continue;
            }

            float distanceBetween = Vector3.Distance(transform.position, node.transform.position);

            if (distanceBetween < edgeRange)
                allNeighbours.Add(node);
        }
    }

    /*
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
    */
    public void ToggleNodeType()
    {
        isWanted = !isWanted;

        if (isWanted)
        {
            traversalCost = defaultTraversalCost;
        }
        else
        {
            traversalCost = -1;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, edgeRange);
    }



}
