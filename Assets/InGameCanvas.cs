using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCanvas : MonoBehaviour
{

    public GameObject playerReference;
    public Rigidbody2D playerRigidbodyReference;
    private GameObject textVerticalSpeedContainer;
    private Text verticalSpeedText;
    private GameObject textHorizontalSpeedContainer;

    private Text horizontalSpeedText;
    // Start is called before the first frame update
    void Start()
    {
    
        textVerticalSpeedContainer = GameObject.FindGameObjectWithTag("VerticalSpeedTextBox");
        verticalSpeedText = textVerticalSpeedContainer.GetComponent<Text>();

        textHorizontalSpeedContainer = GameObject.FindGameObjectWithTag("HorizontalSpeedTextBox");
        horizontalSpeedText = textHorizontalSpeedContainer.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerReference != null)
        {
            verticalSpeedText.text = playerRigidbodyReference.velocity.x.ToString();
            horizontalSpeedText.text = playerRigidbodyReference.velocity.y.ToString();
        }

    }
    public void FindPlayerInScene()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerRigidbodyReference = playerReference.GetComponent<Rigidbody2D>();
    }
}
