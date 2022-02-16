using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ShipController : MonoBehaviour
{

    #region Variables
    private Rigidbody2D shipRigidbody2D; //The body on which we applied the calculated speed and force.

    [Header("Input-Linked Variables")]

    // The two following values represent the X axis and Y axis input values. They will be refered as the "Input Linked Variables".
    private float UpThurstInputValue; // Lerps toward 1 while Z is being pressed. Lerps toward 0 when its released
    private float LateralThrustInputValue; //Lerps toward -1 or 1 while Q or D are being pressed. Lerps toward 0 when they are released.
    public float accelerationFactor = 2.5f; //Leave at 1 to cancel effect.

    [Header("Coroutines")]

    //Variables used to store active coroutines to access them easily should it be needed.
    private Coroutine yThrustCoroutine;
    private Coroutine xThrustCoroutine;

    #endregion


    #region Init & Update
    void Awake()
    {
        shipRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ManageShipSpeed();
    }

    #endregion

    #region Input System Events
    public void OnZKeyPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            yThrustCoroutine = StartCoroutine(ZThrustLerp(1));

            // On Z being pressed, the Coroutine starts with 1 as parameter (meaning the x input-linked variable will tend toward 1)
        }

        if (context.canceled)
        {
            StopCoroutine(yThrustCoroutine);  // On Z being pressed, the Coroutine starts.

            yThrustCoroutine = StartCoroutine(ZThrustLerp(0));  // It is restarted with 0 as parameter.            
        }
    }

    public void OnQDKeysPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float currentValue = context.ReadValue<float>();

            if (currentValue != 0)
            {
                xThrustCoroutine = StartCoroutine(LateralThrustLerp(currentValue));
            }
        }

        if (context.canceled)
        {
            StopCoroutine(xThrustCoroutine);

            xThrustCoroutine = StartCoroutine(LateralThrustLerp(0));

        }



    }


    #endregion


    #region Input Value Coroutines
    private IEnumerator ZThrustLerp(float _TempA)
    {
        float startMoveTime = Time.time;

        float duration = 2.0f;

        float targetTime = startMoveTime + duration;

        while (Time.time < targetTime)
        {
            float currentTime = Time.time - startMoveTime;
            float progress = currentTime / duration;

            UpThurstInputValue = Mathf.Lerp(UpThurstInputValue, _TempA, progress);

            yield return null;
        }

    }

    private IEnumerator LateralThrustLerp(float _TempA)

    {
        float startMoveTime = Time.time;

        float duration = 2.0f;

        float targetTime = startMoveTime + duration;

        while (Time.time < targetTime)
        {
            float currentTime = Time.time - startMoveTime;

            float progress = currentTime / duration;

            LateralThrustInputValue = Mathf.Lerp(LateralThrustInputValue, _TempA, progress);

            Debug.Log(LateralThrustInputValue);

            yield return null;
        }

    }

    //Both these coroutines could be merged into one. ()

    //They are used to lerp the values of the movement input create a "thrust delay" on pressed and released
    //Meaning : When you press Z, there is a delay before your ship gets at full thurst. When you release it,
    //it takes the same time for the value to get back to 0.
    #endregion



    #region Speed and Position Methods
    private bool IsShipWithinBounds()
    {
        if (this.transform.position.x > -30 && this.transform.position.x < 30 && !(this.transform.position.y > 20))
        {
            return true;
        }

        else
        {
            Debug.Log("Game Over ! Ship is outside of reach");
            return false;
        }


    }
    private void ManageShipSpeed()
    {
        if (IsShipWithinBounds() == true) UpdateShipSpeed();
    }
    private void UpdateShipSpeed()
    {

        float xSpeed = LateralThrustInputValue * accelerationFactor;

        float ySpeed = UpThurstInputValue * accelerationFactor;

        shipRigidbody2D.AddForce(new Vector2(xSpeed, ySpeed));

    }

    #endregion

}