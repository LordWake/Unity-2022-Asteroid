using UnityEngine;

public enum BULLET_TYPE
{
  DEFAULT, WAVE, UFO
};

[RequireComponent(typeof(BoxCollider2D))]

public class Bullet : MonoBehaviour
{
    public delegate void BulletHandler(Bullet thisBullet);
    private delegate void BulletMovement();

    [SerializeField] protected float bulletSpeed = 15.0f;
    [SerializeField] private float deathTime     = 2.5f;

    [Header("Wave Movement Settings")]
    [SerializeField] private float waveFrequency = 0.0f;
    [SerializeField] private float waveHeight    = 0.0f;

    public event BulletHandler OnDeath  = delegate { };
    private event BulletMovement OnMove = delegate { };

    [SerializeField] private BULLET_TYPE bulletType = BULLET_TYPE.DEFAULT;

    private bool canDoDamage = false;

    private PlayerModelCharacter playerRef = null;
    private UFOEnemy ufoEnemy = null;

    public BULLET_TYPE BulletType
    {
        set { bulletType = value; }
        get { return bulletType;  }
    }

    public void Spawn(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        transform.up       = direction;

        canDoDamage = true;
        Invoke("ForceDeath", deathTime);
    }

    private void Start()
    {
        SwitchBulletType();
    }

    private void Update()
    {
        OnMove();
    }

    private void SwitchBulletType()
    {
        switch (bulletType)
        {
            case BULLET_TYPE.DEFAULT: OnMove = MoveThisBullet; break;
            case BULLET_TYPE.WAVE:    OnMove = MoveWaveBullet; break; //Wave momevement
            case BULLET_TYPE.UFO:     OnMove = MoveThisBullet; break;
        }
    }

    private void MoveThisBullet()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    private void MoveWaveBullet()
    {
        Vector3 SineWaveMovement = transform.right * Mathf.Sin(Time.time * waveFrequency) * waveHeight;
        transform.position      += (SineWaveMovement + transform.up) * bulletSpeed * Time.deltaTime;
    }

    private void ForceDeath()
    {
        OnDeath(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDoDamage)
        {
            return;
        }

        /*************************************************/
        /*************** Player's Bullet *****************/
        /*************************************************/
        if (bulletType != BULLET_TYPE.UFO)
        {
            //Collision checks.-
            if (collision.gameObject.layer == LayersData.asteroid)
            {
                canDoDamage = false;
                collision.gameObject.GetComponent<Asteroid>().DestroyAsteroid();
                OnDeath(this);
            }
            else if(collision.gameObject.layer == LayersData.ufoEnemy)
            {
                canDoDamage = false;

                if (ufoEnemy == null)
                {
                    ufoEnemy = collision.gameObject.GetComponent<UFOEnemy>();
                }

                ufoEnemy.TakeDamage();
                OnDeath(this);
            }
        }

        /*************************************************/
        /***************** UFO's Bullet ******************/
        /*************************************************/
        else
        {
            if ((collision.gameObject.layer == LayersData.player))
            {
                canDoDamage = false;

                if(playerRef == null)
                {
                    playerRef = collision.gameObject.GetComponent<PlayerModelCharacter>();
                }

                playerRef.TakeDamage();
                OnDeath(this);
            }
        }

    }
}
