using UnityEngine;

// Simple class used as a reference for finding coins as well as creating a spinning effect for the coin
public class Coin : MonoBehaviour
{
    [SerializeField] float rotateSpeed;

    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, rotateSpeed * Time.deltaTime);
    }
}
