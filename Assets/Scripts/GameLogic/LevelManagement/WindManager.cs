using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public float windForceValue;
    void Start()
    {
        InitializeWindManager();
    }

    private void InitializeWindManager()
    {

    }

    public IEnumerator WindforceCoroutine(ShipController _ShipControllerRef, float _Parameter)
    {
        while (GameManager.gameManager.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            ApplyWindForce(_ShipControllerRef, _Parameter);

            yield return null;
        }
    }
    private void ApplyWindForce(ShipController _ShipControllerRef, float _WindForceValue)
    {
        _ShipControllerRef.shipRigidbody2D.AddForce(new Vector3(_WindForceValue, 0, 0));
    }
}
