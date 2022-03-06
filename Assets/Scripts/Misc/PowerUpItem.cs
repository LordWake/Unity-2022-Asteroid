using UnityEngine;

public enum POWER_UP_TYPE
{
    INVULNERABILITY,
    PLUS_BULLET,
    ARC_SHOT,
    BOMB,
    WAVE,
    SIZE_T
};

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class PowerUpItem : MonoBehaviour
{
    public delegate void OnPowerUpPickedDelegate();
    public OnPowerUpPickedDelegate OnPowerUpPicked = delegate { };

    [SerializeField] private float lifeSpan = 10.0f;
    [SerializeField] private float speed    = 5.0f;

    [SerializeField] private Sprite[] powerUpSprites = new Sprite[5];

    private SpriteRenderer spriteRenderer = null;

    private POWER_UP_TYPE powerupType;
    private POWER_UP_TYPE lastPowerUpType = POWER_UP_TYPE.INVULNERABILITY;

    private Vector2 randomDirector = new Vector2();

    public void SpawnPowerUp()
    {
        powerupType = SetPowerUpType();
        SetPowerUpTexture();

        randomDirector = Random.insideUnitCircle.normalized;

        CancelInvoke("OnPowerUpDeath");
        Invoke("OnPowerUpDeath", lifeSpan);
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MovePowerUp();
    }

    private void MovePowerUp()
    {
        transform.position += new Vector3(randomDirector.x, randomDirector.y, 0.0f) * speed * Time.deltaTime;
    }

    private void OnPowerUpDeath()
    {
        OnPowerUpPicked();
    }

    private void SetPowerUpTexture()
    {
        spriteRenderer.sprite = powerUpSprites[(int)powerupType];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayersData.player)
        {
            collision.gameObject.GetComponent<PlayerModelCharacter>().TakePowerUp(powerupType);
            OnPowerUpDeath();
        }
    }

    private POWER_UP_TYPE SetPowerUpType()
    {
        POWER_UP_TYPE returnPowerUpType = (POWER_UP_TYPE)Random.Range(0, ((int)POWER_UP_TYPE.SIZE_T - 1));

        if(returnPowerUpType == lastPowerUpType)
        {
            return SetPowerUpType();
        }
        else
        {
            lastPowerUpType = returnPowerUpType;
            return returnPowerUpType;
        }
    }
}
