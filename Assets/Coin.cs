using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float rotateSpeed;


    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, rotateSpeed * Time.deltaTime);
    }
}
