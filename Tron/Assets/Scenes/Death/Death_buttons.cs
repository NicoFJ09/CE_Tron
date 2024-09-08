using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{

    public void MainMenu_death()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart_death()
    {
        SceneManager.LoadScene(1);
    }
}