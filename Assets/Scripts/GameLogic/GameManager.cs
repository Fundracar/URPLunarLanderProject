using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    public enum GamePhase { Setup, GameWaitingToStart, GamePlaying, GameLost, GameWon };
    public GamePhase currentGamePhase { get; private set; }
    public LevelManager levelManagerRef { get; private set; }
    public InGameCanvas inGameCanvasComponent { get; private set; }
    public GameObject mainMenuCanvasObject;
    public TimeTracker timeTrackerComponent { get; private set; }
    private ScoreCalculator scoreCalculatorRef;
    private Coroutine timeCoroutine;
    private Coroutine windCoroutine;

    [Header("Ship Data")]
    [SerializeField] GameObject shipPrefab, instanciatedShip;
    private Vector3 spawnPosition;
    public Rigidbody2D instanciatedRigidbody2D { get; set; }
    public ShipController shipControllerRef { get; private set; }
    public EdgeCollider2D instanciatedShipCollider { get; set; }
    public FuelConsumption fuelConsumptionComponent { get; set; }
    private bool shipIsFrozen = false;

    #endregion
    #region Init & Update
    void Start()
    {
        spawnPosition = new Vector3(-2.5f, 2.2f, 3f);

        GetScoreCalculatorComponent();

        GetTimeTrackerComponent();

        GetLevelManagerComponent();

        mainMenuCanvasObject = GameObject.FindGameObjectWithTag("MainMenuCanvas");

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
        #WHAT THIS DOES : This methods conditions what behaviors are avalaible to the player's Ship according to the current game phase.
        #This should be called "OnFixedUpdate"  
        #Example : While "Playing", the ship's movement are not constrained ect.
        #CASE GUARD : For each case this methods should check, we take into account when the "ConstraintShipMovements"
        has already been fired in order not to fire it everyframe. */
        switch (currentGamePhase)
        {
            case GamePhase.GameWaitingToStart when shipIsFrozen == true:
                ConstraintShipMovements(true);
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
        switch (levelManagerRef.listOfCurrentlyLoadedScenes.Count)
        {
            case 1: //Only the main menu scene is loaded.
                mainMenuCanvasObject.SetActive(true);
                break;

            case 2: //The main menu scene + a level scene are loaded.
                GetInGameCanvasComponent();
                inGameCanvasComponent.SetCurrentLevelInfo();
                mainMenuCanvasObject.SetActive(false);
                SwitchOnGamePhase(GamePhase.GameWaitingToStart);
                break;
        }
    }
    private void GameWaitingToStart()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);
        PlaceShipController();
        levelManagerRef.DefaultLevelSetupRoutine();
        //The switch to "GamePlaying" is made by the player on ReadyKeyPressed (shipcontroller.cs)
    }
    private void GamePlaying()
    {
        inGameCanvasComponent.DisplayMessageInfos(false, currentGamePhase);

        timeCoroutine = StartCoroutine(timeTrackerComponent.TrackAndDisplayGameTime());

        if (!(levelManagerRef.levelConditionnerRef.windManagerRef.windForceValue == 0))
        {
            windCoroutine = StartCoroutine(levelManagerRef.levelConditionnerRef.windManagerRef.WindforceCoroutine(shipControllerRef, levelManagerRef.levelConditionnerRef.windManagerRef.windForceValue));
        }
        fuelConsumptionComponent.shipConsumptionCoroutine = StartCoroutine(fuelConsumptionComponent.ShipFuelConsumptionCoroutine());
    }
    private void GameLost()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);
        StopInGameCoroutines();
    }
    public void GameWon()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);
        StopInGameCoroutines();
        ScoreUpdateRoutine();
    }
    private void ScoreUpdateRoutine()
    {
        float calculatedTimeInSeconds = timeTrackerComponent.CalculateTimeInSecondsFromTimeTracker();
        float fuelLeft = shipControllerRef.fuelValue;
        // + way to get the plateforme bonus hit
        float calculatedScore = scoreCalculatorRef.CalculateScoreForCurrentLevel(calculatedTimeInSeconds, fuelLeft, 1f /* hitplateform bonus */);
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
            inGameCanvasComponent.FindPlayerInScene();
        }
        else RestartShipInitialState();

    }
    private void RestartShipInitialState()
    {
        instanciatedShip.transform.position = spawnPosition;
        shipIsFrozen = false;
        shipControllerRef.fuelValue = 1000f;
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
        //OBSELETE : The velocity of the ship is already close to 0 when this event is fired (since, by definition, a collision occured)

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
        //This will help us determine 
    }
    private void UpdateShipSpeed()
    {
        /*This checks wether the ship is within the defined constraints and if so, adds a calculated force to it.
        The BOTTOM limit is not considered by this method, as it will be represented by Terrain with its own rigidbody. */
        if (IsShipPositionWithinBound() == true)
        {
            shipControllerRef.AddForceToShip();
        }
    }
    private bool IsShipPositionWithinBound()
    {
        if (instanciatedShip.gameObject.transform.position.x > -3f && instanciatedShip.gameObject.transform.position.x < 3f && !(instanciatedShip.gameObject.transform.position.y > 2.8f))
        {
            return true;
        }
        else
        {
            SwitchOnGamePhase(GamePhase.GameLost);
            return false;
        }
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
    #region Valuable Components Tools
    private void GetLevelManagerComponent()
    {
        levelManagerRef = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
    private void GetInGameCanvasComponent()
    {
        inGameCanvasComponent = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<InGameCanvas>();
    }
    private void GetScoreCalculatorComponent()
    {
        scoreCalculatorRef = GetComponent<ScoreCalculator>();
    }
    private void GetTimeTrackerComponent()
    {
        timeTrackerComponent = GetComponent<TimeTracker>();
    }




 
    #endregion
}