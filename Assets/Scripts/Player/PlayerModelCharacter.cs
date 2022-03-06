using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerWeapon))]

public class PlayerModelCharacter : MonoBehaviour
{
    private PlayerWeapon weaponsManager = null;
    private Rigidbody2D rigidBody       = null;

    [SerializeField] private GameObject shield = null;

    [Header("Movement Settings")]
    [SerializeField, Range(0.0f, 10.0f)]  private float force      = 0.0f;
    [SerializeField, Range(1.01f, 10.0f)] private float extraForce = 0.0f;
    [SerializeField, Range(0.0f, 10.0f)]  private float torque     = 0.0f;

    private float invulnerabilityTime = 10.0f;
    private float currentSpeed        = 1.0f;

    [Header("Movement Events")]
    public UnityEvent OnMovePlayer;
    public UnityEvent OnStopPlayer;

    private bool invulnerability = false;
    [HideInInspector] public bool onPlayerDeath    = false;

    public Rigidbody2D RigidBody
    {
        get { return rigidBody; }
    }

    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/

    #region Player Movement
    public void MovePlayerVertical(float axisInput)
    {
        rigidBody.AddForce(transform.up * force * axisInput * currentSpeed);
    }

    public void StopPlayer()
    {
        OnStopPlayer.Invoke();
    }

    public void MovePlayerHorizontal(float direction)
    {
        rigidBody.AddTorque(torque * -direction);
    }

    public void IncreaseSpeed()
    {
        OnMovePlayer.Invoke();
        currentSpeed = extraForce;
    }

    public void DecreaseSpeed()
    {
        currentSpeed = 1.0f;
    }
    #endregion

    #region Player Shot
    public void TriggerWeapon()
    {
        if (weaponsManager)
        {
            weaponsManager.Trigger();
        }
    }
    #endregion

    public void TakeDamage()
    {
        if (invulnerability || onPlayerDeath)
        {
            return;
        }

        onPlayerDeath = true;
        GameManager.GameManagerInstance.UpdatePlayerLife();
    }

    public void TakePowerUp(POWER_UP_TYPE powerupType)
    {
        switch (powerupType)
        {
            case POWER_UP_TYPE.INVULNERABILITY: StartCoroutine(InvulnerabilityPowerUp());                    break;
            case POWER_UP_TYPE.PLUS_BULLET:     weaponsManager.ChangeWeaponBehaviour(WEAPON_BEHAVIOUR.PLUS); break;
            case POWER_UP_TYPE.ARC_SHOT:        weaponsManager.ChangeWeaponBehaviour(WEAPON_BEHAVIOUR.ARC);  break;
            case POWER_UP_TYPE.BOMB:            weaponsManager.ChangeWeaponBehaviour(WEAPON_BEHAVIOUR.BOMB); break;
            case POWER_UP_TYPE.WAVE:            weaponsManager.ChangeWeaponBehaviour(WEAPON_BEHAVIOUR.WAVE); break;
        }
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        InitialSettings();
    }

    private void InitialSettings()
    {
        weaponsManager = GetComponent<PlayerWeapon>();
        rigidBody      = GetComponent<Rigidbody2D>();

        rigidBody.gravityScale = 0.0f;
        shield.SetActive(invulnerability);
    }

    private IEnumerator InvulnerabilityPowerUp()
    {
        invulnerability = true;
        shield.SetActive(invulnerability);

        yield return new WaitForSeconds(invulnerabilityTime);

        invulnerability = false;
        shield.SetActive(invulnerability);
    }
}
