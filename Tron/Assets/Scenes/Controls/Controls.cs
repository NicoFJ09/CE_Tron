using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{

    public void OnButtonClick()
    {

        int Controls_back = PlayerPrefs.GetInt("Controls_Back", 0);
        // Recuperar el nombre de la escena anterior y cargarla
        if (Controls_back == 0)    {
            SceneManager.LoadScene(0);
        }
        else if (Controls_back == 3)
        {
            SceneManager.LoadScene(3);
        }
    }
}