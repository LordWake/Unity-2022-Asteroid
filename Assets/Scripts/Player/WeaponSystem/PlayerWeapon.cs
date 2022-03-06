using System.Collections;
using UnityEngine;

public enum WEAPON_BEHAVIOUR
{
    DEFAULT, PLUS, ARC, BOMB, WAVE
};

[RequireComponent(typeof(DefaultWeapon))]
[RequireComponent(typeof(PlusBulletWeapon))]
[RequireComponent(typeof(ArcWeapon))]
[RequireComponent(typeof(BombWeapon))]
[RequireComponent(typeof(WaveWeapon))]

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Shooter shooter = null;
    [SerializeField] private WEAPON_BEHAVIOUR initialWeapon = WEAPON_BEHAVIOUR.DEFAULT;

    private DefaultWeapon defaultWeapon = null;
    private PlusBulletWeapon plusWeapon = null;
    private ArcWeapon arcWeapon         = null;
    private BombWeapon bombWeapon       = null;
    private WaveWeapon waveWeapon       = null;

    [SerializeField] private float powerupTimer = 10.0f;

    private ShootBehaviour lastSavedWeapon = null;

    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/

    public void Trigger()
    {
        shooter.Shoot();
    }

    public void ChangeWeaponBehaviour(WEAPON_BEHAVIOUR newBehaviour)
    {
        switch (newBehaviour)
        {
            case WEAPON_BEHAVIOUR.DEFAULT: shooter.shootBehaviour = defaultWeapon; break;
            case WEAPON_BEHAVIOUR.PLUS:    shooter.shootBehaviour = plusWeapon;    break;
            case WEAPON_BEHAVIOUR.ARC:     shooter.shootBehaviour = arcWeapon;     break;

            /* PowerUps with timer */
            case WEAPON_BEHAVIOUR.BOMB:
                lastSavedWeapon        = shooter.shootBehaviour;
                shooter.shootBehaviour = bombWeapon;

                StartCoroutine(PowerUpTimer());
                break;
            case WEAPON_BEHAVIOUR.WAVE:
                lastSavedWeapon        = shooter.shootBehaviour;
                shooter.shootBehaviour = waveWeapon;

                StartCoroutine(PowerUpTimer());
                break;
        }
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Start()
    {
        defaultWeapon = GetComponent<DefaultWeapon>();
        plusWeapon    = GetComponent<PlusBulletWeapon>();
        arcWeapon     = GetComponent<ArcWeapon>();
        bombWeapon    = GetComponent<BombWeapon>();
        waveWeapon    = GetComponent<WaveWeapon>();

        ChangeWeaponBehaviour(initialWeapon);
    }

    private IEnumerator PowerUpTimer()
    {
        yield return new WaitForSeconds(powerupTimer);
        shooter.shootBehaviour = lastSavedWeapon;
    }
}
