using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gotothegame : MonoBehaviour
{
    keepPlaying myAudio;

    private void Start()
    {
        myAudio = FindObjectOfType<keepPlaying>();
    }
    public void GoGame()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 2) {
            myAudio.StopTheAudio();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 0)
            myAudio.StartTheAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

        
    }
}
