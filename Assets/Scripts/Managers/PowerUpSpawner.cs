using UnityEngine;

public class PowerUpSpawner : MonoBehaviour, IObserver
{
    [SerializeField] private PowerUpItem powerUp = null;

    private UFOManager ufoManager = null;

    private bool powerUpAlreadySpawned = false;

    public void OnNotify(NOTIFICATION_TYPE notificationType)
    {
        if(notificationType == NOTIFICATION_TYPE.SPAWN_POWER_UP)
        {
            SpawnPowerUp();
        }
    }

    private void Awake()
    {
        SubscribeToNotifier();
        powerUp.gameObject.SetActive(false);
    }

    private void SubscribeToNotifier()
    {
        ufoManager = GetComponent<UFOManager>();
        ufoManager.SubscribeObserver(this);
    }

    private void SpawnPowerUp()
    {
        powerUp.gameObject.SetActive(true);
        powerUp.transform.position = ufoManager.GetAlienPosition();
        powerUp.SpawnPowerUp();

        if(!powerUpAlreadySpawned)
        {
            powerUp.OnPowerUpPicked += PowerUpPicked;
        }
    }

    private void PowerUpPicked()
    {
        powerUp.gameObject.SetActive(false);
    }

}
