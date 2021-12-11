using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] int[] levelsToPlay;

    [SerializeField] string[] planets;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetPlanetPath(int[] planetList, string[] planetNames )
    {
        levelsToPlay = planetList;
        planets = planetNames; 
       
        
    }

    public int[] GetPlanetPath()
    {
        return levelsToPlay;
    }

    public string GetCurrentPlanet(int index)
    {
        return planets[index];
    }

    
}
