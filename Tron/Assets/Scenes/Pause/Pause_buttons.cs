using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    public void Quit_pause()
    {
        SceneManager.LoadScene(0);
    }

    public void Controls_pause()

    {
        PlayerPrefs.SetInt("Controls_Back", 3);
        SceneManager.LoadScene(2);
    }

    public void Resume_pause()
    {
        SceneManager.LoadScene(1);
    }
}