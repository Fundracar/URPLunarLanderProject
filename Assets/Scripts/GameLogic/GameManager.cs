using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Variables



    public enum GamePhase { Setup, Menu, WaitingToStart, Playing, LostLevel, WonLevel };

    /* Phases definition:

        Setup : Specific initialisation behaviours are launched.
        
        MainMenu = The player is inside a menu and not in game.

        WaitingToStart = The player is in a game level and is instructed to press a button for the game to start.

        Playing : The player is playing the game.

        LostLevel : The player crashed its ship and has to choose an option from here (restart level or leave)

        WonLevel : The player managed to land its ship and is being displayed its statistics.

    */

    [Header("Phase & Scene Variables")]
    public GamePhase currentGamePhase;  //Variable in which the current state information will be stored.
    private Coroutine testCoroutine;
    private Scene currentScene;

    private GameObject inGameCanvasRef;


    [Header("Ship references variables")]
    public GameObject shipPrefab; //this is referenced by hand in the engine.
    public GameObject instanciatedShip;
    public Rigidbody2D instanciatedRigidbody2D;
    private ShipController shipControllerRef;
    private bool shipIsFrozen = false;

    #endregion

    #region Init & Update
    void Awake()
    {
        SwitchOnGamePhase(GamePhase.Setup);
    }
    void FixedUpdate()
    {
        if (instanciatedShip != null) //The "ManageShipState() method should not fire if there are not instanciated ship yet.
        {
            ManageShipState();
        }
    }
    #endregion

    #region Core Function
    public void SwitchOnGamePhase(GamePhase _gamePhaseToSet)
    {

        /*WHAT THIS DOES : This method is used to set the current game phase to the _parameter value
        Then, a switch statement is performed on this value to trigger to appropriate behavior
        This function will be called when necessary to easily change the "game phase" */

        currentGamePhase = _gamePhaseToSet;

        Debug.Log("CurrentGamePhase is" + " " + currentGamePhase);

        switch (currentGamePhase)
        {
            case GamePhase.Setup:
                Setup();
                break;

            case GamePhase.Menu:
                InitializeMenu();
                break;

            case GamePhase.WaitingToStart:
                WaitingToStart();
                break;

            case GamePhase.Playing:
                GamePlaying();

                break;

            case GamePhase.LostLevel:
                break;

            case GamePhase.WonLevel:
                break;

            default:
                break;
        }
    }

    private void ManageShipState()
    {

        /* WHAT THIS DOES : This methods conditions what behaviors are avalaible to the player's Ship 
        according to the current game phase.

        This should be called "OnFixedUpdate"
        
        Example : While "Playing", the ship's movement are not constrained ect. */

        switch (currentGamePhase)
        {
            case GamePhase.Setup:
                break;

            case GamePhase.Menu:
                break;

            case GamePhase.Playing:
                ConstraintMovement(false);
                UpdateShipSpeed();
                break;

            case GamePhase.WaitingToStart when shipIsFrozen == true:
                break;

            case GamePhase.WaitingToStart:
                ConstraintMovement(true);
                break;

            case GamePhase.WonLevel when shipIsFrozen == true:
                break;

            case GamePhase.WonLevel:
                ConstraintMovement(true);
                break;

            case GamePhase.LostLevel:
                break;

            default:
                break;
        }

    }

    #endregion

    #region Specific Game Phase Functions

    //All the following methods are firing thanks to the "SwitchOnGamePhase()" method
    private void Setup()
    {
        DontDestroyOnLoad(this.gameObject); //The GameObject containing the "this" component should not be destroyed between scenes. 
        currentScene = SceneManager.GetActiveScene();
        SwitchOnGamePhase(GamePhase.Menu); //Once the setup behavior is finished, this triggers the next game phase.
    }
    private void InitializeMenu()
    {
        //This is fired when the game state should be changed to 
    }
    private void WaitingToStart()
    {
        testCoroutine = StartCoroutine(SpawnPlayerAfterDelay());
    }
    private void GamePlaying()
    {

    }

    #endregion

    #region Tools

    //Various "tool" methods that can come of use.
    public void LaunchNewGame()
    {

        SceneManager.LoadSceneAsync("Level 1 Scene", LoadSceneMode.Single);

        SwitchOnGamePhase(GamePhase.WaitingToStart);
    }
    private void SpawnShipController()
    {
        Vector3 spawnPosition = new Vector3(-2.95f, 2.5f, 3f);

        instanciatedShip = Instantiate(shipPrefab) as GameObject;

        instanciatedRigidbody2D = instanciatedShip.GetComponent<Rigidbody2D>();

        shipControllerRef = instanciatedShip.GetComponent<ShipController>();

        instanciatedShip.transform.position = spawnPosition;

        inGameCanvasRef = GameObject.FindGameObjectWithTag("GameCanvas");

        InGameCanvas tempInGameCanvas = inGameCanvasRef.GetComponent<InGameCanvas>();

        tempInGameCanvas.FindPlayerInScene();

    }
    IEnumerator SpawnPlayerAfterDelay()
    {
        yield return new WaitForSeconds(0.01f);
        //this is because the Instanciate method wasn't working properly while performed during the loading of a new scene.
        SpawnShipController();
    }

    #endregion

    #region Ship Supervision

    //The following methods combine each other and are tools for the ship controller supervision.
    private void UpdateShipSpeed()
    {
        //This checks wether the ship is within the defined constraints and if so, adds a calculated force to it.

        if (IsShipPositionWithinBound() == true) AddForceToShip();
    }
    private void AddForceToShip()
    {
        float xSpeed = shipControllerRef.LateralThrustInputValue * shipControllerRef.accelerationFactor;

        float ySpeed = shipControllerRef.UpThurstInputValue * (shipControllerRef.accelerationFactor * 3f);

        instanciatedRigidbody2D.AddForce(new Vector2(xSpeed, ySpeed));
    }
    private bool IsShipPositionWithinBound()
    {
        if (instanciatedShip.gameObject.transform.position.x > -3f && instanciatedShip.gameObject.transform.position.x < 3f && !(instanciatedShip.gameObject.transform.position.y > 2.8f))
        {
            return true;
        }

        else
        {
            Debug.Log("Game Over ! Ship is outside of reach");
            return false;
        }
    }
    private void ConstraintMovement(bool _Instruction)
    {
        //This methods freezes or unfreezes the rigidbody2D constraints at will.
        if (_Instruction == true)
        {
            instanciatedRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            shipIsFrozen = true;
        }
        else
        {
            instanciatedRigidbody2D.constraints = RigidbodyConstraints2D.None;
            shipIsFrozen = false;
        }

    }

    #endregion
}
