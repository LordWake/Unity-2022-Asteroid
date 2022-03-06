using UnityEngine;

public class BombWeapon : DefaultWeapon
{
    [Header("Bomb Data")]
    private Pool<BombBullet> bombPool = null;

    [SerializeField] private BombBullet bombPrefab = null;

    private void Awake()
    {
        bulletsContainer = FindObjectOfType<PoolContainerManager>();
        bombPool         = new Pool<BombBullet>(initialBulletCount, GetBombBullet, OnReleaseBomb, OnGetBomb);
    }

    public override void Shoot()
    {
        if(!canShot)
        {
            return;
        }

        BombBullet bomb = bombPool.GetObject();
        bomb.SpawnBomb(transform.position, transform.up);
        ResetCoolDown();
        onShoot.Invoke();
    }

    #region Pool Methods
    protected void OnReleaseBomb(BombBullet bombBullet)
    {
        bombBullet.gameObject.SetActive(false);
        bombBullet.OnDeath -= bombPool.ReleaseObject;
    }

    protected void OnGetBomb(BombBullet bombBullet)
    {
        bombBullet.gameObject.SetActive(true);
        bombBullet.OnDeath += bombPool.ReleaseObject;
    }

    protected BombBullet GetBombBullet()
    {
        BombBullet bombBullet = Instantiate(bombPrefab);
        bulletsContainer.SaveObjectInContainer(bombBullet.gameObject);

        bombBullet.gameObject.SetActive(false);

        return bombBullet;
    }
    #endregion
}
