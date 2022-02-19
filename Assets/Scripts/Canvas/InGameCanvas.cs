using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InGameCanvas : MonoBehaviour
{
    #region  Variables
    [SerializeField] GameManager gameManagerRef;

    [Header("Player Object & Components References")]
    [SerializeField] GameObject playerReference;
    [SerializeField] Rigidbody2D playerRigidbodyReference;

    [Header("Text Objects & Components for the ship")]
    [SerializeField] GameObject textVerticalSpeedContainer;
    [SerializeField] TextMeshProUGUI verticalSpeedText;
    [SerializeField] GameObject textHorizontalSpeedContainer;
    [SerializeField] TextMeshProUGUI horizontalSpeedText;

    [Header("Text Objects & Components for the player")]
    [SerializeField] GameObject mainPlayerMessageContainer;
    [SerializeField] TextMeshProUGUI mainPlayerMessageText;
    [SerializeField] GameObject secondaryPlayerMessageContainer;
    [SerializeField] TextMeshProUGUI secondaryPlayerMessageText;
    [SerializeField] GameObject playerInstructionMessageContainer;
    [SerializeField] TextMeshProUGUI playerInstructiontext;

    [Header("Time Tracker")]

    float miliseconds;
    float seconds;
    float minutes;

    [SerializeField] GameObject TimeTrackerObject;
    [SerializeField] TextMeshProUGUI TimeTrackerText;


    #endregion
    void Awake()
    {
        InitializeInGameCanvas();
    }
    void FixedUpdate()
    {
        UpdateSpeedInfos();
        
    }
   

    #region Updating Infos
    private void UpdateSpeedInfos()
    {
        /* Should this be done on Update or FixedUpdate ?
              I figured that since this script is supposed to track velocity values that are highly physics based, 
              it would be more accurate to track them 'OnFixedUpdate' */
        if (playerReference != null)
        {
            verticalSpeedText.text = (Mathf.Abs(playerRigidbodyReference.velocity.y * 10f)).ToString();
            horizontalSpeedText.text = (Mathf.Abs(playerRigidbodyReference.velocity.x * 10f)).ToString();

            /*  if (verticalSpeedText.text.Length - 6 > 0 && horizontalSpeedText.text.Length - 6 > 0)
              {
                  verticalSpeedText.text.Substring(verticalSpeedText.text.Length - 6);
                  horizontalSpeedText.text.Substring(horizontalSpeedText.text.Length - 6);
              } */
        }
    }

    public IEnumerator TrackAndDisplayGameTime()
    {
        while (gameManagerRef.currentGamePhase == GameManager.GamePhase.GamePlaying)
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

 public void FindPlayerInScene()
    {
        /*Since the player doesn't spawn right away on scene loading, 
   this methods encapsulates the behaviors that 'finds and references the player object in the scene' 
   in order to "fire" it at the right time.
   This prevents the script from trying to find an element that hasn't been spawned yet. */
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerRigidbodyReference = playerReference.GetComponent<Rigidbody2D>();
    }
    private void InitializeInGameCanvas()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //All of those variables come of use in the "SetMessageToDisplay()" & "EnableMessageToDisplay()" methods.

        textVerticalSpeedContainer = GameObject.FindGameObjectWithTag("VerticalSpeedTextBox");
        verticalSpeedText = textVerticalSpeedContainer.GetComponent<TextMeshProUGUI>();

        textHorizontalSpeedContainer = GameObject.FindGameObjectWithTag("HorizontalSpeedTextBox");
        horizontalSpeedText = textHorizontalSpeedContainer.GetComponent<TextMeshProUGUI>();

        mainPlayerMessageContainer = GameObject.FindGameObjectWithTag("MainPlayerInfoTextBox");
        mainPlayerMessageText = mainPlayerMessageContainer.GetComponent<TextMeshProUGUI>();

        secondaryPlayerMessageContainer = GameObject.FindGameObjectWithTag("SecondaryPlayerInfoTextBox");
        secondaryPlayerMessageText = secondaryPlayerMessageContainer.GetComponent<TextMeshProUGUI>();

        playerInstructionMessageContainer = GameObject.FindGameObjectWithTag("PlayerInstructionsTextBox");
        playerInstructiontext = playerInstructionMessageContainer.GetComponent<TextMeshProUGUI>();

        TimeTrackerObject = GameObject.FindGameObjectWithTag("TimeTracker");
        TimeTrackerText = TimeTrackerObject.GetComponent<TextMeshProUGUI>();

    }
    public void DisplayMessageInfos(bool _instruct, GameManager.GamePhase gamePhase)
    {
        SetMessageToDisplay(gamePhase);
        EnableMessageDisplay(_instruct);
    }
    private void SetMessageToDisplay(GameManager.GamePhase _currentGamePhase)
    {
        switch (_currentGamePhase)
        {
            case GameManager.GamePhase.GameWaitingToStart:
                mainPlayerMessageText.text = "Prepare for landing";
                secondaryPlayerMessageText.text = "";
                playerInstructiontext.text = "Press SPACE to start landing procedure";
                break;

            case GameManager.GamePhase.GameLost:

                mainPlayerMessageText.text = "Landing Failed";

                playerInstructiontext.text = "SPACE to continue, Escape to save & quit !";

                if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.WentAway) secondaryPlayerMessageText.text = "Station Control lost your ship, as a result landing procedure was denied.";

                else if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.Terrain) secondaryPlayerMessageText.text = "You crashed the ship into terrain";

                else if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.Speed) secondaryPlayerMessageText.text = "Your speed was too high, as a result the ship crashed on landing.";

                break;

            case GameManager.GamePhase.GameWon:
                mainPlayerMessageText.text = "Landing Successful !";
                secondaryPlayerMessageText.text = "You managed to land the ship without incident";
                playerInstructiontext.text = "SPACE to continue, Escape to save & quit !";
                break;

            case GameManager.GamePhase.GamePlaying:
                //No text should be displayed when the game phase equals "GamePlaying"
                mainPlayerMessageText.text = null;
                secondaryPlayerMessageText.text = null;
                playerInstructiontext.text = null;
                break;
        }
    }
    private void EnableMessageDisplay(bool _instruction)
    {
        mainPlayerMessageContainer.SetActive(_instruction);
        secondaryPlayerMessageContainer.SetActive(_instruction);
        playerInstructionMessageContainer.SetActive(_instruction);
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
    #endregion
}
