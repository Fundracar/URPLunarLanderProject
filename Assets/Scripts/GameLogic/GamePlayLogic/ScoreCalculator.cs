using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public static ScoreCalculator scoreCalculator;

    void Start()
    {
        scoreCalculator = this;
    }
    public float CalculateScoreForCurrentLevel(float _TimeElapsedInSeconds, float _FuelLeft, float _PlateformBonus)
    {
        float calculatedScore = (((_FuelLeft * 5) * _PlateformBonus) / _TimeElapsedInSeconds)*10f;
        Debug.Log("MyScore is" + " " + calculatedScore);
        return calculatedScore;
    }
}
