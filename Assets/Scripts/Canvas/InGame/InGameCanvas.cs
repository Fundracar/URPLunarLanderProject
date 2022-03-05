using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class InGameCanvas : MonoBehaviour
{
    #region  Variables
    public GameManager gameManagerRef { get; private set; }

    [Header("Player Object & Components References")]
    private GameObject playerReference;
    [SerializeField] Rigidbody2D playerRigidbodyReference;
    [SerializeField] ShipController playerShipController;

    [Header("Text Objects & Components for the ship")]
    [SerializeField] TextMeshProUGUI verticalSpeedText, horizontalSpeedText, fuelText, levelText;

    [Header("Text Objects & Components for the player")]
    [SerializeField] TextMeshProUGUI mainPlayerMessageText, secondaryPlayerMessageText, playerInstructiontext;

    #endregion
    #region Init&Update
    void Start()
    {
        InitializeInGameCanvas();
    }
    void FixedUpdate()
    {
        if (playerReference != null)
        {
            UpdateSpeedInfos();
            UpdateFuelInfos();
        }
    }
    #endregion 
    #region Tools 
    private void UpdateSpeedInfos()
    {
        /* Should this be done on Update or FixedUpdate ?
              I figured that since this script is supposed to track velocity values that are highly physics based, 
              it would be more accurate to track them 'OnFixedUpdate' */
        verticalSpeedText.text = "Vertical Speed" + " " + (Mathf.Round(Mathf.Abs(playerRigidbodyReference.velocity.y * 100f))).ToString();
        horizontalSpeedText.text = "Horizontal Speed" + " " + (Mathf.Round(Mathf.Abs(playerRigidbodyReference.velocity.x * 100f))).ToString();
    }
    private void UpdateFuelInfos()
    {
        fuelText.text = "FUEL" + " " + Mathf.Round(playerShipController.fuelValue).ToString();
    }
    public void SetCurrentLevelInfo()
    {
        levelText.text = "Level" + " " +  gameManagerRef.levelManagerRef.listOfCurrentlyLoadedScenes[1].buildIndex.ToString();
    }
    public void FindPlayerInScene()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerRigidbodyReference = playerReference.GetComponent<Rigidbody2D>();
        playerShipController = playerReference.GetComponent<ShipController>();
    }
    private void InitializeInGameCanvas()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        verticalSpeedText = GameObject.FindGameObjectWithTag("VerticalSpeedTextBox").GetComponent<TextMeshProUGUI>();
        horizontalSpeedText = GameObject.FindGameObjectWithTag("HorizontalSpeedTextBox").GetComponent<TextMeshProUGUI>();
        mainPlayerMessageText = GameObject.FindGameObjectWithTag("MainPlayerInfoTextBox").GetComponent<TextMeshProUGUI>();
        secondaryPlayerMessageText = GameObject.FindGameObjectWithTag("SecondaryPlayerInfoTextBox").GetComponent<TextMeshProUGUI>();
        playerInstructiontext = GameObject.FindGameObjectWithTag("PlayerInstructionsTextBox").GetComponent<TextMeshProUGUI>();
        fuelText = GameObject.FindGameObjectWithTag("FuelValueTextBox").GetComponent<TextMeshProUGUI>();
        levelText = GameObject.FindGameObjectWithTag("CurrentLevelTextBox").GetComponent<TextMeshProUGUI>();
    }
    #endregion
    #region UI Message display
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
                secondaryPlayerMessageText.text = null;
                playerInstructiontext.text = "Press SPACE to start landing procedure";
                break;

            case GameManager.GamePhase.GameLost:

                mainPlayerMessageText.text = "Landing Failed";

                playerInstructiontext.text = "SPACE to continue, Escape to save & quit !";

                if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.WentAway)
                {
                    secondaryPlayerMessageText.text = "Station Control lost your ship, as a result landing procedure was denied.";
                }
                else if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.Terrain)
                {
                    secondaryPlayerMessageText.text = "You crashed the ship into terrain";

                }

                else if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.Speed)
                {
                    secondaryPlayerMessageText.text = "Your speed was too high, as a result the ship crashed on landing.";
                }
                break;

            case GameManager.GamePhase.GameWon:

                mainPlayerMessageText.text = "Landing Successful !";
                secondaryPlayerMessageText.text = "You managed to land the ship without incident";
                playerInstructiontext.text = "SPACE to continue, Escape to save & quit !";

                if (gameManagerRef.levelManagerRef.listOfCurrentlyLoadedScenes[1].buildIndex < gameManagerRef.levelManagerRef.numberOfScenesInBuild - 1)
                {
                    playerInstructiontext.text = "Space to go back to Main menu !";
                    secondaryPlayerMessageText.text = "You completed all the landings ! Congratulations";
                }
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
        mainPlayerMessageText.gameObject.SetActive(_instruction);
        secondaryPlayerMessageText.gameObject.SetActive(_instruction);
        playerInstructiontext.gameObject.SetActive(_instruction);
    }
    #endregion
}