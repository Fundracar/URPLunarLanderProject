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
    [SerializeField] GameObject playerReference;
    [SerializeField] Rigidbody2D playerRigidbodyReference;
    [SerializeField] ShipController playerShipController;

    [Header("Text Objects & Components for the ship")]
    [SerializeField] GameObject textVerticalSpeedContainer;
    [SerializeField] TextMeshProUGUI verticalSpeedText;
    [SerializeField] GameObject textHorizontalSpeedContainer;
    [SerializeField] TextMeshProUGUI horizontalSpeedText;
    [SerializeField] GameObject fuelTextContainer;
    [SerializeField] TextMeshProUGUI fuelText;
    [SerializeField] GameObject levelTextContainer;
    [SerializeField] TextMeshProUGUI levelText;

    [Header("Text Objects & Components for the player")]
    [SerializeField] GameObject mainPlayerMessageContainer;
    [SerializeField] TextMeshProUGUI mainPlayerMessageText;
    [SerializeField] GameObject secondaryPlayerMessageContainer;
    [SerializeField] TextMeshProUGUI secondaryPlayerMessageText;
    [SerializeField] GameObject playerInstructionMessageContainer;
    [SerializeField] TextMeshProUGUI playerInstructiontext;

    #endregion
    #region Init&Update
    void Start()
    {
        InitializeInGameCanvas();
        SetCurrentLevelInfo();
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
        verticalSpeedText.text = (Mathf.Abs(playerRigidbodyReference.velocity.y * 10f)).ToString();
        horizontalSpeedText.text = (Mathf.Abs(playerRigidbodyReference.velocity.x * 10f)).ToString();
    }
    private void UpdateFuelInfos()
    {
        fuelText.text = playerShipController.fuelValue.ToString();
    }
    private void SetCurrentLevelInfo()
    {
        levelText.text = "Level" + " " + SceneManager.GetActiveScene().buildIndex.ToString();
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

        fuelTextContainer = GameObject.FindGameObjectWithTag("FuelValueTextBox");
        fuelText = fuelTextContainer.GetComponent<TextMeshProUGUI>();

        levelTextContainer = GameObject.FindGameObjectWithTag("CurrentLevelTextBox");
        levelText = levelTextContainer.GetComponent<TextMeshProUGUI>();
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

                if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.WentAway) secondaryPlayerMessageText.text = "Station Control lost your ship, as a result landing procedure was denied.";

                else if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.Terrain) secondaryPlayerMessageText.text = "You crashed the ship into terrain";

                else if (gameManagerRef.shipControllerRef.causeOfDeath == ShipController.CauseOfDeath.Speed) secondaryPlayerMessageText.text = "Your speed was too high, as a result the ship crashed on landing.";

                break;

            case GameManager.GamePhase.GameWon:
                mainPlayerMessageText.text = "Landing Successful !";
                secondaryPlayerMessageText.text = "You managed to land the ship without incident";
                playerInstructiontext.text = "SPACE to continue, Escape to save & quit !";
                if (SceneManager.sceneCountInBuildSettings > (gameManagerRef.levelManagerRef.currentScene.buildIndex + 1))
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
        mainPlayerMessageContainer.SetActive(_instruction);
        secondaryPlayerMessageContainer.SetActive(_instruction);
        playerInstructionMessageContainer.SetActive(_instruction);
    }
    #endregion
}
