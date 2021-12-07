using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderClicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray r = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo))
            {
                PathfindingNode pathNode = hitInfo.collider.GetComponent<PathfindingNode>();

                if (pathNode)
                {
                    pathNode.ToggleNodeType();
                }
            }
        }
    }
}
