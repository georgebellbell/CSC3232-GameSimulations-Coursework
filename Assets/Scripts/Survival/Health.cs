using System.Collections;
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
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBarImage;
   
    float health;
    float maxHealth = 100;

    float changeSpeed;

    PickupGenerator pickupGenerator;


    void Start()
    {
        health = maxHealth;

        pickupGenerator = FindObjectOfType<PickupGenerator>();
    }

    // every frame the displayed health is updated and capped at 100 and healthbar in UI is updated
    void Update()
    {
        healthText.text = health + "%";

        if (health > maxHealth)
        {
            health = maxHealth;
            pickupGenerator.ChangeHealthChance(-10);
        }
            

        changeSpeed = 3f * Time.deltaTime;

        FillHealthBar();
        ChangeColour();
    }

    // Updates the fill amount of a UI image to help represent how much health the player has and if they are doing the right thing
    void FillHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount, health / maxHealth, changeSpeed);
    }

    // Changes colour of health percent and bar from green to red depending on amount of health
    private void ChangeColour()
    {
        Color healthColour = Color.Lerp(Color.red, Color.green, (health / maxHealth));

        healthBarImage.color = healthColour;
        healthText.color = healthColour;

    }

    // Called in OnChildCollisionEnter via specific hit collision with rover (player) individual parts
    void TakeDamage(float damagePoints)
    {
        // if player takes damage the probability of each of the pickups spawning is increased
        pickupGenerator.ChangeHealthChance(3);
        pickupGenerator.ChangeJumpChance(0.5);
        pickupGenerator.ChangeSpeedChance(0.4);

        // if player health is above 0, damage will be taken, but minimum the health can go to is 0
        if (health > 0)
        {
            health = Mathf.Max(0, health - damagePoints);
        }
        
        // if player health is less than or equal to zero, a death coroutine will begin as the player will have lost
        if (health <= 0)
        {
            StartCoroutine(LoseGame());
        }
               
    }

    // Player death is called as a coroutine to allow world to naturally progress and give time for death animations and sounds
    // these aesthetic elements will be implemented during part 2
    IEnumerator LoseGame()
    {
        GetComponent<RoverStateMachine>().enabled = false;
        yield return new WaitForSeconds(1f);
        FindObjectOfType<ManagementSystem>().LoseGame();

    }

    // References the class PickupManager, specifically the Health Pickup which when picked up will give the player additional health.
    public void GainHealth(float healPoints)
    {
        // if player health is already at max, health will not increase
        if (health < maxHealth)
        {
            health = health + healPoints;
        }
    }
    /*
    // Collider around entire rover will destroy any Meteor objects that collide with it if the rover as a speed powerup active (immunity)
    private void OnCollisionEnter(Collision other)
    {
        if (pickupManager.PickupActive(pickupManager.speedTimeLeft) && other.gameObject.CompareTag("Meteor"))
        {
            Destroy(other.gameObject);
        }
    }

    // Similar to OnCollisionEnter but for the Crater objects
    private void OnTriggerEnter(Collider other)
    {
        if (pickupManager.PickupActive(pickupManager.speedTimeLeft) && other.gameObject.CompareTag("Crater"))
        {
            Destroy(other.gameObject);
        }
    }
    */

    // Used for specific collisions and is called via OnCollisionEnter on the objects underneath this class
    public void OnChildCollisionEnter(RoverParts part, float damage)
    {
        // depending on what part of the rover is hit, different damage will be taken
        switch (part)
        {
            case RoverParts.Body:
                {
                    TakeDamage(damage);
                    break;
                }
            case RoverParts.Wheel:
                {
                    TakeDamage(damage * 0.25f);
                    break;
                }

        }
    }

   
}
