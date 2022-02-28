using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] GameManager gameManagerRef;
    public LevelConditionner levelConditionnerRef;
    public Scene currentScene { get; set; }
    void Awake()
    {
        SetCurrentSceneValue();
        
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (!(currentScene.buildIndex == 0))
        {
            levelConditionnerRef = GameObject.FindGameObjectWithTag("LevelConditionManager").GetComponent<LevelConditionner>();
        }
    }
    #region Level Loading Tools
    public void LaunchNewGame()
    {
        SceneManager.LoadSceneAsync("Level 1 Scene", LoadSceneMode.Single);
    }
    public void RestartCurrentLevel()
    {
        SceneManager.LoadSceneAsync(currentScene.buildIndex);
    }
    public void LoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings > (currentScene.buildIndex + 1)) //If a "next level" exists
        {
            SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
        }
        else GoBackToMainMenu();
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void SetCurrentSceneValue()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    #endregion
    #region Level Setup
    public void DefaultLevelSetupRoutine()
    {
        switch (currentScene.buildIndex)
        {
            case 1:
                InitializePlanetState(2, 0, 0);
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
        /*
        #Wind represents the presence and strengh of the wind in the level.
     
         0 : No wind.
         1 :
         2 :
         3 :

        */

        InitializePlanetWind(_WindPresetValue);

        /*
        #Gravity represent the gravity scale of the ship's rigidbody

         0 : Default gravity scale value.
         1 :
         2 :
         3 :
         */

        InitializePlanetGravity(_GravityPresetValue);

        /*
        #Comet Represents the frequency and number of comets falling in the level.
        0 : No comets falling.
        1 :
        2 :
        3 :     
        */

        InitializePlanetComets(_CometPresetValue);

    }

    private void InitializePlanetWind(int __WindPresetValue)
    {
        switch (__WindPresetValue)
        {
            case 0:
                levelConditionnerRef.SetWindForce(0);
                break;

            case 1:
                levelConditionnerRef.SetWindForce(100);
                break;

            case 2:
                levelConditionnerRef.SetWindForce(200);
                break;

            case 3:
                levelConditionnerRef.SetWindForce(300);
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
