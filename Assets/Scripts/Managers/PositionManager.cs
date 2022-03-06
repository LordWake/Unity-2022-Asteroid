using UnityEngine;

public class PositionManager : MonoBehaviour
{
    private Transform[] screenLimits = new Transform[4];

    public Transform [] ScreenLimits
    {
        get { return screenLimits; }
    }

    private void Awake()
    {
        SetScreenLimits();
    }

    private void SetScreenLimits()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            screenLimits[i] = transform.GetChild(i);
        }
    }
}
