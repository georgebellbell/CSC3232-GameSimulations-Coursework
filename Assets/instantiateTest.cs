using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateTest : MonoBehaviour
{
    [SerializeField] GameObject point;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 newPos = Random.onUnitSphere * GetComponent<SphereCollider>().radius * transform.lossyScale.x;
            GameObject newObj = Instantiate(point);
            newObj.transform.position = newPos;
        }
    }
}
