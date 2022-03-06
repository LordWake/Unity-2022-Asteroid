using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [Header("Type of Asteroids")]
    [SerializeField] private Asteroid bigAsteroidRef  = null;
    [SerializeField] private Asteroid midAsteroidRef  = null;
    [SerializeField] private Asteroid tinyAsteroidRef = null;

    private Transform[] spawnPositions  = new Transform[4];
    private Transform currentSpawnPlace = default(Transform);
    [SerializeField] private Transform asteroidContainer = default(Transform);

    [SerializeField] private AsteroidEvent OnAsteroidSpawned = null;

    private Pool<Asteroid> bigAsteroid  = null;
    private Pool<Asteroid> midAsteroid  = null;
    private Pool<Asteroid> tinyAsteroid = null;

    [SerializeField] private int initialAmount     = 10;
    [SerializeField] private int midAsteroidSpawn  = 2;
    [SerializeField] private int tinyAsteroidSpawn = 2;


    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/
    public void CreateAsteroid()
    {
        Asteroid asteroid = bigAsteroid.GetObject();
        SpawnAsteroidHere();
        asteroid.Spawn(currentSpawnPlace.position, currentSpawnPlace.transform.up);
        asteroid.transform.eulerAngles = new Vector3(0, 0, asteroid.transform.eulerAngles.z + Random.Range(-50, 50));
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        SetSpawnersPoints();
        SetPools();
    }

    #region Pool Settings
    //---------------------------------
    //---------BIG ASTEROID -----------
    //---------------------------------

    private void SetPools()
    {
        bigAsteroid  = new Pool<Asteroid>(initialAmount, GetBigAsteroid,  OnReleaseBigAsteroid,  OnGetBigAsteroid);
        midAsteroid  = new Pool<Asteroid>(initialAmount, GetMidAsteroid,  OnReleaseMidAsteroid,  OnGetMidAsteroid);
        tinyAsteroid = new Pool<Asteroid>(initialAmount, GetTinyAsteroid, OnReleaseTinyAsteroid, OnGetTinyAsteroid);
    }

    private Asteroid GetBigAsteroid()
    {
        Asteroid asteroid = Instantiate(bigAsteroidRef);
        asteroid.transform.parent = asteroidContainer;
        asteroid.gameObject.SetActive(false);

        return asteroid;
    }

    private void OnReleaseBigAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
        asteroid.OnDeath.RemoveListener(bigAsteroid.ReleaseObject);
    }

    private void OnGetBigAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        asteroid.OnDeath.AddListener(bigAsteroid.ReleaseObject);
        asteroid.OnDeath.AddListener(SpawnMidAsteroid);

        OnAsteroidSpawned.Invoke(asteroid);
    }

    //---------------------------------
    //---------MID ASTEROID -----------
    //---------------------------------

    private Asteroid GetMidAsteroid()
    {
        Asteroid asteroid = Instantiate(midAsteroidRef);
        asteroid.transform.parent = asteroidContainer;
        asteroid.gameObject.SetActive(false);
        return asteroid;
    }

    private void OnReleaseMidAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
        asteroid.OnDeath.RemoveListener(midAsteroid.ReleaseObject);
    }

    private void OnGetMidAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        asteroid.OnDeath.AddListener(midAsteroid.ReleaseObject);
        asteroid.OnDeath.AddListener(SpawnTinyAsteroid);

        OnAsteroidSpawned.Invoke(asteroid);
    }

    //---------------------------------
    //---------TINY ASTEROID ----------
    //---------------------------------
    private Asteroid GetTinyAsteroid()
    {
        Asteroid asteroid = Instantiate(tinyAsteroidRef);
        asteroid.transform.parent = asteroidContainer;
        asteroid.gameObject.SetActive(false);
        return asteroid;
    }

    private void OnReleaseTinyAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
        asteroid.OnDeath.RemoveListener(midAsteroid.ReleaseObject);
    }

    private void OnGetTinyAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        asteroid.OnDeath.AddListener(midAsteroid.ReleaseObject);
        OnAsteroidSpawned.Invoke(asteroid);
    }
    #endregion

    private void SetSpawnersPoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPositions[i] = transform.GetChild(i);
        }
    }

    private void SpawnMidAsteroid(Asteroid spawnInThisPosition)
    {
        for (int i = 0; i < midAsteroidSpawn; i++)
        {
            Asteroid asteroidRef = midAsteroid.GetObject();
            asteroidRef.Spawn(spawnInThisPosition.transform.position, new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)));
        }
    }

    private void SpawnTinyAsteroid(Asteroid spawnInThisPosition)
    {
        for (int i = 0; i < tinyAsteroidSpawn; i++)
        {
            Asteroid asteroidRef = tinyAsteroid.GetObject();
            asteroidRef.Spawn(spawnInThisPosition.transform.position, new Vector2(Random.Range(-180, 180), Random.Range(-180, 180)));
        }
    }

    private void SpawnAsteroidHere()
    {
        int random = Random.Range(0, 4);
        currentSpawnPlace = spawnPositions[random];
    }
}
