//Data class that stores whether or not a planet has been beaten by the player or not
[System.Serializable]
public class PlanetData
{
    public bool planetCompleted;

    public PlanetData (bool isCompleted)
    {
        planetCompleted = isCompleted;
    }
}
