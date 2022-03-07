using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProfileButtonScript : MonoBehaviour
{
    public void OnClickRegistered()
    {
        NewProfileNameCanvas.newProfileNameCanvas.ManageClickedButton(this.gameObject);
    }
}
