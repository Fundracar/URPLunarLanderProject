using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConditionner : MonoBehaviour
{
    public static LevelConditionner levelConditionner;
    public CometDispenser cometDispenserRef;
    public WindManager windManagerRef;


    // Start is called before the first frame update
    void Start()
    {
        levelConditionner = this;
        InitialiazeLevelConditionner();
    }
    private void InitialiazeLevelConditionner()
    {
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
