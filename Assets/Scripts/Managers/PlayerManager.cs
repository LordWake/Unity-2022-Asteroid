using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerGameObject = null;

    public GameObject PlayerGameObject
    {
        get { return playerGameObject; }
    }

    public void RespawnPlayer(bool gameIsOver)
    {
        StartCoroutine(RespawnPlayerVFX(gameIsOver));
    }

    private void Awake()
    {
        OnBeginGameSession();
    }

    private void OnBeginGameSession()
    {
        if (playerGameObject != null)
        {
            playerGameObject = Instantiate(playerGameObject, new Vector2(0.0f, 0.0f), Quaternion.identity);
        }
        else
        {
            throw new System.Exception("PlayerManager.cs - OnBeginGameSession() : playerGameObject variable is null.");
        }
    }

    private void HandlePlayerInputs(bool enableInputs)
    {
        PlayerController controller = playerGameObject.GetComponent<PlayerController>();
        controller.EnableInputs     = enableInputs;
    }

    private IEnumerator RespawnPlayerVFX(bool gameIsOver)
    {
        SpriteRenderer playerSprite      = playerGameObject.GetComponent<SpriteRenderer>();
        PlayerModelCharacter playerModel = playerGameObject.GetComponent<PlayerModelCharacter>();

        int repeatDeathVFX = 10;
        HandlePlayerInputs(false);

        for(int i = 0; i < repeatDeathVFX; i++)
        {
            playerSprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            playerSprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        if(!gameIsOver)
        {
            playerModel.onPlayerDeath = false;
            Vector3 vectorZero = new Vector3(0.0f, 0.0f, 0.0f);

            playerModel.RigidBody.velocity         = vectorZero;
            playerGameObject.transform.position    = vectorZero;
            playerGameObject.transform.eulerAngles = vectorZero;

            playerModel.TakePowerUp(POWER_UP_TYPE.INVULNERABILITY);
            HandlePlayerInputs(true);
        }
    }
}
