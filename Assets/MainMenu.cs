using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string gameScenePath = "Scenes/Game";

    public void quit()
    {
        //check if we are running in a standalone build
        #if UNITY_STANDALONE
            //quit the application
            Application.Quit();
        #elif UNITY_EDITOR
                //stop playing the scene
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void startGame()
    {
        SceneManager.LoadScene(gameScenePath);
    }
}
