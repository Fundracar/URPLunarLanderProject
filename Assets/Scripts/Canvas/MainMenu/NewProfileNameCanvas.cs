using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewProfileNameCanvas : MonoBehaviour
{
    public static NewProfileNameCanvas newProfileNameCanvas;
    public TMP_InputField playerNameInputField;
    public string submittedName;
    public TextMeshProUGUI errorTextComponent;

    private bool blinkCoroutineRunning = false;
    private Coroutine MessageBlinkCoroutine;
    void Awake()
    {
        newProfileNameCanvas = this;
        StartCoroutine(InitializeNewProfileNameCanvas());
    }

    public IEnumerator InitializeNewProfileNameCanvas()
    {
        playerNameInputField = GameObject.FindGameObjectWithTag("PlayerNameInputField").GetComponent<TMP_InputField>();
        errorTextComponent = GameObject.FindGameObjectWithTag("ProfileNameErrorText").GetComponent<TextMeshProUGUI>();
        yield return playerNameInputField;
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
            case "NewProfileConfirmButton":

                VerifyAndRegisterPlayerName();

                break;

            case "NewProfileBack":
                playerNameInputField.text = null;
                this.gameObject.SetActive(false);
                break;

            default:
                Debug.Log("Button Was Not Recognized");
                break;
        }
    }

    private void VerifyAndRegisterPlayerName()
    {
        if (!(submittedName.Contains(" "))) //If the name doesn't contain spaces.
        {
            if (submittedName.ToCharArray().Length != 0) //And that is isn't empty.
            {
                /*if(And that it doesn't already exist) 

                { */

                //Register the name somewhere

                StartCoroutine(LevelManager.levelManager.StartNewGame());

                /* } 

                else 
                {
                   DisplayPlayerNameErrorMessage("Exists");
                } */

            }

            else
            {
                DisplayPlayerNameErrorMessage("Empty");

            }
        }
        else
        {
            DisplayPlayerNameErrorMessage("Spaces");
        }
    }

    public void UpdatePlayerName()
    {
        submittedName = playerNameInputField.text;
    }

    private void DisplayPlayerNameErrorMessage(string _modality)
    {
        switch (_modality)
        {
            case "Spaces":
                errorTextComponent.text = "Pilot Name Cannot Contain Spaces";
                Debug.Log("Pilot Name Cannot Contain Spaces !");
                break;

            case "Empty":
                errorTextComponent.text = "Pilot Name Cannot Be Empty";
                Debug.Log("Pïlot Name Cannot Be Empty !");
                break;

            case "Exists":

                errorTextComponent.text = "Pilot Name is already registered";
                Debug.Log("This name is already registered !");
                break;

            default:
                Debug.Log("Case not recognized");
                break;
        }

        if (blinkCoroutineRunning == true && MessageBlinkCoroutine != null)
        {
            StopCoroutine(MessageBlinkCoroutine);
        }

        StartCoroutine(MakeErrorMessageBlink());
    }

    private IEnumerator MakeErrorMessageBlink()
    {
        blinkCoroutineRunning = true;
        for (int i = 0; i < 5; i++)
        {
            errorTextComponent.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            errorTextComponent.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        errorTextComponent.text = null;
        blinkCoroutineRunning = false;
    }

}
