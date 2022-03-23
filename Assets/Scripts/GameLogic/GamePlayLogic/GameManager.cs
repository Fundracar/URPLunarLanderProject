using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager gameManager;
    public enum GamePhase { Setup, GameWaitingToStart, GamePlaying, GameLost, GameWon };
    public GamePhase currentGamePhase { get; private set; }
    private Coroutine timeCoroutine, windCoroutine;

    [Header("Ship Data")]
    [SerializeField] GameObject shipPrefab, instanciatedShip;
    private Vector3 spawnPosition = new Vector3(-2.5f, 2.2f, 3f);
    private bool shipIsFrozen = false;
    public Rigidbody2D instanciatedRigidbody2D { get; set; }
    public EdgeCollider2D instanciatedShipCollider { get; set; }
    public ShipController shipControllerRef { get; private set; }
    public FuelConsumption fuelConsumptionComponent { get; set; }
    #endregion
    #region Init & Update
    void Start()
    {
        gameManager = this;
        SwitchOnGamePhase(GamePhase.Setup);
    }
    void FixedUpdate()
    {
        if (instanciatedShip != null)
        {
            ManageShipState();
        }
    }
    #endregion
    #region Core Functions
    /*
    #There are two core functions in the GameManager
    #One that manages the game logic through the use of a "Game Phase" enum.
    #Another that manages the player's ship in function of the player input values tracked in the "ShipController.cs" */
    public void SwitchOnGamePhase(GamePhase _gamePhaseToSet)
    {
        /*
        #This method is used to set the current game phase enum value to the _parameter value
        #Then, a switch statement is performed on said value to fire the appropriate behavior depending on the matching case. */

        currentGamePhase = _gamePhaseToSet;
        Debug.Log("CurrentGamePhase is" + " " + currentGamePhase);
        switch (currentGamePhase)
        {
            case GamePhase.Setup:
                Setup();
                break;

            case GamePhase.GameWaitingToStart:
                GameWaitingToStart();
                break;

            case GamePhase.GamePlaying:
                GamePlaying();
                break;

            case GamePhase.GameLost:
                GameLost();
                break;

            case GamePhase.GameWon:
                GameWon();
                break;

            default:
                break;
        }
    }
    private void ManageShipState()
    {
        /* 
        #This methods conditions what behaviors are avalaible to the player's Ship according to the current game phase.
        #This should be called "OnFixedUpdate" to better follow the actual physics behavior of the ship.  
        #User case Example : While in the "Playing" GameState, the ship's movement are not constrained ect.
        #CASE GUARDS : For each case this methods should check, we take into account when the "ConstraintShipMovements"
        has already been fired in order not to fire it everyframe, since we already know every case is evaluated anyway in a switch() statement */

        switch (currentGamePhase)
        {
            case GamePhase.GameWaitingToStart when shipIsFrozen == true:
                break;

            case GamePhase.GameWaitingToStart:
                ConstraintShipMovements(true);
                break;

            case GamePhase.GamePlaying when shipIsFrozen == false:
                UpdateShipSpeed();
                break;

            case GamePhase.GamePlaying:
                ConstraintShipMovements(false);
                UpdateShipSpeed();
                break;

            case GamePhase.GameWon when shipIsFrozen == true:
                break;

            case GamePhase.GameWon:
                ConstraintShipMovements(true);
                break;

            case GamePhase.GameLost when shipIsFrozen == true:
                break;

            case GamePhase.GameLost:
                ConstraintShipMovements(true);
                break;

            default:
                break;
        }
    }
    #endregion
    #region Game State linked methods.
    //All the following methods are firing thanks to the "SwitchOnGamePhase()" method
    private void Setup()
    {
        switch (LevelManager.levelManager.listOfCurrentlyLoadedScenes.Count)
        {
            case 1: //Only the main menu scene is loaded.
                MainMenuCanvasManager.mainMenuCanvasManager.gameObject.SetActive(true);
                NewProfileNameCanvas.newProfileNameCanvas.gameObject.SetActive(false);
                break;

            case 2: //The main menu scene + a level scene are loaded.

                MainMenuCanvasManager.mainMenuCanvasManager.gameObject.SetActive(false);
                InGameCanvas.inGameCanvas.SetCurrentLevelInfo();
                SwitchOnGamePhase(GamePhase.GameWaitingToStart);
                break;
        }
    }
    private void GameWaitingToStart()
    {
        InGameCanvas.inGameCanvas.DisplayMessageInfos(true, currentGamePhase);
        PlaceShipController();
        LevelManager.levelManager.DefaultLevelSetupRoutine();
        //The switch to "GamePlaying" is made by the player on ReadyKeyPressed (shipcontroller.cs)
    }
    private void GamePlaying()
    {
        InGameCanvas.inGameCanvas.DisplayMessageInfos(false, currentGamePhase);
        timeCoroutine = StartCoroutine(TimeTracker.timeTracker.TrackAndDisplayGameTime());

        if (!(LevelConditionner.levelConditionner.windManagerRef.windForceValue == 0))
        {
            windCoroutine = StartCoroutine(LevelConditionner.levelConditionner.windManagerRef.WindforceCoroutine(shipControllerRef, LevelConditionner.levelConditionner.windManagerRef.windForceValue));
        }

        fuelConsumptionComponent.shipConsumptionCoroutine = StartCoroutine(fuelConsumptionComponent.ShipFuelConsumptionCoroutine());
    }
    private void GameLost()
    {
        InGameCanvas.inGameCanvas.DisplayMessageInfos(true, currentGamePhase);
        StopInGameCoroutines();
    }
    public void GameWon()
    {
        StopInGameCoroutines();
        InGameCanvas.inGameCanvas.updatedPlayerScore = ((int)ScoreUpdateRoutine());
        InGameCanvas.inGameCanvas.DisplayMessageInfos(true, currentGamePhase);
    }
    private float ScoreUpdateRoutine()
    {
        float calculatedTimeInSeconds = TimeTracker.timeTracker.CalculateTimeInSecondsFromTimeTracker();
        float fuelLeft = shipControllerRef.fuelValue;
        int plateformBonus = shipControllerRef.plateformMultiplier;
        // + way to get the plateforme bonus hit
        float calculatedScore = ScoreCalculator.scoreCalculator.CalculateScoreForCurrentLevel(calculatedTimeInSeconds, fuelLeft,plateformBonus);

        return calculatedScore;
    }
    #endregion

    #region Ship Instantiation
    private void InstantiateAndReferencePlayerShip()
    {
        instanciatedShip = Instantiate(shipPrefab, spawnPosition, Quaternion.identity);
        instanciatedRigidbody2D = instanciatedShip.GetComponent<Rigidbody2D>();
        instanciatedShipCollider = instanciatedShip.GetComponent<EdgeCollider2D>();
        shipControllerRef = instanciatedShip.GetComponent<ShipController>();
        fuelConsumptionComponent = instanciatedShip.GetComponent<FuelConsumption>();
        shipControllerRef.fuelValue = 1000f;
    }
    private void PlaceShipController()
    {
        /*This method either spawns or places the shipController in the scene depending on wether it exists or not. */
        if (instanciatedShip == null) //If the ship doesn't exist in the scene, spawns it and references its valuable components.
        {
            InstantiateAndReferencePlayerShip();
            InGameCanvas.inGameCanvas.FindPlayerInScene();
        }
        else RestartShipInitialState();

    }
    private void RestartShipInitialState()
    {
        instanciatedShip.transform.position = spawnPosition;
        shipIsFrozen = false;
        shipControllerRef.fuelValue = 1000f;
        shipControllerRef.plateformMultiplier = 1;
    }
    private void StopInGameCoroutines()
    {
        StopCoroutine(timeCoroutine);
        StopCoroutine(fuelConsumptionComponent.shipConsumptionCoroutine);
    }

    #endregion
    #region Ship Instance Management
    //The following methods combine each other and are tools for the ship controller supervision.
    public void VerifyShipSpeedOnLanding()
    {
        //OBSELETE : The velocity of the ship is already close to 0 when this event is fired (since, by definition, a blocking collision occured)

        if (shipControllerRef.shipRigidbody2D.velocity.y * 100f < 50f && shipControllerRef.shipRigidbody2D.velocity.y * 10f > -50f)
        {
            SwitchOnGamePhase(GamePhase.GameWon);
        }
        else
        {
            shipControllerRef.causeOfDeath = ShipController.CauseOfDeath.Speed;
            SwitchOnGamePhase(GamePhase.GameLost);
        }
        //In the case of a collision with a landing plateform, this method will be performed to verify the ship of the speed.
    }
    private void UpdateShipSpeed()
    {
        shipControllerRef.AddForceToShip();
    }

    private void ConstraintShipMovements(bool _Instruction)
    {
        //This methods freezes or unfreezes the rigidbody2D constraints at will.
        if (_Instruction == true)
        {
            instanciatedRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            shipIsFrozen = _Instruction;
        }
        else
        {
            instanciatedRigidbody2D.constraints = RigidbodyConstraints2D.None;
            shipIsFrozen = _Instruction;
        }
    }
    #endregion
}