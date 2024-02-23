using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovementScript : MonoBehaviour, ITractorBeamable
{
    public void beamMeUP(charactermovementscript charactermovementscript)
    { 
        startingPosition = transform.position;
        destination = (transform.position + charactermovementscript.transform.position)/2;
        isNow = asteroidStates.Moving;
        timer = 0;
    }
    enum asteroidStates { Idle, Starting, Moving }
    asteroidStates isNow = asteroidStates.Idle;
    float timer, TimePerMove = 0.25f, TimePer90Rotate = 0.25f;
    private Vector3 destination, startingPosition;

    void Start()
    {
        
    }

    void Update()
    {
        switch (isNow)
        {
            case asteroidStates.Idle:

                break;

            case asteroidStates.Moving:
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startingPosition, destination, Mathf.Sin((Mathf.PI * timer / (TimePerMove * 2))));
                if (timer > TimePerMove)
                {
                    isNow = asteroidStates.Idle;
                }
                break;
        }
    }
}
