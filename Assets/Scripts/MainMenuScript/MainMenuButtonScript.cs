using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonScript : MonoBehaviour
{

    private MainMenuCanvasManager mainMenuCanvasManagerRef;

    
    void Awake()
    {
        GameObject mainMenuCanvasReference = GameObject.FindGameObjectWithTag("MainMenuCanvas");

        mainMenuCanvasManagerRef = mainMenuCanvasReference.GetComponent<MainMenuCanvasManager>();
    }
    public void OnClickRegistered()
    {
        mainMenuCanvasManagerRef.ManageClickedButton(this.gameObject);
    }
}
