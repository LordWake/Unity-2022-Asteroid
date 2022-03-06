using UnityEngine;

[RequireComponent(typeof(PowerUpSpawner))]

public class UFOManager : MonoBehaviour, INotifier
{
    private IObserver myObserver = null;

    [SerializeField] private UFOEnemy ufoEnemyRef = null;
    [SerializeField] private PlayerManager playerManagerRef = null;

    private UFOEnemy alienOnGame = null;

    [SerializeField] private float spawnAlienTimer = 15.0f;
    private float spawnAlientCurrentTime           = 0.0f;

    private bool alienAlive = false;

    private Transform ufoSpawnPoint = null;

    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/

    public Vector3 GetAlienPosition()
    {
        return alienOnGame.transform.position;
    }

    public GameObject GetPlayerReference()
    {
        return playerManagerRef.PlayerGameObject;
    }

    public void SubscribeObserver(IObserver observer)
    {
        myObserver = observer;
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        ufoSpawnPoint = transform.GetChild(0).transform;
    }

    private void Update()
    {
        CheckAlienSpawn();
    }

    private void CheckAlienSpawn()
    {
        spawnAlientCurrentTime += Time.deltaTime;
        if (spawnAlientCurrentTime >= spawnAlienTimer)
        {
            if (!alienAlive)
            {
                SpawnAlien();
            }

            spawnAlientCurrentTime = 0.0f;
        }
    }

    private void SpawnAlien()
    {
        if(alienOnGame == null)
        {
            alienOnGame = Instantiate(ufoEnemyRef);
            alienOnGame.AlienDeath += OnAlienDeath;
            alienOnGame.myManager = this;
        }

        alienOnGame.gameObject.SetActive(true);
        alienOnGame.transform.position = ufoSpawnPoint.position;
        alienOnGame.EnterOnScene();
        alienAlive = true;
    }

    private void OnAlienDeath()
    {
        alienOnGame.gameObject.SetActive(false);
        spawnAlientCurrentTime = 0.0f;
        alienAlive = false;
        myObserver.OnNotify(NOTIFICATION_TYPE.SPAWN_POWER_UP);
    }
}