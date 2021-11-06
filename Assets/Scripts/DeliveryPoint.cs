using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    bool objectDelivered = false;

    Renderer renderer;
    PuzzleMode puzzleMode;

    [SerializeField] Color noObjectColour;
    [SerializeField] Color hasObjectColour;


    private void Start()
    {
        renderer = GetComponent<Renderer>();
        puzzleMode = FindObjectOfType<PuzzleMode>();

        renderer.material.color = noObjectColour;
    }

    private void OnTriggerEnter(Collider other)
    {
        renderer.material.color = hasObjectColour;
        objectDelivered = true;

        puzzleMode.CheckAllPointsCovered(); 

    }


    private void OnTriggerExit(Collider other)
    {
        renderer.material.color = noObjectColour;
        objectDelivered = false;
    }

    public bool HasObject()
    {
        return objectDelivered;
    }

}
