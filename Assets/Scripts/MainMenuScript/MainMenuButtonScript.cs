using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonScript : MonoBehaviour
{

    private MainMenuCanvasManager mainMenuCanvasManagerRef;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject mainMenuCanvasReference = GameObject.FindGameObjectWithTag("MainMenuCanvas");

        mainMenuCanvasManagerRef = mainMenuCanvasReference.GetComponent<MainMenuCanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickRegistered()
    {
        Debug.Log("I was clicked");
        mainMenuCanvasManagerRef.DefineClickedButton(this.gameObject);
    }
}
