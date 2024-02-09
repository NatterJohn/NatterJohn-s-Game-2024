using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void StarttheGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void EndtheGame()
    {
        Application.Quit();
    }
}
