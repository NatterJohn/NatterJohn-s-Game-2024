using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timeLeft : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] public float remainingTime;
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 6) 
            { 
                timerText.color = Color.red;
            }
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        timerText.text = string.Format("Time: " + (int)remainingTime);
    }
}
