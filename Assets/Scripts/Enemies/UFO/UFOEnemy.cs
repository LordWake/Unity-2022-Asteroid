using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UFOWeapons))]

public class UFOEnemy : MonoBehaviour
{
    public delegate void OnAlienDeath();
    public OnAlienDeath AlienDeath = delegate { };
    public UnityEvent DeadExplosion;

    private Animator animator = null;

    private UFOWeapons ufoWeapons = null;
    [HideInInspector] public UFOManager myManager = null;

    private SpriteRenderer spriteRenderer = null;
    private BoxCollider2D boxCollider     = null;

    [SerializeField] private float shootTimer = 0.0f;
    private float explosionTimer = 0.5f;

    private bool alienIsDead = false;

    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/
    public void EnterOnScene()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled    = true;
        alienIsDead            = false;

        animator.Play("Anim_AlienEnter");
        StopCoroutine(OnDeathUFO());
    }

    public void TakeDamage()
    {
        DeadExplosion.Invoke();
        StartCoroutine(OnDeathUFO());
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        animator       = GetComponent<Animator>();
        ufoWeapons     = GetComponent<UFOWeapons>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider    = GetComponent<BoxCollider2D>();

        AlienDeath += OnAlienIsDead;

        InvokeRepeating("ShootPlayer", shootTimer, shootTimer);
    }

    private void ShootPlayer()
    {
        GameObject playerRef = myManager.GetPlayerReference();
        bool playerIsDead    = playerRef.GetComponent<PlayerModelCharacter>().onPlayerDeath;

        if(alienIsDead || playerIsDead)
        {
            return;
        }

        ufoWeapons.Trigger(playerRef);
    }

    private void OnAlienIsDead()
    {
        animator.StopPlayback();
    }

    private IEnumerator OnDeathUFO()
    {
        alienIsDead            = true;
        spriteRenderer.enabled = false;
        boxCollider.enabled    = false;

        GameManager.GameManagerInstance.UpdateScore(GameManager.GameManagerInstance.UFOScore);
        yield return new WaitForSeconds(explosionTimer);

        AlienDeath();
    }
}
