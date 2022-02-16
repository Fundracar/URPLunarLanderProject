using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvasManager : MonoBehaviour
{

    private GameManager gameManagerRef;

    void Awake()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

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

                gameManagerRef.LaunchNewGame(); //The game manager handle's the scene switch.
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
