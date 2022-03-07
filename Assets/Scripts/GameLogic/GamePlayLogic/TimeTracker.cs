
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    public static TimeTracker timeTracker;
    private GameObject TimeTrackerTextObject;
    private TextMeshProUGUI TimeTrackerText;
    float miliseconds, seconds, minutes;

    void Start()
    {
        timeTracker = this;
    }
    public IEnumerator TrackAndDisplayGameTime()
    {
        TimeTrackerText = GameObject.FindGameObjectWithTag("TimeTracker").GetComponent<TextMeshProUGUI>();

        while (GameManager.gameManager.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            if (miliseconds <= 99)
            {
                miliseconds += 1f;
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
    public float CalculateTimeInSecondsFromTimeTracker()
    {
        float timeInSecond = (((minutes * 60) + seconds + (miliseconds / 100)));
        return timeInSecond;
    }

    public void RegisterLevelGameTime()
    {
        float timeInSeconds = CalculateTimeInSecondsFromTimeTracker();
        //Method to save in a file the value.
    }
}
