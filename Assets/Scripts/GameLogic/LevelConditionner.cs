using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConditionner : MonoBehaviour
{

    public GameManager gameManagerRef;
    public CometDispenser cometDispenserRef;
    public WindManager windManagerRef;


    // Start is called before the first frame update
    void Start()
    {
        InitialiazeLevelConditionner();
    }
    private void InitialiazeLevelConditionner()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        cometDispenserRef = GetComponent<CometDispenser>();
        windManagerRef = GetComponent<WindManager>();
    }
    public void SetShipGravityScaleValue(ShipController _targetShip, float _GravityScaleValue)
    {
        _targetShip.shipRigidbody2D.gravityScale = _GravityScaleValue;
    }
    public void SetWindForce(float _TargetWindForce)
    {
        windManagerRef.windForceValue = _TargetWindForce;
    }

}
