using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    int[] levelsToPlay;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetPlanetPath(int[] planetList)
    {
        levelsToPlay = planetList;
    }

    public int[] GetPlanetPath()
    {
        return levelsToPlay;
    }

    
}
