using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{

    public GameManager gameManagerRef;
    public ShipController shipControllerRef;
    public float windForceValue;

    // Start is called before the first frame update
    void Start()
    {
        shipControllerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().shipControllerRef;
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public IEnumerator WindforceCoroutine(ShipController _ShipControllerRef, float _Parameter)
    {
        while (gameManagerRef.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            int diceRoll = Random.Range(0, 101);

            if (diceRoll > 10)
            {
                ApplyWindForce(_ShipControllerRef, _Parameter);
            }

            else
            {
                ApplyWindForce(_ShipControllerRef, -(_Parameter));
            }

            yield return null;
        }
    }

    private void ApplyWindForce(ShipController _ShipControllerRef, float _WindForceValue)
    {
        _ShipControllerRef.shipRigidbody2D.AddForce(new Vector3(_WindForceValue, 0, 0));
    }
}
