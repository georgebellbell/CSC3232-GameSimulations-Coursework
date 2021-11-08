using UnityEngine;
using UnityEngine.SceneManagement;

// Creates a 3D and interactive game menu, only one "button" option at moment but will be developed in part 2
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

    // When mouse hovers over object, each of the letters are highlighted in red to make interaction clear
    private void OnMouseEnter()
    {
        foreach(Renderer renderer in menuOptionRenderer)
        {
            renderer.material.color = buttonActive;
        }
    }

    // When mouse is no longer over letters of object, they return to normal
    private void OnMouseExit()
    {
        foreach (Renderer renderer in menuOptionRenderer)
        {
            renderer.material.color = buttonNotActive;
        }
    }

    // When mouse is clicked, the letters within the collider will have the GravityBody class enabled and they will fall to planet
    // After a bit of time the game will load the overworld
    private void OnMouseUp()
    {
        buttonNotActive = buttonActive;
        foreach (GravityBody gravityBody in menuOptionGravityBody)
        {
            gravityBody.enabled = true;
        }

        GetComponent<BoxCollider>().enabled = false;
        Invoke("PlayGame", 1.5f);
    }

    void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
