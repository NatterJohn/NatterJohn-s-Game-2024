using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class keepPlaying : MonoBehaviour
{
    public static keepPlaying instance;
    AudioSource myAudio;
    public void Awake()
    {
            myAudio = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); 
    }

    internal void StartTheAudio()
    {
        myAudio.Play();
    }

    internal void StopTheAudio()
    {
        myAudio.Stop();
    }
}
