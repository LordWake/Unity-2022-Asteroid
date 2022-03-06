using UnityEngine;

public enum TYPE_OF_LIMIT
{
    MAX_X, MIN_X, MAX_Y, MIN_Y
};

public class PositionChecker : MonoBehaviour
{
    private PositionManager positionManager = null;
    private Transform[] screenLimitsRef     = new Transform[4];

    private void Start()
    {
        InitialSettings();
    }

    private void Update()
    {
        CheckPositionLimit();
    }

    private void InitialSettings()
    {
        positionManager = FindObjectOfType<PositionManager>();

        for(int i = 0; i < positionManager.ScreenLimits.Length; i++)
        {
            screenLimitsRef[i] = positionManager.ScreenLimits[i];
        }
    }

    private void CheckPositionLimit()
    {
        if(positionManager == null)
        {
            Debug.LogError("PositionChecker.cs - CheckPositionLimit() : positionManager variable is null.");
            return;
        }

        bool maxRightPosition = transform.position.x > screenLimitsRef[(int)TYPE_OF_LIMIT.MAX_X].position.x;
        bool minRightPosition = transform.position.x < screenLimitsRef[(int)TYPE_OF_LIMIT.MIN_X].position.x;
        bool maxUpPosition    = transform.position.y > screenLimitsRef[(int)TYPE_OF_LIMIT.MAX_Y].position.y;
        bool minUpPosition    = transform.position.y < screenLimitsRef[(int)TYPE_OF_LIMIT.MIN_Y].position.y;

        if (maxRightPosition)
        {
            transform.position = new Vector3(screenLimitsRef[ReturnNewPosition(TYPE_OF_LIMIT.MAX_X)].position.x,
                                              transform.position.y, transform.position.z);
        }
        if (minRightPosition)
        {
            transform.position = new Vector3(screenLimitsRef[ReturnNewPosition(TYPE_OF_LIMIT.MIN_X)].position.x,
                                             transform.position.y, transform.position.z);
        }
        if (maxUpPosition)
        {
            transform.position = new Vector3(transform.position.x, screenLimitsRef[ReturnNewPosition(TYPE_OF_LIMIT.MAX_Y)].position.y,
                                             transform.position.z);
        }
        if (minUpPosition)
        {
            transform.position = new Vector3(transform.position.x, screenLimitsRef[ReturnNewPosition(TYPE_OF_LIMIT.MIN_Y)].position.y,
                                             transform.position.z);
        }
    }

    private int ReturnNewPosition(TYPE_OF_LIMIT currentLimit)
    {
        TYPE_OF_LIMIT returnLimit = TYPE_OF_LIMIT.MAX_X;

        switch (currentLimit)
        {
            case TYPE_OF_LIMIT.MAX_X: returnLimit = TYPE_OF_LIMIT.MIN_X; break;
            case TYPE_OF_LIMIT.MIN_X: returnLimit = TYPE_OF_LIMIT.MAX_X; break;
            case TYPE_OF_LIMIT.MAX_Y: returnLimit = TYPE_OF_LIMIT.MIN_Y; break;
            case TYPE_OF_LIMIT.MIN_Y: returnLimit = TYPE_OF_LIMIT.MAX_Y; break;
        }

        return (int)returnLimit;
    }

}
