using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetData
{
    public bool planetCompleted;

    public PlanetData (bool isCompleted)
    {
        planetCompleted = isCompleted;
    }
}
