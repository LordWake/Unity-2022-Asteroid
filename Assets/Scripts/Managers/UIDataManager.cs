using UnityEngine;
using UnityEngine.UI;

public class UIDataManager : MonoBehaviour, IObserver
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text lifeText  = null;
    [SerializeField] private Text stageText = null;

    [SerializeField] private GameObject gameOverBG = null;

    [SerializeField] private string scoreInitialText = "Score: ";
    [SerializeField] private string lifeInitialText  = "Lifes: ";
    [SerializeField] private string stageInitialText = "Sage: ";

    [SerializeField] private GameManager gameManagerRef = null;

    public void OnNotify(NOTIFICATION_TYPE notificationType)
    {
        switch (notificationType)
        {
            case NOTIFICATION_TYPE.UPDATE_SCORE: SetScoreText();     break;
            case NOTIFICATION_TYPE.UPDATE_LIFE:  SetLifeText();      break;
            case NOTIFICATION_TYPE.UPDATE_STAGE: SetStageText();     break;
            case NOTIFICATION_TYPE.GAME_OVER:    SetGameOverImage(); break;
        }
    }

    private void Awake()
    {
        gameManagerRef.SubscribeObserver(this);
    }

    private void SetScoreText()
    {
        scoreText.text = scoreInitialText + GameManager.GameManagerInstance.GetPlayerScore();
    }

    private void SetLifeText()
    {
        lifeText.text = lifeInitialText + GameManager.GameManagerInstance.GetPlayerLife();
    }

    private void SetStageText()
    {
        stageText.text = stageInitialText + GameManager.GameManagerInstance.CurrentStage;
    }

    private void SetGameOverImage()
    {
        gameOverBG.SetActive(true);
    }
}
