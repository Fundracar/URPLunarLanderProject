using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public ShipController shipControllerRef;
    public Animator shipAnimatorRef;

    void Awake()
    {
        InitializeRotator();
    }

    void FixedUpdate()
    {
        shipAnimatorRef.SetFloat(shipAnimatorRef.GetParameter(0).name, shipControllerRef.LateralThrustInputValue);
        shipAnimatorRef.SetFloat(shipAnimatorRef.GetParameter(1).name, shipControllerRef.UpThurstInputValue);
    }

    private void InitializeRotator()
    {
        shipControllerRef = GetComponent<ShipController>();
        shipAnimatorRef = GetComponent<Animator>();
    }
}
