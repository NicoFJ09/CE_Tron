using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public int MainMenuSceneIndex = 0;
    public int GamePlaySceneIndex = 1;
    public void MainMenu()
    {
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(GamePlaySceneIndex);
    }
}