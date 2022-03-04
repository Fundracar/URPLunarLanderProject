using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ShipController : MonoBehaviour
{

    #region Variables

    [Header("Game Manager & Other components of interest")]
    public GameManager gameManagerRef;
    public Rigidbody2D shipRigidbody2D { get; private set; }


    public enum CauseOfDeath { WentAway, Terrain, Speed }
    public CauseOfDeath causeOfDeath;

    [Header("Movement Variables")]
    public float UpThurstInputValue, LateralThrustInputValue, accelerationFactor, UpAccelerationDuration, LatAccelerationDuration, fuelValue;
    private Coroutine yThrustCoroutine, xThrustCoroutine;
    #endregion
    #region Init & Update
    void Awake()
    {
        shipRigidbody2D = GetComponent<Rigidbody2D>();
        gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        UpAccelerationDuration = 0.5f;
        LatAccelerationDuration = UpAccelerationDuration;
    }
    #endregion
    #region Input System Events
    public void OnZKeyPressed(InputAction.CallbackContext context)
    {
        if (shipRigidbody2D.gameObject.activeInHierarchy == true)
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
        if (shipRigidbody2D.gameObject.activeInHierarchy == true)
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
        //Ready key (Spacebar) can be pressed on level start (to launch the game) and on Lost/Won level (to retry/Go to next Level)
        if (context.performed == true)
        {
            switch (gameManagerRef.currentGamePhase)
            {
                case GameManager.GamePhase.GameWaitingToStart:
                    gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.GamePlaying);
                    break;

                case GameManager.GamePhase.GamePlaying:
                    this.GetComponent<EPropulsor>().PropellShip();
                    break;
                case GameManager.GamePhase.GameLost:
                    StartCoroutine(gameManagerRef.levelManagerRef.RestartLevel());
                    Debug.Log("I pressed fucking space");
                    break;
                case GameManager.GamePhase.GameWon:
                    gameManagerRef.levelManagerRef.LoadNextLevel();
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

        shipRigidbody2D.AddForce(new Vector2(xSpeed, ySpeed), ForceMode2D.Force);
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (gameManagerRef.currentGamePhase == GameManager.GamePhase.GamePlaying)
        {
            string coltag = col.gameObject.tag;
            switch (coltag)
            {
                case "Terrain":
                    Debug.Log("Terrain");
                    causeOfDeath = CauseOfDeath.Terrain;
                    gameManagerRef.SwitchOnGamePhase(GameManager.GamePhase.GameLost);
                    break;
                case "Plateform":
                    Debug.Log("I collided a plateform");
                    gameManagerRef.VerifyShipSpeedOnLanding();
                    break;

                default:
                    break;
            }
        }
    }

    #endregion
}