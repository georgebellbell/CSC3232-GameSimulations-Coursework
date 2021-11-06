using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GravityBody[] menuOptionGravityBody;
    [SerializeField] Renderer[] menuOptionRenderer;

    [SerializeField] Color buttonActive = Color.red;
    [SerializeField] Color buttonNotActive = Color.white;

    private void Start()
    {
        
        menuOptionGravityBody = GetComponentsInChildren<GravityBody>();
        menuOptionRenderer = GetComponentsInChildren<Renderer>();

    }
    private void OnMouseEnter()
    {
        foreach(Renderer renderer in menuOptionRenderer)
        {
            renderer.material.color = buttonActive;
        }
        Debug.Log("COnsidering Playing");
    }

    private void OnMouseExit()
    {
        foreach (Renderer renderer in menuOptionRenderer)
        {
            renderer.material.color = buttonNotActive;
        }
        Debug.Log("Decided against it");
    }

    private void OnMouseUp()
    {
        buttonNotActive = buttonActive;
        foreach (GravityBody gravityBody in menuOptionGravityBody)
        {
            gravityBody.enabled = true;
        }
        Debug.Log("Lets play!");

        GetComponent<BoxCollider>().enabled = false;
        Invoke("PlayGame", 1.5f);
    }

    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
