using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = Mathf.Clamp(-10f, -15f, -2f);
    public float distance;

    public float planetMass = 160;

    public void Attract(Transform player, float playerMass)
    {
        Vector3 upGravity = (player.position - transform.position).normalized;
        Vector3 upBody = player.up;

        gravity = -GravitationalCalculation(player, playerMass);

        player.GetComponent<Rigidbody>().AddForce(upGravity * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(upBody, upGravity) * player.rotation;
        player.rotation = Quaternion.Slerp(player.rotation, targetRotation, 50 + Time.deltaTime);


    }

    float GravitationalCalculation(Transform player, float playerMass)
    {
        float massProduct = planetMass * playerMass;

        distance = Vector3.Distance(this.transform.position, player.transform.position);

        float gravitationalForce = massProduct / (distance * distance);

        return gravitationalForce;
    }

}
