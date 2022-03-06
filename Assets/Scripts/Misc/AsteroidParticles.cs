using UnityEngine;

public class AsteroidParticles : MonoBehaviour
{
    public delegate void ParticleHandler(AsteroidParticles thisParticle);
    public event ParticleHandler onDeath = delegate { };

    [SerializeField] private float deathTime = 2.5f;

    public void Spawn(Vector2 spawnHere)
    {
        transform.position = spawnHere;
        Invoke("LifeSpan", deathTime);
    }

    private void LifeSpan()
    {
        onDeath(this);
    }
}
