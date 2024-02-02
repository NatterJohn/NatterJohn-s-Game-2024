using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timeLeft : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 10) 
            { 
                timerText.color = Color.red;
            }
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
        }
        int simpleTime = Mathf.FloorToInt(remainingTime % 1);
        timerText.text = string.Format("Time: " + simpleTime);
    }
}
