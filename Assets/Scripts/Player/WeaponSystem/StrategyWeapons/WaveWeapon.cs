public class WaveWeapon : DefaultWeapon
{
    private void Awake()
    {
        bulletsContainer = FindObjectOfType<PoolContainerManager>();
        pool             = new Pool<Bullet>(initialBulletCount, GetBullet, OnReleaseBullet, OnGetBullet);
    }

    public override void Shoot()
    {
        if (!canShot)
        {
            return;
        }

        Bullet bullet     = pool.GetObject();
        bullet.BulletType = BULLET_TYPE.WAVE;

        bullet.Spawn(transform.position, transform.up);

        ResetCoolDown();
        onShoot.Invoke();
    }
}
