using UnityEngine.Events;

public class DefaultWeapon : ShootBehaviour
{
    protected bool canShot = true;

    public UnityEvent onShoot;
    public UnityEvent onCoolDown;

    public BULLET_TYPE thisBulletType = BULLET_TYPE.DEFAULT;

    private void Awake()
    {
        bulletsContainer = FindObjectOfType<PoolContainerManager>();
        pool             = new Pool<Bullet>(initialBulletCount, GetBullet, OnReleaseBullet, OnGetBullet);
    }

    public override void Shoot()
    {
        if(!canShot)
        {
            return;
        }

        Bullet bullet     = pool.GetObject();
        bullet.BulletType = thisBulletType;

        bullet.Spawn(transform.position, transform.up);
        ResetCoolDown();

        onShoot.Invoke();
    }

    protected void CoolDown()
    {
        canShot = true;
    }

    protected void ResetCoolDown()
    {
        canShot = false;
        Invoke("CoolDown", coolDown);
    }
}
