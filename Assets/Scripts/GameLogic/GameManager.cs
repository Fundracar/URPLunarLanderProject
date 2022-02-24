using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public enum GamePhase { Setup, GameWaitingToStart, GamePlaying, GameLost, GameWon };
    public GamePhase currentGamePhase { get; private set; }                 //The current "state" enum value.
    public LevelManager levelManagerRef { get; private set; }               //The "Level Manager" object's component reference.
    public InGameCanvas inGameCanvasComponent { get; private set; }         //The "InGameCanvas" object's component reference.
    public TimeTracker timeTrackerComponent { get; private set; }           //The Time tracker object's component reference.
    [SerializeField] GameObject inGameCanvasRef;                            //The "InGameCanvas" Object's reference.
    [SerializeField] Coroutine timeCoroutine;                               //The tracking and display of the time spent in game.

    [Header("Ship references variables")]
    [SerializeField] GameObject shipPrefab;                                 //Prefab from which the ship will be instanciated.
    [SerializeField] Vector3 spawnPosition;                                 //Represent the spawn position of the ship.
    [SerializeField] GameObject instanciatedShip;                           //The spawned instance of the ship.
    [SerializeField] Rigidbody2D instanciatedRigidbody2D;                   //The Rigidbody of the instanciated ship.
    public ShipController shipControllerRef { get; private set; }           //The ShipController component of the ship.
    [SerializeField] EdgeCollider2D instanciatedShipCollider;                //The BoxCollider2D of the ship.
    public FuelConsumption fuelConsumptionComponent;                        //The "FuelConsumption" object's component reference.
    [SerializeField] bool shipIsFrozen = false;                             //Conditions wether or not the ship's movements should be frozen or not.
    #endregion
    #region Init & Update
    void Start()
    {
        SwitchOnGamePhase(GamePhase.Setup);
    }
    void FixedUpdate()
    {
        if (instanciatedShip != null)
        {
            ManageShipState();
        
        }//The "ManageShipState() method should not fire if there are not instanciated ship yet. 

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
        GetLevelManager();
        levelManagerRef.SetCurrentSceneValue();
        if (levelManagerRef.currentScene.buildIndex > 0)
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
        StopInGameCoroutines();
    }
    public void GameWon()
    {
        inGameCanvasComponent.DisplayMessageInfos(true, currentGamePhase);// ""
        StopInGameCoroutines();
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
        instanciatedShipCollider = instanciatedShip.GetComponent<EdgeCollider2D>();
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

    private void StopInGameCoroutines()
    {
        StopCoroutine(timeCoroutine);
        StopCoroutine(fuelConsumptionComponent.shipConsumptionCoroutine);
    }
    #endregion
}