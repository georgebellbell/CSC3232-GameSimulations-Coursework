using UnityEngine;

// This class acts similar to that of the Design Pattern of Singleton in that there will only ever exist one in the world and it stores key values between scenes
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
