using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class charactermovementscript : MonoBehaviour
{
    float snapPosition = 1;
    float timer, TimePerMove = 0.25f,TimePer90Rotate = 0.25f;
    float completeTimer, textTime = 5f;
    enum characterStates { Waiting , Moving , Rotating , Goal , Fail }
    float max_width = 4, max_depth = 3;
    int charOrientation = 0;
    characterStates isCurrently = characterStates.Waiting;
    private Vector3 startLocation, desiredLocation, goalLocation;
    Quaternion startRotation, desiredRotation;
    private float multiplier;
    [SerializeField] timeLeft counter;

    GoalStarScript theGoal;
    textVisibility theText;
    buttonVisibilty theButton;
    gameOver failure;
    RestartButton restart;
    void Start()
    {
        
        theGoal = FindAnyObjectByType<GoalStarScript>();
        theText = FindObjectOfType<textVisibility>();
        theText.gameObject.SetActive(false);
        theButton = FindObjectOfType<buttonVisibilty>();
        theButton.gameObject.SetActive(false);
        failure = FindObjectOfType<gameOver>();
        failure.gameObject.SetActive(false);
        restart = FindObjectOfType<RestartButton>();
    }

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

                if(counter.remainingTime == 0)
                {
                    isCurrently = characterStates.Fail;
                }

                break;

                case characterStates.Moving:
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(startLocation, desiredLocation, Mathf.Sin((Mathf.PI * timer /(TimePerMove*2))));
                if (timer > TimePerMove)
                {
                    isCurrently = characterStates.Waiting;
                    transform.position = desiredLocation;

                    if (hasReachedGoal())
                        isCurrently = characterStates.Goal;
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

                case characterStates.Goal:
                counter.gameObject.SetActive(false);
                //need to deactivate restart button
                completeTimer += Time.deltaTime;
                theText.gameObject.SetActive(true);
                if (completeTimer > textTime)
                {
                    theText.gameObject.SetActive(false);
                    theButton.gameObject.SetActive(true);
                }
                break;

                case characterStates.Fail:
                counter.gameObject.SetActive(false);
                //need to deactivate restart button
                completeTimer += Time.deltaTime;
                failure.gameObject.SetActive(true);
                if (completeTimer > textTime)
                {
                    failure.gameObject.SetActive(false);
                    theButton.gameObject.SetActive(true);
                }
                break;
        }
    }

    private bool hasReachedGoal()
    {
       return  Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(theGoal.transform.position.x, theGoal.transform.position.z)) < 0.2f;
    }

    private void setupMovement(Vector3 desiredDestination)
    {
        if (isInsideArea(desiredDestination))
        {
            if (!Physics.CheckSphere(desiredDestination, 0.45f))
            {
                startLocation = transform.position;
                timer = 0;
                desiredLocation = desiredDestination;


                isCurrently = characterStates.Moving;
            }


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



