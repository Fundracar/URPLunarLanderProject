using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProfileNameCanvas : MonoBehaviour
{

    [SerializeField] GameManager gameManagerRef;
    void Start()
    {
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        this.gameObject.SetActive(false);
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

                StartCoroutine(gameManagerRef.levelManagerRef.StartNewGame());

                break;

            case "NewProfileBack":
                this.gameObject.SetActive(false);
                break;

            default:
                Debug.Log("Button Was Not Recognized");
                break;
        }
    }


}
