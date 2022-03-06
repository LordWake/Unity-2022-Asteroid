using UnityEngine;

[RequireComponent(typeof(DefaultWeapon))]

public class UFOWeapons : MonoBehaviour
{
    [SerializeField] private Shooter shooter = null;

    private DefaultWeapon defaultWeapon = null;

    private void Start()
    {
        defaultWeapon = GetComponent<DefaultWeapon>();
        shooter.shootBehaviour = defaultWeapon;
    }

    public void Trigger(GameObject target)
    {
        Quaternion savePosition = transform.localRotation;
        Vector3 aimDirection    = target.transform.position - transform.position;

        transform.up = aimDirection;
        shooter.Shoot();

        transform.localRotation = savePosition;
    }
}
