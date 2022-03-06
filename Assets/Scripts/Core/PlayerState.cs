using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private float playerScore = 0.0f;

    [SerializeField, Range(3, 10)] private int maxPlayerLife = 3;
    private int playerLife  = 3;

    public float PlayerScore
    {
        get
        {
            return playerScore;
        }
        set
        {
            if(value > 0)
            {
                playerScore += value;
            }
        }
    }

    public int PlayerLife
    {
        get
        {
            return playerLife;
        }
        set
        {
            if (value > 0)
            {
                playerLife -= value;
            }
        }
    }

    private void Awake()
    {
        playerLife = maxPlayerLife;
    }
}
