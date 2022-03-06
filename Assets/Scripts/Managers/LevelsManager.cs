using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    public void EnterMainLevel()
    {
        SceneManager.LoadScene(LevelsData.mainLevel);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(LevelsData.mainMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
