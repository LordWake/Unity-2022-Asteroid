using UnityEngine;

public class ArcWeapon : DefaultWeapon
{
    [SerializeField] private Transform[] bulletSpawners = new Transform[3];

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

        for(int i = 0; i < bulletSpawners.Length; i++)
        {
            Bullet bullet     = pool.GetObject();
            bullet.BulletType = BULLET_TYPE.DEFAULT;

            bullet.Spawn(transform.position, bulletSpawners[i].transform.up);
            ResetCoolDown();

            onShoot.Invoke();
        }
    }
}
