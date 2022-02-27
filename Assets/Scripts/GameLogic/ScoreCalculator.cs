using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{

    public float CalculateScoreForCurrentLevel(float _TimeElapsedInSeconds, float _FuelLeft, float _PlateformBonus)
    {
        float calculatedScore  = (_FuelLeft * _PlateformBonus) / _TimeElapsedInSeconds;

        Debug.Log(calculatedScore);
        return calculatedScore;
    }
}
