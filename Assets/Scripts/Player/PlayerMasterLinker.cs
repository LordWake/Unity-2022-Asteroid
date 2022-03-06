using UnityEngine;

public class PlayerMasterLinker : MonoBehaviour
{
    private PlayerController playerController   = null;
    private PlayerModelCharacter modelCharacter = null;

    private void Awake()
    {
        InitialSet();
    }

    private void InitialSet()
    {
        //Get Components.
        playerController = GetComponent<PlayerController>();
        modelCharacter   = GetComponent<PlayerModelCharacter>();

        //Link Inputs.
        playerController.OnVerticalInput   += modelCharacter.MovePlayerVertical;
        playerController.OnHorizontalInput += modelCharacter.MovePlayerHorizontal;
        playerController.OnStopMovement    += modelCharacter.StopPlayer;
        playerController.OnAccelerate      += modelCharacter.IncreaseSpeed;
        playerController.OnDecelerate      += modelCharacter.DecreaseSpeed;
        playerController.OnStartShoot      += modelCharacter.TriggerWeapon;
    }
}
