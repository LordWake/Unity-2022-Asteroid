using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]

public class BombBullet : MonoBehaviour
{
    public delegate void BombHandler(BombBullet thisBullet);
    public event BombHandler OnDeath = delegate { };

    public UnityEvent DeadExplosion;

    [SerializeField] private float bombSpeed = 1.0f;
    [SerializeField] private float radius    = 1.0f;

    private SpriteRenderer spriteRenderer = null;
    private BoxCollider2D boxCollider     = null;

    private float explosionTimer = 0.5f;

    public void SpawnBomb(Vector2 position, Vector2 direction)
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled    = true;

        transform.position = position;
        transform.up       = direction;

        StopCoroutine(OnDeathBomb());
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider    = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        MoveThisBullet();
    }

    private void MoveThisBullet()
    {
        transform.position += transform.up * bombSpeed * Time.deltaTime;
    }

    private void BombExplosion()
    {
        Collider2D[] explosionRadius = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach(Collider2D target in explosionRadius)
        {
            if(target.gameObject.layer == LayersData.asteroid)
            {
                target.gameObject.GetComponent<Asteroid>().DestroyAsteroid();
                continue;
            }
            else if(target.gameObject.layer == LayersData.ufoEnemy)
            {
                target.gameObject.GetComponent<UFOEnemy>().TakeDamage();
                continue;
            }
        }

        DeadExplosion.Invoke();
        StartCoroutine(OnDeathBomb());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( (collision.gameObject.layer == LayersData.ufoEnemy) || (collision.gameObject.layer == LayersData.asteroid) )
        {
            BombExplosion();
        }
    }

    private IEnumerator OnDeathBomb()
    {
        spriteRenderer.enabled = false;
        boxCollider.enabled    = false;

        yield return new WaitForSeconds(explosionTimer);
        OnDeath(this);
    }
}
