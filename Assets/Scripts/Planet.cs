using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Planet : MonoBehaviour
{

    public enum PlanetType
    {
        survival,
        puzzle,
        menu
    };

    [SerializeField] float gravity = -10f;
    [SerializeField] float distance;
    [SerializeField] float maximumGravity = -10;
    [SerializeField] float minimumGravity = -1f;
    [SerializeField] float thisPlanetMass = 160;

    public PlanetType thisPlanetType = PlanetType.survival;
    

    public void Attract(Transform player, float playerMass)
    {
        Vector3 upGravity = (player.position - transform.position).normalized;
        Vector3 upBody = player.up;

        gravity = Mathf.Clamp(-GravitationalCalculation(player, playerMass), maximumGravity, minimumGravity);

        player.GetComponent<Rigidbody>().AddForce(upGravity * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(upBody, upGravity) * player.rotation;
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, 50 + Time.deltaTime);

        


    }

    float GravitationalCalculation(Transform player, float playerMass)
    {
        float massProduct = thisPlanetMass * playerMass;

        distance = Vector3.Distance(this.transform.position, player.transform.position);

        float gravitationalForce = massProduct / (distance * distance);

        return gravitationalForce;
    }

}
