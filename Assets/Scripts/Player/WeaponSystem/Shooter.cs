using UnityEngine;

[RequireComponent(typeof(ShootBehaviour))]

public class Shooter : MonoBehaviour
{
    public ShootBehaviour shootBehaviour = null;

    private void Awake()
    {
        shootBehaviour = GetComponent<ShootBehaviour>();
    }

    public void Shoot()
    {
        shootBehaviour.Shoot();
    }
}
