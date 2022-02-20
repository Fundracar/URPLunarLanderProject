using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public enum GamePhase { Setup, Menu, GameWaitingToStart, GamePlaying, GameLost, GameWon };
    /* Phases definition:
        Setup : Specific initialisation behaviours are launched.
        MainMenu = The player is inside a menu and not in game.
        GameWaitingToStart = The player is in a game level and is instructed to press a button for the game to start.
        Playing : The player is playing the game.
        LostLevel : The player crashed its ship and has to choose an option from here (restart level or leave)
        WonLevel : The player managed to land its ship and is being displayed its statistics.
    */
    public GamePhase currentGamePhase { get; private set; }  //Variable in which the current "state" information will be stored.
    public Scene currentScene { get; private set; }
    public LevelManager levelManagerRef { get; private set; }
    public InGameCanvas inGameCanvasComponent { get; private set; }
    public TimeTracker timeTrackerComponent { get; private set; }

    public FuelConsumption fuelConsumptionComponent;
    [SerializeField] GameObject inGameCanvasRef;
    [SerializeField] Coroutine timeCoroutine;

    [Header("Ship references variables")]
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] GameObject shipPrefab; //this is referenced by hand in the engine.
    [SerializeField] GameObject instanciatedShip;
    [SerializeField] Rigidbody2D instanciatedRigidbody2D;
    public ShipController shipControllerRef { get; private set; }
    [SerializeField] BoxCollider2D instanciatedShipCollider;
    [SerializeField] bool shipIsFrozen = false; //This isn't declared in the Ship because it is the game manager that "freezes" or "unfreezes" the controller.
    //The "ship" doesn't know what "being frozen" means in this context.ss

    #endregion
    #region Init & Update
    void Start()
    {
        SwitchOnGamePhase(GamePhase.Setup);
    }
    void FixedUpdate()
    {
        if (instanciatedShip != null) ManageShipState();//The "ManageShipState() method should not fire if there are not instanciated ship yet.   
    }
    #endregion
    #region Core Functions
    /*There are two core functions in the GameManager
    One that manages the game logic through the use of a "Game Phase" enum.
    Another that manages the player's ship in function of the player input values tracked in the "ShipController.cs" */
    public void SwitchOnGamePhase(GamePhase _gamePhaseToSet)
    {
        /*WHAT THIS DOES : This method is used to set the current game phase to the _parameter value
        Then, a switch statement is performed on this value to fire the appropriate behavior
        This function will be called when necessary to easily change the "game phase" */
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

        if (instanciatedShip != null)
        {
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
        else
        {
            Debug.Log("Character is Null");
        }
    }
    #endregion
    #region Game State linked methods.

    //All the following methods are firing thanks to the "SwitchOnGamePhase()" method
    private void Setup()
    {
        SetCurrentSceneValue();
        GetLevelManager();
        if (currentScene.buildIndex != 0)
        {
            spawnPosition = new Vector3(-2.95f, 2.5f, 3f);
            GetInGameCanvas();
            SwitchOnGamePhase(GamePhase.GameWaitingToStart);
        }
    }
    private void GameWaitingToStart()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);
        PlaceShipController(new Vector3(-2.95f, 2.5f, 3f));
    }
    private void GamePlaying()
    {
        inGameCanvasComponent.DisplayMessageInfos(false, currentGamePhase);
        timeCoroutine = StartCoroutine(timeTrackerComponent.TrackAndDisplayGameTime());
        fuelConsumptionComponent.shipConsumptionCoroutine = StartCoroutine(fuelConsumptionComponent.ShipFuelConsumptionCoroutine());
    }
    private void GameLost()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);
        StopCoroutine(timeCoroutine); StopCoroutine(fuelConsumptionComponent.shipConsumptionCoroutine);
    }
    private void GameWon()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);// ""
        StopCoroutine(timeCoroutine);
    }
    #endregion
    #region Ship Management
    //The following methods combine each other and are tools for the ship controller supervision.
    public void VerifyShipSpeedOnLanding()
    {
        //OBSELETE : The velocity of the ship is already close to 0 when this event is fired (since, by definition, a collision occured)
        if (shipControllerRef.shipRigidbody2D.velocity.y * 10f < 0.02 && shipControllerRef.shipRigidbody2D.velocity.y * 10f > -0.02)
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
    #region Reference Setting Tools
    //Various "tool" methods that can come of use.
    private void PlaceShipController(Vector3 _spawn)
    {
        /*WHAT THIS DOES : This method either spawns or places the shipController in the scene depending on wether it exists or not. */
        spawnPosition = _spawn;
        if (instanciatedShip == null) //If the ship doesn't exist in the scene, spawns it and references its valuable components.
        {
            InstantiateAndReferencePlayerShip();
            inGameCanvasComponent.FindPlayerInScene();
        }
        else //If it already exists, just re-place it at its spawn position.
        {
            RestartShipInitialState();
        }
    }
    private void GetLevelManager()
    {
        levelManagerRef = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
    private void GetInGameCanvas()
    {
        inGameCanvasRef = GameObject.FindGameObjectWithTag("GameCanvas");
        inGameCanvasComponent = inGameCanvasRef.GetComponent<InGameCanvas>();
        timeTrackerComponent = inGameCanvasComponent.GetComponent<TimeTracker>();
    }
    private void InstantiateAndReferencePlayerShip()
    {
        instanciatedShip = Instantiate(shipPrefab, spawnPosition, Quaternion.identity);
        instanciatedRigidbody2D = instanciatedShip.GetComponent<Rigidbody2D>();
        instanciatedShipCollider = instanciatedShip.GetComponent<BoxCollider2D>();
        shipControllerRef = instanciatedShip.GetComponent<ShipController>();
        fuelConsumptionComponent = instanciatedShip.GetComponent<FuelConsumption>();
        shipControllerRef.fuelValue = 10000f;
    }
    private void RestartShipInitialState()
    {
        instanciatedShip.transform.position = spawnPosition;
        shipIsFrozen = false;
        shipControllerRef.fuelValue = 10000f;
    }
    private void SetCurrentSceneValue()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    #endregion
}