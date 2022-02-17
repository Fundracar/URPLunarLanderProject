using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InGameCanvas : MonoBehaviour
{

#region  Variables

    [Header ("Player Object & Components References")]
    private GameObject playerReference;
    private Rigidbody2D playerRigidbodyReference;

    [Header("Text Objects & Components")]
    private GameObject textVerticalSpeedContainer;
    private TextMeshProUGUI verticalSpeedText;
    private GameObject textHorizontalSpeedContainer;
    private TextMeshProUGUI horizontalSpeedText;

    #endregion
    void Start()
    {
        //Script Initialization : Every valuable object/information is looked for by tag and referenced.

        textVerticalSpeedContainer = GameObject.FindGameObjectWithTag("VerticalSpeedTextBox");

        verticalSpeedText = textVerticalSpeedContainer.GetComponent<TextMeshProUGUI>();

        textHorizontalSpeedContainer = GameObject.FindGameObjectWithTag("HorizontalSpeedTextBox");

        horizontalSpeedText = textHorizontalSpeedContainer.GetComponent<TextMeshProUGUI>();

    }

    void FixedUpdate()
    {
        /* Should this be done on Update or FixedUpdate ?
        I figured that since this script is supposed to track velocity values that are highly physics based, 
        it would be more accurate to track them 'OnFixedUpdate' */

        if (playerReference != null)
        {
            verticalSpeedText.text = (playerRigidbodyReference.velocity.y * 100f).ToString();

            horizontalSpeedText.text = (playerRigidbodyReference.velocity.x * 100f).ToString();
        }
    }
    public void FindPlayerInScene()

    {
        /*Since the player doesn't spawn right away on scene loading, 
   this methods encapsulates the behaviors that 'finds and references the player object in the scene' 
   in order to "fire" it at the right time.

   This prevents the script from trying to find an element that hasn't been spawned yet. */

        playerReference = GameObject.FindGameObjectWithTag("Player");

        playerRigidbodyReference = playerReference.GetComponent<Rigidbody2D>();
    }
}
