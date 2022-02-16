using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DefineClickedButton(GameObject _buttonClicked)

    {

        //This will define which button was clicked and trigger the appropriate behavior

        string tagToCheck = _buttonClicked.tag;

        switch (tagToCheck)
        {
            case "NewGameButton":
                Debug.Log("NewGame Button was clicked");
                break;

            case "LoadGameButton":
                Debug.Log("LoadGame Button was clicked");

                break;

            case "OptionsButton":
                Debug.Log("OptionButton");
                break;

            case "QuitGameButton":
                Debug.Log("QuitGameButton");
                break;

            default:
                Debug.Log("Button Was Not Recognized");
                break;
        }

    }
}
