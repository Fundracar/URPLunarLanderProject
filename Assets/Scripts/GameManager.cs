using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Variables

    enum GamePhase { Test, Setup, Menu, WaitingToStart, Playing, LostLevel, WonLevel };
    /* Phases definition:

        Setup : Specific initialisation behaviours are launched.
        
        MainMenu = The player is inside a menu and not in game.

        WaitingToStart = The player is in a game level and is instructed to press a button for the game to start.

        Playing : The player is playing the game.

        LostLevel : The player crashed its ship and has to choose an option from here (restart level or leave)

        WonLevel : The player managed to land its ship and is being displayed its statistics.

    */


    private GamePhase currentGamePhase; //Variable in which the current state information will be stored.
    public GameObject shipPrefab; //this is referenced by hand in the engine.



    #endregion

    #region Init & Update
    void Awake()
    {
        currentGamePhase = GamePhase.Test;

        SwitchOnGamePhase(GamePhase.Setup);

    }

    void Update()
    {

    }

    #endregion

    private void SwitchOnGamePhase(GamePhase _gamePhaseToSet)
    {
        currentGamePhase = _gamePhaseToSet;


        switch (currentGamePhase)
        {
            case GamePhase.Setup:
                Setup();
                break;

            case GamePhase.Menu:
                Menu();
                break;

            case GamePhase.WaitingToStart:
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
        //This method will fire the "Setup Behavior"

        Debug.Log("CurrentGamePhase is" + " " + currentGamePhase);

        DontDestroyOnLoad(this.gameObject); //The GameObject containing the "this" component should not be destroyed between scenes. 

        Debug.Log("SETUP : The Game Manager's parent won't be destroyed on Load !");

        SwitchOnGamePhase(GamePhase.Menu); //Once the setup behavior is finished, this triggers the next game phase.

    }

    private void Menu()
    {
        //This is fired when the game state should be changed to 

        Debug.Log("MENU : Game Phase is Menu");
    }


}
