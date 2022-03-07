using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewProfileNameCanvas : MonoBehaviour
{

    [SerializeField] GameManager gameManagerRef;
    public TMP_InputField playerNameInputField;
    public string submittedName;
    void Start()
    {
        StartCoroutine(InitializeNewProfileNameCanvas());
    }

    public IEnumerator InitializeNewProfileNameCanvas()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        playerNameInputField = GameObject.FindGameObjectWithTag("PlayerNameInputField").GetComponent<TMP_InputField>();

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

                StartCoroutine(gameManagerRef.levelManagerRef.StartNewGame());

                /* } 

                else 
                {
                    Debug.Log("This name is already registered !");
                } */

            }

            else
            {
                Debug.Log("Pïlot Name Cannot Be Empty !");
            }
        }
        else
        {
            Debug.Log("Pilot Name Cannot Contain Spaces !");
        }
    }

    public void UpdatePlayerName()
    {
        submittedName = playerNameInputField.text;
    }


}
