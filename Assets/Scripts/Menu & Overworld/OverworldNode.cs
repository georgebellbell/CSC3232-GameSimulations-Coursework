using System.Collections.Generic;
using UnityEngine;

// Pathfinding node used by OverworldPathfinding as part of the A* algorithm
public class OverworldNode : MonoBehaviour
{ 
    public List<OverworldNode> allNeighbours = new List<OverworldNode>();
    public OverworldNode parent;

    [SerializeField] float edgeRange;
    public float traversalCost;

    public float g, h;
   
    void Start()
    {
        FindNeighbours();      
    }
    private void FindNeighbours()
    {
        foreach (OverworldNode node in FindObjectsOfType<OverworldNode>())
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

    public float GetFScore()
    {
        return g + h;
    }

    // Used in debugging to see which planets were neighbours to this one
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, edgeRange);
    }



}
