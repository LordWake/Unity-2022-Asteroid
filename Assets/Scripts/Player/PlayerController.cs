using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void AxisInputHandler(float axisValue);
    public delegate void DefaultInputHandler();

    public AxisInputHandler OnVerticalInput   = delegate { };
    public AxisInputHandler OnHorizontalInput = delegate { };

    public DefaultInputHandler OnStopMovement = delegate { };
    public DefaultInputHandler OnAccelerate   = delegate { };
    public DefaultInputHandler OnDecelerate   = delegate { };
    public DefaultInputHandler OnStartShoot   = delegate { };

    [SerializeField] private string horizontalInputName = "";
    [SerializeField] private string verticalInputName   = "";
    [SerializeField] private string spaceInputName      = "";
    [SerializeField] private string shootInputName      = "";

    private bool enableInputs = true;

    public bool EnableInputs
    {
        set { enableInputs = value; }
    }

    private void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if(!enableInputs)
        {
            return;
        }

        float horizontalAxis = Input.GetAxis(horizontalInputName);
        float verticalAxis   = Input.GetAxis(verticalInputName);

        bool spaceKeyboardPressed  = Input.GetButtonDown(spaceInputName);
        bool spaceKeyboardReleased = Input.GetButtonUp(spaceInputName);
        bool shootInputPressed     = Input.GetButtonDown(shootInputName);

        /* Move */
        if(horizontalAxis != 0.0f)
        {
            OnHorizontalInput(horizontalAxis);
        }

        if(verticalAxis > 0.0f)
        {
            OnVerticalInput(verticalAxis);
        }
        else
        {
            OnStopMovement();
        }
        if(spaceKeyboardPressed)
        {
            OnAccelerate();
        }
        if(spaceKeyboardReleased)
        {
            OnDecelerate();
        }

        /* Shoot */
        if(shootInputPressed)
        {
            OnStartShoot();
        }
    }
}
