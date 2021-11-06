using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Will be developed in part 2 to include pathfinding to create path of planets
public class OverworldNavigator : MonoBehaviour
{

    [SerializeField] GameObject selectedCanvas;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI massText;
    [SerializeField] TextMeshProUGUI gameTypeText;
    [SerializeField] GameObject stats;

    [SerializeField] GameObject camera;

    float yChange;
    float xChange;
    float zChange;

    private void Update()
    {
       
        ChangeCameraPosition();
    }

    private void ChangeCameraPosition()
    {
        

        Vector3 originalPosition = camera.transform.position;

        if (Input.GetKey(KeyCode.Mouse2))
        {
            xChange = Mathf.Clamp(originalPosition.x - Input.GetAxisRaw("Mouse X"), -10, 10);
            zChange = Mathf.Clamp(originalPosition.z - Input.GetAxisRaw("Mouse Y"), -4, 4);
        }
        yChange = Mathf.Clamp(-Input.GetAxisRaw("Mouse ScrollWheel") + originalPosition.y, 5, 10);
        Vector3 newPosition = new Vector3(xChange, yChange, zChange);

        camera.transform.position = newPosition;
    }

    public void SetPlanetStats(string planetName, string planetType, string planetMass, Transform transform)
    {
        nameText.text = planetName;
        massText.text = planetMass;
        gameTypeText.text = planetType;

        selectedCanvas.SetActive(true);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        stats.transform.position = screenPos;

    }

    public void SetPlanetStats()
    {
        nameText.text = "";
        massText.text = "";
        gameTypeText.text = "";

        selectedCanvas.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }


}
