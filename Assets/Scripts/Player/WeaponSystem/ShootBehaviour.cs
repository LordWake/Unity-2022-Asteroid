using UnityEngine;

public abstract class ShootBehaviour : MonoBehaviour
{
    [SerializeField] protected Bullet bulletPrefab = null;

    protected PoolContainerManager bulletsContainer = null;

    protected Pool<Bullet> pool = null;

    [SerializeField] protected float coolDown = 0.0f;
    [SerializeField] protected int initialBulletCount = 10;

    public abstract void Shoot();

    #region Pool Methods
    protected void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.OnDeath -= pool.ReleaseObject;
    }

    protected void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.OnDeath += pool.ReleaseObject;
    }

    protected Bullet GetBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bulletsContainer.SaveObjectInContainer(bullet.gameObject);

        bullet.gameObject.SetActive(false);
        return bullet;
    }
    #endregion
}
