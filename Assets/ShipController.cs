using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ShipController : MonoBehaviour
{

    #region Variables


    private Rigidbody2D shipRigidbody2D;



    [Header("Speed Variables")] // Those two values represent the X axis and Y axis input values. 
    private float UpThurstInputValue; // Lerps toward 1 while Z is being pressed. Lerps toward 0 when its released
    private float LateralThrustInputValue; //Lerps toward -1 or 1 while Q or D are being pressed. Lerps toward 0 when they are released.



    [Header("Coroutines")] //Variables used to store active coroutines to access them easily should it be needed.
    private Coroutine ZaccelerationCoroutine;
    private Coroutine LateralAccelerationCoroutine;




    #endregion
    void Awake()
    {
        shipRigidbody2D = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {

    }



    #region Input System Events
    public void OnZKeyPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ZaccelerationCoroutine = StartCoroutine(ZThrustLerp(1));

            Debug.Log("I am pressing Z");
        }

        if (context.canceled)
        {
            StopCoroutine(ZaccelerationCoroutine);

            ZaccelerationCoroutine = StartCoroutine(ZThrustLerp(0));


            Debug.Log("I stopped pressing Z");
        }
    }

    public void OnQDKeysPressed(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            float currentValue = context.ReadValue<float>();

            if (currentValue != 0)
            {
                LateralAccelerationCoroutine = StartCoroutine(LateralThrustLerp(currentValue));
            }
        }
        if (context.canceled)
        {
            StopCoroutine(LateralAccelerationCoroutine);
            LateralAccelerationCoroutine = StartCoroutine(LateralThrustLerp(0));
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



}