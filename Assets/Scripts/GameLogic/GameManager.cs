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


    public GamePhase currentGamePhase;  //Variable in which the current state information will be stored.
    private Scene currentScene;
    public GameObject shipPrefab; //this is referenced by hand in the engine.
    public GameObject instanciatedShip;

    private Coroutine testCoroutine;


    #endregion

    #region Init & Update
    void Start()
    {
        SwitchOnGamePhase(GamePhase.Setup);
    }

    void Update()
    {

    }

    #endregion

    public void SwitchOnGamePhase(GamePhase _gamePhaseToSet)
    {
        currentGamePhase = _gamePhaseToSet;

        Debug.Log("SWITCH :" + " " + currentGamePhase);


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

                break;

            case GamePhase.LostLevel:
                break;

            case GamePhase.WonLevel:
                break;

            default:
                break;
        }
    }

    private void Setup()
    {

        DontDestroyOnLoad(this.gameObject); //The GameObject containing the "this" component should not be destroyed between scenes. 

        currentScene = SceneManager.GetActiveScene();

        SwitchOnGamePhase(GamePhase.Menu); //Once the setup behavior is finished, this triggers the next game phase.

    }

    private void InitializeMenu()
    {
        //This is fired when the game state should be changed to 

        Debug.Log("CurrentGamePhase is" + " " + currentGamePhase);

    }

    private void WaitingToStart()
    {
        testCoroutine = StartCoroutine(WaitForSeconds());

        Debug.Log("CurrentGamePhase is" + " " + currentGamePhase);
        //Get the ship and prevent it to move
    }

    private void GameStarting()
    {
        Debug.Log("Game has started, current state is " + " " + currentGamePhase);
    }


    public void LaunchNewGame()
    {

        SceneManager.LoadSceneAsync("Level 1 Scene", LoadSceneMode.Single);

        SwitchOnGamePhase(GamePhase.WaitingToStart);
    }

    private void SpawnShipController()
    {
        Vector3 spawnPosition = new Vector3(-2.95f, 2.5f, 3f);

        instanciatedShip = Instantiate(shipPrefab) as GameObject;

        instanciatedShip.transform.position = spawnPosition;
    }


    IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(1f);
        SpawnShipController();
    }

}
