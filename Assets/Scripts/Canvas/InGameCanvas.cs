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

    #endregion
    void Start()
    {
        InitializeInGameCanvas();
    }
    void FixedUpdate()
    {
        UpdateSpeedInfos();
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

    #region Updating Infos
    private void UpdateSpeedInfos()
    {
        /* Should this be done on Update or FixedUpdate ?
              I figured that since this script is supposed to track velocity values that are highly physics based, 
              it would be more accurate to track them 'OnFixedUpdate' */
        if (playerReference != null)
        {
            verticalSpeedText.text = (playerRigidbodyReference.velocity.y * 100f).ToString();
            horizontalSpeedText.text = (playerRigidbodyReference.velocity.x * 100f).ToString();
        }
    }
    private void DisplayWaitingForGameToStar()
    {


    }
    private void InitializeInGameCanvas()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
        gameManagerRef.inGameCanvasComponent = this;
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
                mainPlayerMessageText.text = "Station is ready";
                secondaryPlayerMessageText.text = "Prepare for landing on the designated plateforms";
                playerInstructiontext.text = "Press SPACE to start landing procedure";
                break;

            case GameManager.GamePhase.GameLost:
                mainPlayerMessageText.text = "Landing Failed";
                secondaryPlayerMessageText.text = "No further reasons displayed.";
                playerInstructiontext.text = "Press SPACE to try again, or Escape to save and quit !";

                break;

            case GameManager.GamePhase.GameWon:
                mainPlayerMessageText.text = "Landing Successful !";
                secondaryPlayerMessageText.text = "You managed to land the ship without incident";
                playerInstructiontext.text = "Press SPACE to continue, or Escape to save & quit !";
                break;

            case GameManager.GamePhase.GamePlaying:

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
    #endregion
}
