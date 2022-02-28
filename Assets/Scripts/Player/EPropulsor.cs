using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EPropulsor : MonoBehaviour
{
    public ShipController shipControllerRef;

    void Awake()
    {
        shipControllerRef = GetComponent<ShipController>();
    }

    public void PropellShip()
    {
        if (shipControllerRef.fuelValue > 100)
        {
            shipControllerRef.fuelValue -= 100;
            shipControllerRef.shipRigidbody2D.AddRelativeForce(new Vector3(0, 200, 0));
        }
    }
}
