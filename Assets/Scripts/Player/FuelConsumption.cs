using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelConsumption : MonoBehaviour
{
    public Coroutine shipConsumptionCoroutine;
    private ShipController shipControllerRef;

    void Awake()
    {
        InitializeReferences();
    }
    private void InitializeReferences()
    {
        shipControllerRef = GetComponent<ShipController>();
    }
    public IEnumerator ShipFuelConsumptionCoroutine()
    {
        while (GameManager.gameManager.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            ConsumeFuel();
            yield return new WaitForSeconds(1f);
        }
    }
    private void ConsumeFuel()
    {
        float fuelToConsume = ((shipControllerRef.LateralThrustInputValue + shipControllerRef.UpThurstInputValue) / 2) * 10;

        shipControllerRef.fuelValue -= Mathf.Abs(fuelToConsume); //Mathf.Abs() is used to keep the operation from becoming an addition.

        if (shipControllerRef.fuelValue < 0) shipControllerRef.fuelValue = 0;
    }
}
