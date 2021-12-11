using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDistribution : MonoBehaviour
{
    [SerializeField] float scaling = 32;
    [SerializeField] int numberOfPoints = 128;
    // Start is called before the first frame update
    void Start()
    {
        
        Vector3[] points = PointsOnSphere(numberOfPoints);
        List<GameObject> uspheres = new List<GameObject>();
        int i = 0;

        foreach (Vector3 value in points)
        {
            uspheres.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
            uspheres[i].transform.parent = transform;
            uspheres[i].transform.position = value * scaling;
            i++;
        }
    }

    Vector3[] PointsOnSphere(int n)
    {
        List<Vector3> upoints = new List<Vector3>();
        float inc = Mathf.PI * (3 - Mathf.Sqrt(5));
        float off = 2.0f / n;
        float x = 0;
        float y = 0;
        float z = 0;
        float r = 0;
        float phi = 0;

        for (int i = 0; i < n; i++)
        {
            y = i * off - 1 + (off / 2);
            r = Mathf.Sqrt(1 - y * y);
            phi = i * inc;
            x = Mathf.Cos(phi) * r;
            z = Mathf.Sin(phi) * r;

            upoints.Add(new Vector3(x, y, z));
        }

        Vector3[] points = upoints.ToArray();
        return points;
    }
}
