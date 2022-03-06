public enum NOTIFICATION_TYPE
{
    UPDATE_SCORE, UPDATE_LIFE, UPDATE_STAGE, SPAWN_POWER_UP, GAME_OVER
};

public interface IObserver
{
    void OnNotify(NOTIFICATION_TYPE notificationType);
}
