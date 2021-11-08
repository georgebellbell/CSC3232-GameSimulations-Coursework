using UnityEngine;

//Attached to each of the pickup objects, defining what they are during interactions with via PickupManager
public class Pickup : MonoBehaviour
{
    public enum PickupType
    {
        SpeedPickup,
        HealthPickup,
        JumpPickup
    };

    public PickupType ActivePickupType = PickupType.HealthPickup;
   
}
