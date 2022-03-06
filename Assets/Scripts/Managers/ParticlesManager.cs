using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [SerializeField] private AsteroidParticles asteroidParticles = null;

    [SerializeField] private PoolContainerManager asteroidParticleContainer = null;

    [SerializeField] private int initialCount = 20;

    private Pool<AsteroidParticles> pool = null;

    private void Awake()
    {
        pool = new Pool<AsteroidParticles>(initialCount, GetParticle, OnReleaseParticle, OnGetParticle);
    }

    public void SpawnParticle(Asteroid myAsteroid)
    {
        AsteroidParticles myParticle = pool.GetObject();
        myParticle.Spawn(myAsteroid.transform.position);
    }

    public void AddParticle(Asteroid asteroid)
    {
        asteroid.OnDeath.AddListener(SpawnParticle);
    }

    #region Pool Methods
    private void OnReleaseParticle(AsteroidParticles particle)
    {
        particle.gameObject.SetActive(false);
        particle.onDeath -= pool.ReleaseObject;
    }

    private void OnGetParticle(AsteroidParticles particle)
    {
        particle.gameObject.SetActive(true);
        particle.onDeath += pool.ReleaseObject;
    }

    private AsteroidParticles GetParticle()
    {
        AsteroidParticles particle = Instantiate(asteroidParticles);
        asteroidParticleContainer.SaveObjectInContainer(particle.gameObject);
        particle.gameObject.SetActive(false);
        return particle;
    }
    #endregion
}
