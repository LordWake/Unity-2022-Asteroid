using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, INotifier
{
    private static GameManager gameManagerInstance = null;

    private PlayerState playerState     = null;
    private LevelsManager levelsManager = null;

    [SerializeField] private PlayerManager playerManager = null;

    [SerializeField] private float ufoScore      = 10.0f;
    [SerializeField] private float asteroidScore = 5.0f;
    [SerializeField] private float stageScore    = 20.0f;

    private List <IObserver> observers = new List<IObserver>();

    private int currentStage = 0;

    #region Setters/Getters
    public static GameManager GameManagerInstance
    {
        get { return gameManagerInstance; }
    }

    public float UFOScore
    {
        get { return ufoScore; }
    }

    public float AsteroidScore
    {
        get { return asteroidScore; }
    }

    public int CurrentStage
    {
        get { return currentStage; }
    }
    #endregion

    /*****************************/
    /***** PUBLIC METHODS ********/
    /*****************************/

    public void UpdateScore(float scoreToAdd)
    {
        playerState.PlayerScore = scoreToAdd;

        foreach (IObserver observer in observers)
        {
            observer.OnNotify(NOTIFICATION_TYPE.UPDATE_SCORE);
        }
    }

    public float GetPlayerScore()
    {
        return playerState.PlayerScore;
    }

    public void UpdatePlayerLife()
    {
        playerState.PlayerLife = 1;

        foreach (IObserver observer in observers)
        {
            observer.OnNotify(NOTIFICATION_TYPE.UPDATE_LIFE);
        }

        if (playerState.PlayerLife <= 0)
        {
            playerManager.RespawnPlayer(true);
            OnGameOver();
        }
        else
        {
            playerManager.RespawnPlayer(false);
        }
    }

    public float GetPlayerLife()
    {
        return playerState.PlayerLife;
    }

    public void UpdateCurrentStage()
    {
        currentStage++;
        UpdateScore(stageScore);

        foreach (IObserver observer in observers)
        {
            observer.OnNotify(NOTIFICATION_TYPE.UPDATE_STAGE);
        }
    }

    public void SubscribeObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    /* Called from UI Button */
    public void ResetLevel()
    {
        levelsManager.EnterMainLevel();
    }

    /* Called from UI Button */
    public void BackToMainMenu()
    {
        levelsManager.GoToMainMenu();
    }

    /*****************************/
    /****** PRIVATE METHODS ******/
    /*****************************/

    private void Awake()
    {
        SingletonInstance();
        SetComponents();
    }

    private void Start()
    {
        SetInitialUI();
    }

    private void SetComponents()
    {
        playerState   = GetComponent<PlayerState>();
        levelsManager = GetComponent<LevelsManager>();
    }

    private void SingletonInstance()
    {
        if ((gameManagerInstance != null) && (gameManagerInstance != this))
        {
            Destroy(this.gameObject);
        }
        else
        {
            gameManagerInstance = this;
        }
    }

    private void SetInitialUI()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(NOTIFICATION_TYPE.UPDATE_SCORE);
            observer.OnNotify(NOTIFICATION_TYPE.UPDATE_LIFE);
            observer.OnNotify(NOTIFICATION_TYPE.UPDATE_STAGE);
        }
    }

    private void OnGameOver()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(NOTIFICATION_TYPE.GAME_OVER);
        }
    }
}
