using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{

    public GameManager gameManagerRef;
    public float windForceValue;
    void Start()
    {
        InitializeWindManager();
    }

    private void InitializeWindManager()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public IEnumerator WindforceCoroutine(ShipController _ShipControllerRef, float _Parameter)
    {
        while (gameManagerRef.currentGamePhase == GameManager.GamePhase.GamePlaying)
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
