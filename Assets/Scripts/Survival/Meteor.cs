using UnityEngine;
using System.Collections;
public class Meteor : MonoBehaviour
{
    [SerializeField] GameObject meteorCrater;
    [SerializeField] AudioClip explosion;

    // Deals with the meteors after their generation and what happens when they collide with objects outside of the player
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            GenerateCrater(other);

        }
        ExplodeMeteor();
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crater"))
        {
            GenerateLargerCrater(other);
            ExplodeMeteor();
        }   
    }
    void ExplodeMeteor()
    {
        AudioSource.PlayClipAtPoint(explosion, transform.position);

        gameObject.SetActive(false);
    }

    // If meteor hits planet, it will instantiate a crater at the point of impact
    private void GenerateCrater(Collision other)
    {
        float planetRadius = MainToolbox.planetRadius;
        Vector3 distance = this.transform.position - other.transform.position;
        distance.Normalize();
        Vector3 impactPoint = other.transform.position + distance * planetRadius;
        Instantiate(meteorCrater, impactPoint, transform.rotation, transform.parent);
    }

    // If a meteor hits an existing crater, it will dynamically change its scale, and colldier, to make a larger crater
    private void GenerateLargerCrater(Collider other)
    {
        // Checks to see if crater is becoming too large
        if (other.gameObject.transform.localScale.x < meteorCrater.transform.lossyScale.x * 2)
        {
            other.gameObject.transform.localScale = other.transform.lossyScale * 1.5f;
        }
    }

}
