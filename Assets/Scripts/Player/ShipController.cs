using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShipController : MonoBehaviour
{

    #region Variables
    public Rigidbody2D shipRigidbody2D { get; private set; }
    public EPropulsor shipPropulsorComponent { get; private set; }

    public enum CauseOfDeath { None, WentAway, Terrain, Speed }
    public CauseOfDeath causeOfDeath = CauseOfDeath.None;
    public int plateformMultiplier = 1;

    [Header("Movement Variables")]
    public float UpThurstInputValue, LateralThrustInputValue, accelerationFactor, UpAccelerationDuration, LatAccelerationDuration, fuelValue;
    private Coroutine yThrustCoroutine, xThrustCoroutine;
    #endregion
    #region Init & Update
    void Awake()
    {
        shipRigidbody2D = GetComponent<Rigidbody2D>();
        shipPropulsorComponent = GetComponent<EPropulsor>();
        UpAccelerationDuration = 0.5f;
        LatAccelerationDuration = UpAccelerationDuration;
    }
    #endregion
    #region Input System Events
    public void OnZKeyPressed(InputAction.CallbackContext context)
    {
        if (shipRigidbody2D.gameObject.activeInHierarchy)
        {
            if (context.performed && fuelValue > 0)
            {
                yThrustCoroutine = StartCoroutine(ZThrustLerp(1));
                // On Z being pressed, the Coroutine starts with 1 as parameter (meaning the x input-linked variable will tend toward 1)
            }
            if (context.canceled)
            {
                StopCoroutine(yThrustCoroutine);  // On Z being released, the previous coroutine is stopped.
                yThrustCoroutine = StartCoroutine(ZThrustLerp(0));  // It is restarted with 0 as parameter for the opposite effect.         
            }
        }
    }
    public void OnQDKeysPressed(InputAction.CallbackContext context)
    {
        if (shipRigidbody2D.gameObject.activeInHierarchy)
        {
            if (context.performed && fuelValue > 0)
            {
                float currentValue = context.ReadValue<float>();
                if (currentValue != 0) xThrustCoroutine = StartCoroutine(LateralThrustLerp(currentValue));
            }
            if (context.canceled)
            {
                StopCoroutine(xThrustCoroutine);
                xThrustCoroutine = StartCoroutine(LateralThrustLerp(0));
            }
        }

    }
    public void OnReadyKeyPressed(InputAction.CallbackContext context)
    {
        //Ready key (Spacebar) can be pressed on level start (to launch the game) and on Lost/Won level (to retry/Go to next Level) and while playing to propell the ship upward.

        if (context.performed)
        {
            switch (GameManager.gameManager.currentGamePhase)
            {
                case GameManager.GamePhase.GameWaitingToStart:
                    GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.GamePlaying);
                    break;

                case GameManager.GamePhase.GamePlaying:
                    shipPropulsorComponent.PropellShip();
                    break;

                case GameManager.GamePhase.GameLost:
                    StartCoroutine(LevelManager.levelManager.RestartLevel());
                    break;

                case GameManager.GamePhase.GameWon:
                    StartCoroutine(LevelManager.levelManager.LoadNextLevel());
                    break;

                default:
                    break;
            }
        }
    }
    #endregion
    #region Input Value Coroutines
    /*
    #Both these coroutines could be merged into one. ()
    #They are used to lerp the values of the movement input create a "thrust delay" on pressed and released
    #Meaning : When you press Z, there is a delay before your ship gets at full thurst. When you release it,
    #it takes the same time for the value to get back to 0. */
    private IEnumerator ZThrustLerp(float _ValueToLerpTo)
    {
        float startMoveTime = Time.time;
        float targetTime = startMoveTime + UpAccelerationDuration;

        while (Time.time < targetTime)
        {
            float currentTime = Time.time - startMoveTime;
            float progress = currentTime / UpAccelerationDuration;
            UpThurstInputValue = Mathf.Lerp(UpThurstInputValue, _ValueToLerpTo, progress);
            yield return null;
        }
    }
    private IEnumerator LateralThrustLerp(float _TempA)
    {
        float startMoveTime = Time.time;
        float targetTime = startMoveTime + LatAccelerationDuration;

        while (Time.time < targetTime)
        {
            float currentTime = Time.time - startMoveTime;
            float progress = currentTime / LatAccelerationDuration;
            LateralThrustInputValue = Mathf.Lerp(LateralThrustInputValue, _TempA, progress);
            yield return null;
        }
    }
    public void AddForceToShip()
    {
        float xSpeed = LateralThrustInputValue * accelerationFactor;
        float ySpeed = UpThurstInputValue * accelerationFactor;

        shipRigidbody2D.AddRelativeForce(new Vector2(xSpeed, ySpeed), ForceMode2D.Force);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.relativeVelocity);

        if (GameManager.gameManager.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            string coltag = col.gameObject.tag;

            switch (coltag)
            {
                //We should derive these behaviors in the specific terrain objects in order to avoid the switch statement (which is overkill in this context)
                case "Terrain":
                    causeOfDeath = CauseOfDeath.Terrain;
                    GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.GameLost);
                    break;

                case "LevelBound":
                    causeOfDeath = CauseOfDeath.WentAway;
                    GameManager.gameManager.SwitchOnGamePhase(GameManager.GamePhase.GameLost);
                    break;
                case "Plateform":
                    plateformMultiplier = col.gameObject.GetComponent<Plateform>().scoreMultiplierValue;
                    GameManager.gameManager.VerifyShipSpeedOnLanding();
                    break;
                default:
                    break;
            }
        }
    }

    #endregion
}