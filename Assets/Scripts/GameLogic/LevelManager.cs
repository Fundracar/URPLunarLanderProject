using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] GameManager gameManagerRef;
    public LevelConditionner levelConditionnerRef;
    public Scene currentLoadedLevelScene { get; set; }
    public List<Scene> listOfCurrentlyLoadedScenes;
    public Scene emptyScene;

    void Awake()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        levelConditionnerRef = GameObject.FindGameObjectWithTag("LevelConditionManager").GetComponent<LevelConditionner>();

        listOfCurrentlyLoadedScenes = new List<Scene>();
        listOfCurrentlyLoadedScenes.Add(SceneManager.GetActiveScene());
    }
    #region Level Loading Tools
    public IEnumerator StartNewGame()
    {
        AsyncOperation currentAsyncOp = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        while (!(currentAsyncOp.isDone)) //while the scene is still loading/unloading
        {
            yield return null; //keep checking 
        }

        listOfCurrentlyLoadedScenes.Add(SceneManager.GetSceneAt(1));

        gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.Setup);
    }

    public IEnumerator GoBackToMainMenu()
    {

        AsyncOperation currentAsyncOp = SceneManager.UnloadSceneAsync(listOfCurrentlyLoadedScenes[1]);

        while (!(currentAsyncOp.isDone)) //while the scene is still loaded
        {
            yield return null; //keep checking 
        }

        listOfCurrentlyLoadedScenes.Remove(listOfCurrentlyLoadedScenes[1]);

        gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.Setup);
    }
    public IEnumerator RestartLevel()
    {
        int buildIndex = listOfCurrentlyLoadedScenes[1].buildIndex;

        AsyncOperation unloadingAsyncOp = SceneManager.UnloadSceneAsync(listOfCurrentlyLoadedScenes[1]);

        while (!(unloadingAsyncOp.isDone)) //while the scene is still unloading
        {
            yield return null; //keep checking 
        }

        listOfCurrentlyLoadedScenes.Remove(listOfCurrentlyLoadedScenes[1]);

        AsyncOperation loadingAsyncOp = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

        while (!(loadingAsyncOp.isDone)) //while the scene is still loading
        {
            yield return null; //keep checking 
        }

        listOfCurrentlyLoadedScenes.Add(SceneManager.GetSceneAt(1));

        gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.Setup);

    }

    public void LoadNextLevel()
    {
        int sceneBuildIndex = currentLoadedLevelScene.buildIndex;

        SceneManager.UnloadSceneAsync(sceneBuildIndex); //unload current level

        if (SceneManager.sceneCountInBuildSettings >= sceneBuildIndex + 1)
        {
            SceneManager.LoadScene(sceneBuildIndex + 1, LoadSceneMode.Additive);
            currentLoadedLevelScene = SceneManager.GetSceneByBuildIndex(sceneBuildIndex + 1);
            //load next level.
        }

        gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.Setup);
    }

    /* public IEnumerator GoBackToMainMenu()
     {
         int sceneCount = SceneManager.sceneCount;
         if (sceneCount > 1)
         {
             Scene scene = SceneManager.GetSceneAt(sceneCount - 1);
             SceneManager.UnloadSceneAsync(scene);

             while (scene.isLoaded == true)
             {
                 yield return null;
             }

             gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.Setup);
         }
     } */

    #endregion
    #region Level Setup
    public void DefaultLevelSetupRoutine()
    {
        switch (currentLoadedLevelScene.buildIndex)
        {
            case 1:
                InitializePlanetState(2, 2, 0);
                break;

            case 2:
                InitializePlanetState(3, 3, 0);
                break;

            case 3:
                InitializePlanetState(1, 1, 0);
                break;

            case 4:
                InitializePlanetState(2, 0, 1);
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
                levelConditionnerRef.SetWindForce(0f);
                break;
            case 1:
                levelConditionnerRef.SetWindForce(0.10f);
                break;
            case 2:
                levelConditionnerRef.SetWindForce(0.15f);
                break;
            case 3:
                levelConditionnerRef.SetWindForce(0.20f);
                break;
        }

    }
    private void InitializePlanetGravity(int _GravityPresetValue)
    {
        switch (_GravityPresetValue)
        {
            case 0:
                levelConditionnerRef.SetShipGravityScaleValue(gameManagerRef.shipControllerRef, 0.025f);
                break;

            case 1:
                levelConditionnerRef.SetShipGravityScaleValue(gameManagerRef.shipControllerRef, 0.050f);
                break;

            case 2:
                levelConditionnerRef.SetShipGravityScaleValue(gameManagerRef.shipControllerRef, 0.075f);
                break;

            case 3:
                levelConditionnerRef.SetShipGravityScaleValue(gameManagerRef.shipControllerRef, 0.1f);
                break;

        }

    }
    private void InitializePlanetComets(int _CometPresetValue)
    {
        switch (_CometPresetValue)
        {
            case 0:
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
