using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainToolbox // TO BE IMPLEMENTED
{
    public static Planet planet;
    public static float planetRadius;
    public static Transform playerTransform;
    public static Transform planetTransform;
    public static SphereCollider planetCollider;
    public static Planet.PlanetType planetType;

    public static float CalculateArcLength(Vector3 playerPos, Vector3 targetPos)
    {
        Vector3 spokeToActual = playerPos - planetTransform.position;
        Vector3 spokeToTarget = targetPos - planetTransform.position;

        float angleFromCenter = Vector3.Angle(spokeToActual, spokeToTarget);

        return 2 * Mathf.PI * planetRadius * (angleFromCenter / 360);
    }

}
