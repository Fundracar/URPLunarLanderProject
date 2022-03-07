using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonScript : MonoBehaviour
{
    public void OnClickRegistered()
    {
        MainMenuCanvasManager.mainMenuCanvasManager.ManageClickedButton(this.gameObject);
    }
}
