using System.Collections;
using UnityEngine;

public class PlusBulletWeapon : DefaultWeapon
{
    private int plusBullet = 2;

    [SerializeField] private float timeBetweenShots = 0.5f;

    private void Awake()
    {
        bulletsContainer = FindObjectOfType<PoolContainerManager>();
        pool             = new Pool<Bullet>(initialBulletCount, GetBullet, OnReleaseBullet, OnGetBullet);
    }

    public override void Shoot()
    {
        StartCoroutine(ShootTwice());
    }

    private IEnumerator ShootTwice()
    {
        for(int i = 0; i < plusBullet; i++)
        {
            base.Shoot();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
