using UnityEngine;

// Another design pattern I implemented was a Toolbox to keep values I repeatedly use, mostly things to do with the planet
public static class MainToolbox
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
