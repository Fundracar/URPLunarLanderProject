using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonScript : MonoBehaviour
{

    private MainMenuCanvasManager mainMenuCanvasManagerRef;


    void Start()
    {
        mainMenuCanvasManagerRef = GameObject.FindGameObjectWithTag("MainMenuCanvas").GetComponent<MainMenuCanvasManager>();
    }
    public void OnClickRegistered()
    {
        mainMenuCanvasManagerRef.ManageClickedButton(this.gameObject);
    }
}
