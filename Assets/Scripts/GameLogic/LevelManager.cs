using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [SerializeField] GameManager gameManagerRef;

    public Scene currentScene { get; set; }


    void Awake()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
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
        if (!(SceneManager.GetSceneByBuildIndex((currentScene.buildIndex + 1)) == null)) //If a "next level" exists
        {
            SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
        }
        else
        {
            GoBackToMainMenu();
            Debug.Log("No next scene, going back to main menu.");
        }
    }
    public void GoBackToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void SetCurrentSceneValue()
    {
        currentScene = SceneManager.GetActiveScene();
    }

}
