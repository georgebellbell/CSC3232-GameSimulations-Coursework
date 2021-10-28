using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
