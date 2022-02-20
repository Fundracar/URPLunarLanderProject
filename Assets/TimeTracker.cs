using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] GameObject TimeTrackerObject;
    [SerializeField] TextMeshProUGUI TimeTrackerText;

    float miliseconds, seconds, minutes;

    void Awake()
    {
        TimeTrackerObject = GameObject.FindGameObjectWithTag("TimeTracker");
        TimeTrackerText = TimeTrackerObject.GetComponent<TextMeshProUGUI>();
    }
    public IEnumerator TrackAndDisplayGameTime()
    {
        while (this.gameObject.GetComponent<InGameCanvas>().gameManagerRef.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            if (miliseconds <= 99)
            {
                miliseconds += 01f;
            }
            else //A second has passed.
            {
                miliseconds = 0;
                seconds += 1f;

                if (!(seconds <= 59))
                {
                    seconds = 0;
                    minutes += 1f;
                }
            }
            DisplayTimeTracker();
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void DisplayTimeTracker()
    {
        TimeTrackerText.text = minutes + ":" + seconds + ":" + miliseconds;
    }
    private float CalculateTimeInSecondsFromTimeTracker()
    {
        float timeInSecond = minutes * 60 + seconds;
        float modulo = miliseconds;
        return timeInSecond;
    }
}
