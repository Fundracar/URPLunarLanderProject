
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    public static LevelManager levelManager;
    public List<Scene> listOfCurrentlyLoadedScenes;
    public int numberOfScenesInBuild;
    void Awake()
    {
        levelManager = this;
        listOfCurrentlyLoadedScenes = new List<Scene>();
        listOfCurrentlyLoadedScenes.Add(SceneManager.GetActiveScene());
        numberOfScenesInBuild = SceneManager.sceneCountInBuildSettings;
    }
    #region Scene Management Coroutines
    public IEnumerator StartNewGame()
    {

        AsyncOperation currentAsyncOp = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return currentAsyncOp;

        listOfCurrentlyLoadedScenes.Add(SceneManager.GetSceneAt(1));

        GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.Setup);
    }

    public IEnumerator GoBackToMainMenu()
    {
        TimeTracker.timeTracker.ResetTime();
        AsyncOperation currentAsyncOp = SceneManager.UnloadSceneAsync(listOfCurrentlyLoadedScenes[1]);
        yield return currentAsyncOp; //keep checking 

        listOfCurrentlyLoadedScenes.Remove(listOfCurrentlyLoadedScenes[1]);

        GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.Setup);
    }
    public IEnumerator RestartLevel()
    {
        TimeTracker.timeTracker.ResetTime();
        int buildIndex = listOfCurrentlyLoadedScenes[1].buildIndex;

        AsyncOperation unloadingAsyncOp = SceneManager.UnloadSceneAsync(listOfCurrentlyLoadedScenes[1]);
        yield return unloadingAsyncOp;

        listOfCurrentlyLoadedScenes.Remove(listOfCurrentlyLoadedScenes[1]);

        AsyncOperation loadingAsyncOp = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        yield return loadingAsyncOp;

        listOfCurrentlyLoadedScenes.Add(SceneManager.GetSceneAt(1));

        GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.Setup);

    }

    public IEnumerator LoadNextLevel()
    {
        TimeTracker.timeTracker.ResetTime();
        if (listOfCurrentlyLoadedScenes[1].buildIndex < numberOfScenesInBuild - 1) //There IS a next level to be loaded.
        {
            int buildIndex = listOfCurrentlyLoadedScenes[1].buildIndex;

            AsyncOperation unloadingAsyncOp = SceneManager.UnloadSceneAsync(listOfCurrentlyLoadedScenes[1]);
            yield return unloadingAsyncOp;

            listOfCurrentlyLoadedScenes.Remove(listOfCurrentlyLoadedScenes[1]);

            AsyncOperation loadingAsyncOp = SceneManager.LoadSceneAsync((buildIndex + 1), LoadSceneMode.Additive);
            yield return loadingAsyncOp;

            listOfCurrentlyLoadedScenes.Add(SceneManager.GetSceneAt(1));

            GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.Setup);
        }
        else
        {
            StartCoroutine(GoBackToMainMenu());
        } //No Next Level to be loaded, proceeding to main menu.
    }
    #endregion
    #region Level Setup
    public void DefaultLevelSetupRoutine()
    {
        //The use of structs may come in handy in the following user cases.
        switch (listOfCurrentlyLoadedScenes[1].buildIndex)
        {
            case 1: //Level 1 
                InitializePlanetState(0, 0, 0);
                break;

            case 2:
                InitializePlanetState(0, 0, 0);
                break;

            case 3:
                InitializePlanetState(0, 0, 0);
                break;

            case 4:
                InitializePlanetState(0, 0, 0);
                break;

            case 5:
                InitializePlanetState(0, 0, 0);
                break;

            case 6:
                InitializePlanetState(0, 0, 0);
                break;
        }
    }
    private void InitializePlanetState(int _WindPresetValue, int _GravityPresetValue, int _CometPresetValue)
    {
        InitializePlanetWind(_WindPresetValue);
        InitializePlanetGravity(_GravityPresetValue);
        InitializePlanetComets(_CometPresetValue);
    }
    private void InitializePlanetWind(int __WindPresetValue)
    {
        switch (__WindPresetValue)
        {
            case 0:
                LevelConditionner.levelConditionner.SetWindForce(0f);
                break;

            case 1:
                LevelConditionner.levelConditionner.SetWindForce(0.10f);
                break;

            case 2:
                LevelConditionner.levelConditionner.SetWindForce(0.15f);
                break;

            case 3:
                LevelConditionner.levelConditionner.SetWindForce(0.20f);
                break;
        }

    }
    private void InitializePlanetGravity(int _GravityPresetValue)
    {
        switch (_GravityPresetValue)
        {
            case 0:
                LevelConditionner.levelConditionner.SetShipGravityScaleValue(GameManager.gameManager.shipControllerRef, 0.025f);
                break;

            case 1:
                LevelConditionner.levelConditionner.SetShipGravityScaleValue(GameManager.gameManager.shipControllerRef, 0.050f);
                break;

            case 2:
                LevelConditionner.levelConditionner.SetShipGravityScaleValue(GameManager.gameManager.shipControllerRef, 0.075f);
                break;

            case 3:
                LevelConditionner.levelConditionner.SetShipGravityScaleValue(GameManager.gameManager.shipControllerRef, 0.1f);
                break;

        }

    }
    private void InitializePlanetComets(int _CometPresetValue)
    {
        switch (_CometPresetValue)
        {
            case 0:
                //Comet frequency, size, speed
                break;

            case 1:
                break;

            case 2:
                break;

            case 3:
                break;
        }
    }

    #endregion

}
