using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactermovementscript : MonoBehaviour
{
    float snapPosition = 1;
    double timer, TimePerMove = 0.25;
    enum characterStates { Waiting , Moving, Rotating }
    float max_width = 3, max_depth = 3;
    characterStates isCurrently = characterStates.Waiting;
    private Vector3 startLocation, destination;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(isCurrently)
        {
            case characterStates.Waiting:

                if (Input.GetKeyDown(KeyCode.S))
                {
                    setupMovement(transform.position + snapPosition * transform.forward);
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    setupMovement(transform.position + snapPosition * transform.right);
             
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    setupMovement(transform.position - snapPosition * transform.right);
           
                }

                if (Input.GetKeyDown(KeyCode.W))
                {
                    setupMovement(transform.position - snapPosition * transform.forward);
                 
                }
                break;


                case characterStates.Moving:
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startLocation, destination, Mathf.Sin((float)(Mathf.PI * timer /(TimePerMove*2))));
                if (timer> TimePerMove)
                {
                    isCurrently = characterStates.Waiting;
                    transform.position = destination;
                }
                break;
        }
    }

    private void setupMovement(Vector3 desiredDestination)
    {

        if (!Physics.CheckSphere(desiredDestination, 0.45f) && isInsideArea(desiredDestination))
        {
            startLocation = transform.position;
            timer = 0;
            destination = desiredDestination;


            isCurrently = characterStates.Moving;
        }
    }

    private bool isInsideArea(Vector3 desiredDestination)
    {
        return (desiredDestination.x >= 0) && (desiredDestination.z >= 0) && (desiredDestination.x <= max_width) && (desiredDestination.z <= max_depth);
    }
}



