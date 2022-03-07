using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvasManager : MonoBehaviour
{
    public static MainMenuCanvasManager mainMenuCanvasManager;
    void Awake()
    {
        mainMenuCanvasManager = this;

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

                NewProfileNameCanvas.newProfileNameCanvas.gameObject.SetActive(true);

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
