using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainToolbox // TO BE IMPLEMENTED
{
    public static Planet planet;
    public static float planetRadius;
    public static Transform playerTransform;
    public static Transform planetTransform;

    public static float CalculateArcLength(float distanceBetweenPoints)
    {
        float degree = 2 * Mathf.Asin(distanceBetweenPoints / (2 * planetRadius));

        return degree * planetRadius;
    }

}
