
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
        if (seconds <= 9)
        {
            if (minutes <= 9)
            {
                TimeTrackerText.text = "0" + minutes + ":" + "0" + seconds + ":" + miliseconds;

            }
            else TimeTrackerText.text = minutes + ":" + "0" + seconds + ":" + miliseconds;
        }
        else if (minutes <= 9 && seconds > 9)
        {
            TimeTrackerText.text = "0" + minutes + ":" + seconds + ":" + miliseconds;
        }
        /*This conditions the appearance of a raw "0" string besides the values, in regard of wether they are composed of two numbers of one.
        If "minutes" or "seconds" are composed of only 1 number, "0" will appear besides them in the UI. It will not otherwise. */
    }
    private float CalculateTimeInSecondsFromTimeTracker()
    {
        float timeInSecond = minutes * 60 + seconds;
        float modulo = miliseconds;
        return timeInSecond;
    }
}
