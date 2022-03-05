using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProfileButtonScript : MonoBehaviour
{
    private NewProfileNameCanvas newProfileNameCanvas;
    void Start()
    {
        newProfileNameCanvas = GameObject.FindGameObjectWithTag("NewProfileNameCanvas").GetComponent<NewProfileNameCanvas>();
    }
    public void OnClickRegistered()
    {
        newProfileNameCanvas.ManageClickedButton(this.gameObject);
    }
}
