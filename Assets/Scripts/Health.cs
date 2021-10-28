using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image healthBarImage;
   
    float health;
    float maxHealth = 100;

    float changeSpeed;

    PickupManager pickupManager;

    void Start()
    {
        health = maxHealth;
        pickupManager = GetComponent<PickupManager>();
    }

    void Update()
    {
        healthText.text = health + "%";

        if (health > maxHealth)
            health = maxHealth;

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
               
        if (health > 0)
        {
            health = health - damagePoints;
        }
        
        if (health <= 0)
        {
            FindObjectOfType<ManagementSystem>().LoseGame();
        }
               
    }

    public void GainHealth(float healPoints)
    {
        if (health < maxHealth)
        {
            health = health + healPoints;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Meteor"))
        {
            if (pickupManager.IsSpeedPickupActive())
            {
                Destroy(collision.gameObject);
            }
            else
            {
                TakeDamage(33f);
            }
            
        }

        if (collision.gameObject.CompareTag("Crater"))
        {
            if (pickupManager.IsSpeedPickupActive())
            {
                Destroy(collision.gameObject);
            }
            else
            {
                TakeDamage(5f);
            }
        }
    }
}
