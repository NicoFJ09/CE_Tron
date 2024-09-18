using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    public int MainMenuSceneIndex = 0;
    public int PauseSceneIndex = 3;
    public void Return_Scene()
    {

        int Controls_back = PlayerPrefs.GetInt("Controls_Back", 0);
        // Recuperar el nombre de la escena anterior y cargarla
        if (Controls_back == MainMenuSceneIndex)    {
            SceneManager.LoadScene(MainMenuSceneIndex);
        }
        else if (Controls_back == PauseSceneIndex)
        {
            SceneManager.LoadScene(PauseSceneIndex);
        }
    }
}