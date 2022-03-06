using System;
using UnityEngine;
using UnityEngine.Events;

public enum ASTEROID_TYPE
{
    BIG, MEDIUM, TINY
};

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PositionChecker))]

public class Asteroid : MonoBehaviour
{
    [SerializeField] private ASTEROID_TYPE asteroidType = ASTEROID_TYPE.BIG;

    [SerializeField] private float asteroidSpeed = 0.0f;

    public AsteroidEvent OnDeath = null;
    public UnityEvent ScoreEvent;

    private PlayerModelCharacter playerRef = null;

    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/

    public void Spawn(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        transform.rotation = new Quaternion(direction.x, direction.y, 0.0f, 0.0f);
    }

    public void DestroyAsteroid()
    {
        if (asteroidType == ASTEROID_TYPE.TINY)
        {
            GameManager.GameManagerInstance.UpdateScore(GameManager.GameManagerInstance.AsteroidScore);
        }

        OnDeath.Invoke(this);
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Update()
    {
        MoveThisAsteroid();
    }

    private void MoveThisAsteroid()
    {
        transform.position += transform.up * asteroidSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayersData.player)
        {
            if (playerRef == null)
            {
                playerRef = collision.gameObject.GetComponent<PlayerModelCharacter>();
            }

            playerRef.TakeDamage();
            DestroyAsteroid();
        }
    }
}

[Serializable]
public class AsteroidEvent : UnityEvent<Asteroid>
{ }