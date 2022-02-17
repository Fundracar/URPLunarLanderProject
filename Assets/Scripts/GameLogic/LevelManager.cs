using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] GameManager gameManagerRef;

    void Awake()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public void LaunchNewGame()
    {
        SceneManager.LoadSceneAsync("Level 1 Scene", LoadSceneMode.Single);
        gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.GameWaitingToStart);
    }
    public void RestartCurrentLevel()
    {
        SceneManager.LoadSceneAsync(gameManagerRef.currentScene.buildIndex);
    }
    public void LoadNextLevel()
    {
        if (!(SceneManager.GetSceneByBuildIndex((gameManagerRef.currentScene.buildIndex + 1)) == null)) //If a "next level" exists
        {
            SceneManager.LoadSceneAsync(gameManagerRef.currentScene.buildIndex + 1);
        }
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
