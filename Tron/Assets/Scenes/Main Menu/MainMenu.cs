using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll(); // Limpiar todos los PlayerPrefs al inicio
        // Check if the current scene is not the main menu
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Load the main menu scene
            SceneManager.LoadScene(0);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Controls()
    {
        // Establecer el indicador en PlayerPrefs antes de cargar la escena de controles
        PlayerPrefs.SetInt("Controls_Back", 0);
        SceneManager.LoadScene(2);
    }	
}