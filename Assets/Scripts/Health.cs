using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum RoverParts
{
    Body,
    Wheel
}

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image healthBarImage;
   
    float health;
    float maxHealth = 100;

    float changeSpeed;

    PickupManager pickupManager;
    PickupGenerator pickupGenerator;

    [SerializeField] bool takeDamage;

    void Start()
    {
        health = maxHealth;
        pickupManager = GetComponent<PickupManager>();
        pickupGenerator = FindObjectOfType<PickupGenerator>();
    }

    void Update()
    {

        if (!takeDamage)
        {
            takeDamage = true;
            TakeDamage(10f);
        }
        healthText.text = health + "%";

        if (health > maxHealth)
        {
            health = maxHealth;
            pickupGenerator.ChangeHealthChance(-0.7);
        }
            

        changeSpeed = 3f * Time.deltaTime;

        FillHealthBar();
        ChangeColour();
    }

    void FillHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, health / maxHealth, changeSpeed);
    }

    private void ChangeColour()
    {
        Color healthColour = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        healthBarImage.color = healthColour;
        healthText.color = healthColour;

    }

    void TakeDamage(float damagePoints)
    {
        pickupGenerator.ChangeHealthChance((damagePoints * 2) / maxHealth);
        pickupGenerator.ChangeJumpChance(0.2);
        pickupGenerator.ChangeSpeedChance(0.15);

        if (health > 0)
        {
            health = Mathf.Max(0, health - damagePoints);
        }
        
        if (health <= 0)
        {
            StartCoroutine(LoseGame());
        }
               
    }

    IEnumerator LoseGame()
    {
        // death animation
        // death sound
        GetComponent<RoverController>().enabled = false;
        yield return new WaitForSeconds(1f);
        FindObjectOfType<ManagementSystem>().LoseGame();

    }

    public void GainHealth(float healPoints)
    {
        if (health < maxHealth)
        {
            health = health + healPoints;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (pickupManager.SpeedPickupActive() && other.gameObject.CompareTag("Meteor"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickupManager.SpeedPickupActive() && other.gameObject.CompareTag("Crater"))
        {
            Destroy(other.gameObject);
        }
    }

    public void OnChildCollisionEnter(RoverParts part, float damage)
    {
        switch (part)
        {
            case RoverParts.Body:
                {
                    TakeDamage(damage);
                    //UI iamge representing this
                    Debug.Log("Main Body Hit!");
                    break;
                }
            case RoverParts.Wheel:
                {
                    TakeDamage(damage * 0.25f);
                    Debug.Log("Wheel Hit!");
                    break;
                }

        }
    }

   
}
