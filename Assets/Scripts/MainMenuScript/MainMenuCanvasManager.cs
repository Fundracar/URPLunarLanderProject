using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvasManager : MonoBehaviour
{

    [SerializeField] GameManager gameManagerRef;
    void Awake()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ManageClickedButton(GameObject _buttonClicked)
    {
        SwitchOnButtonClicked(_buttonClicked);
    }
    private void SwitchOnButtonClicked(GameObject _buttonClicked)

    {
        //This will define which button was clicked and trigger the appropriate behavior

        string tagToCheck = _buttonClicked.tag;

        switch (tagToCheck)
        {
            case "NewGameButton":

               StartCoroutine(gameManagerRef.levelManagerRef.StartNewGame()); //The game manager handle's the scene switch.
            
                break;

            case "LoadGameButton":
                
                break;

            case "OptionsButton":
                
                break;

            case "QuitGameButton":
                
                break;

            default:
                Debug.Log("Button Was Not Recognized");
                break;
        }

    }

    
}
