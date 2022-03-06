using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AsteroidManager))]

public class AsteroidSpawner : MonoBehaviour
{
    private AsteroidManager manager = null;

    [SerializeField] private Transform asteroidContainer = default(Transform);

    private List<GameObject> asteroidsOnScene = new List<GameObject>();

    [SerializeField] private float increaseDifficultTime = 0.0f;
    [SerializeField] private float timeToSpawn           = 0.0f;

    [SerializeField] private int asteroidsToSpawn    = 0;
    [SerializeField] private int maxAsteroidsOnScene = 0;
    [SerializeField] private int initialAsteroids    = 0;

    private float spawnTimer     = 0.0f;
    private float difficultTimer = 0.0f;

    private int asteroidCount = 0;

    private void Start()
    {
        SetManager();
    }

    private void Update()
    {
        IncreaseDifficulty();
        CheckAsteroidsOnScreen();
    }

    private void SetManager()
    {
        manager = GetComponent<AsteroidManager>();

        for (int i = 0; i <= initialAsteroids; i++)
        {
            manager.CreateAsteroid();
        }
    }

    private void IncreaseDifficulty()
    {
        difficultTimer += Time.deltaTime;

        if (difficultTimer > increaseDifficultTime)
        {
            asteroidsToSpawn++;
            difficultTimer = 0;
            GameManager.GameManagerInstance.UpdateCurrentStage();
        }
    }

    private void CheckAsteroidsOnScreen()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > timeToSpawn)
        {
            asteroidsOnScene.Clear();

            for (int i = 0; i < asteroidContainer.childCount; i++)
            {
                asteroidsOnScene.Add(asteroidContainer.GetChild(i).gameObject);
            }

            for (int i = 0; i < asteroidsOnScene.Count; i++)
            {
                if (asteroidsOnScene[i].gameObject.activeSelf)
                {
                    asteroidCount++;
                }
            }

            if (asteroidCount < maxAsteroidsOnScene)
            {
                for (int i = 0; i < asteroidsToSpawn; i++)
                {
                    manager.CreateAsteroid();
                }
            }

            spawnTimer    = 0;
            asteroidCount = 0;
        }
    }
}
