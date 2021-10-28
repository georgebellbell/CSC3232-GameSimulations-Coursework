using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] GameObject meteorCrater;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {

            float planetRadius = other.gameObject.transform.localScale.x / 2;

            Vector3 distance = this.transform.position - other.transform.position;
            distance.Normalize();
            Vector3 impactPoint = other.transform.position + distance * planetRadius;
            GameObject newCrater = Instantiate(meteorCrater, impactPoint, transform.rotation);
            newCrater.transform.parent = transform.parent;
            
        }
        else if (other.gameObject.CompareTag("Crater"))
        {
            GameObject newCrater = Instantiate(meteorCrater, other.transform.position, transform.rotation);
            newCrater.transform.localScale = newCrater.transform.localScale * 1.5f;
            newCrater.transform.parent = transform.parent;
            Destroy(other.gameObject); 
        }

        Destroy(gameObject);

    }
}
