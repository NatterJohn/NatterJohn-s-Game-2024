using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class charactermovementscript : MonoBehaviour
{
    float snapPosition = 1;
    float timer, TimePerMove = 0.25f,TimePer90Rotate = 0.25f;
    enum characterStates { Waiting , Moving , Rotating }
    float max_width = 4, max_depth = 3;
    int charOrientation = 0;
    characterStates isCurrently = characterStates.Waiting;
    private Vector3 startLocation, desiredLocation;
    Quaternion startRotation, desiredRotation;
    private float multiplier;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(isCurrently)
        {
            case characterStates.Waiting:

                if (Input.GetKeyDown(KeyCode.W))
                {
                    setupMovement(transform.position + snapPosition * transform.forward);
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    setupMovement(transform.position + snapPosition * transform.right);
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    setupMovement(transform.position - snapPosition * transform.right);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    setupMovement(transform.position - snapPosition * transform.forward);
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    setupRotation(Vector3.forward);
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    setupRotation(Vector3.back);
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    setupRotation(Vector3.left);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    setupRotation(Vector3.right);
                }
                break;

                case characterStates.Moving:
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startLocation, desiredLocation, Mathf.Sin((Mathf.PI * timer /(TimePerMove*2))));
                if (timer > TimePerMove)
                {
                    isCurrently = characterStates.Waiting;
                    transform.position = desiredLocation;
                }
                break;

                case characterStates.Rotating:
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(startRotation, desiredRotation, timer/(multiplier * TimePer90Rotate));
                if (timer > multiplier *TimePer90Rotate)
                {
                    isCurrently = characterStates.Waiting;
                    transform.rotation = desiredRotation;
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
            desiredLocation = desiredDestination;
            

            isCurrently = characterStates.Moving;
        }
    }

    private void setupRotation(Vector3 direction)
    {
        if (Vector3.Dot(transform.forward, -direction) < 0.9f)
        {
            startRotation = transform.rotation;
            timer = 0;
            desiredRotation = Quaternion.LookRotation(-direction);
            if (Vector3.Dot(transform.forward, -direction) < -0.9f) multiplier = 2;
            else multiplier = 1;

            isCurrently = characterStates.Rotating;
        }

    }

    private bool isInsideArea(Vector3 desiredDestination)
    {
        return (desiredDestination.x >= 0) && (desiredDestination.z >= 0) && (desiredDestination.x <= max_width) && (desiredDestination.z <= max_depth);
    }
}



