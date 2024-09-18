using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int MainMenuSceneIndex = 0;
    public int GamePlaySceneIndex = 1;
    public int ControlsSceneIndex = 2;
    void Start()
    {
        PlayerPrefs.DeleteAll(); // Limpiar todos los PlayerPrefs al inicio
        // Check if the current scene is not the main menu
        if (SceneManager.GetActiveScene().buildIndex != MainMenuSceneIndex)
        {
            // Load the main menu scene
            SceneManager.LoadScene(MainMenuSceneIndex);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(GamePlaySceneIndex);
    }

    public void Controls()
    {
        // Establecer el indicador en PlayerPrefs antes de cargar la escena de controles
        PlayerPrefs.SetInt("Controls_Back", MainMenuSceneIndex);
        SceneManager.LoadScene(ControlsSceneIndex);
    }	
}