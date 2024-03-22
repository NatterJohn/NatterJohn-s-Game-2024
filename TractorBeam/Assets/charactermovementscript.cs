using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class charactermovementscript : MonoBehaviour
{
    float snapPosition = 1;
    float timer, TimePerMove = 0.25f,TimePer90Rotate = 0.25f;
    float completeTimer, textTime = 5f;
    enum characterStates { Waiting , Moving , Rotating ,TractorBeamPowerUp, TractorBeamPulling, Goal , Fail }
    float max_width = 4, max_depth = 3;
    int charOrientation = 0;
    characterStates isCurrently = characterStates.Waiting;
    
    private ITractorBeamable currentAsteroid;
    private Vector3 startLocation, desiredLocation, goalLocation;
    Quaternion startRotation, desiredRotation;
    Quaternion mustBe;
    private float multiplier;
    [SerializeField] timeLeft counter;

    GoalStarScript theGoal;
    textVisibility theText;
    buttonVisibilty theButton;
    beamVisibilty theBeam;
    gameOver failure;
    RestartButton restart;
    ReturnButton menuReturn;
    ReturnButtonAlt altReturn;
    AsteroidMovementScript theAsteroid;
    gotothegame nextScene;
    void Start()
    {
        
        theGoal = FindAnyObjectByType<GoalStarScript>();
        theText = FindObjectOfType<textVisibility>();
        theText.gameObject.SetActive(false);
        theButton = FindObjectOfType<buttonVisibilty>();
        theButton.gameObject.SetActive(false);
        theBeam = FindObjectOfType<beamVisibilty>();
        theBeam.gameObject.SetActive(false);
        failure = FindObjectOfType<gameOver>();
        failure.gameObject.SetActive(false);
        restart = FindObjectOfType<RestartButton>();
        menuReturn = FindObjectOfType<ReturnButton>();
        menuReturn.gameObject.SetActive(false);
        altReturn = FindObjectOfType<ReturnButtonAlt>();
        theAsteroid = FindAnyObjectByType<AsteroidMovementScript>();
        nextScene = FindObjectOfType<gotothegame>();
        nextScene.gameObject.SetActive(false);
        counter = FindObjectOfType<timeLeft>();
        mustBe = startRotation;
    }

    void Update()
    {
        switch(isCurrently)
        {
                case characterStates.Waiting:
                if (Input.GetKeyDown(KeyCode.W))
                {
                    setupMovement(transform.position + snapPosition * Vector3.forward);
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    setupMovement(transform.position + snapPosition * Vector3.right);
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    setupMovement(transform.position - snapPosition * Vector3.right);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    setupMovement(transform.position - snapPosition * Vector3.forward);
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

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    setupBeam();
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


                case characterStates.TractorBeamPowerUp:
                timer += Time.deltaTime;
                if (timer > 0.25f)
                {
                    isCurrently = characterStates.TractorBeamPulling;
                }


                break;

                case characterStates.TractorBeamPulling:
                timer += Time.deltaTime;

                if (timer > 0.25f)
                {
                    theBeam.gameObject.SetActive(false);
                    isCurrently = characterStates.Moving;
                }


                break;

                case characterStates.Goal:
                counter.gameObject.SetActive(false);
                restart.gameObject.SetActive(false);
                altReturn.gameObject.SetActive(false);
                theGoal.gameObject.SetActive(false);
                completeTimer += Time.deltaTime;
                theText.gameObject.SetActive(true);
                if (completeTimer > textTime)
                {
                    theText.gameObject.SetActive(false);
                    theButton.gameObject.SetActive(true);
                    menuReturn.gameObject.SetActive(true);
                    nextScene.gameObject.SetActive(true);
                }
                break;

                case characterStates.Fail:
                counter.gameObject.SetActive(false);
                restart.gameObject.SetActive(false);
                theGoal.gameObject.SetActive(false);
                altReturn.gameObject.SetActive(false);
                completeTimer += Time.deltaTime;
                failure.gameObject.SetActive(true);
                if (completeTimer > textTime)
                {
                    failure.gameObject.SetActive(false);
                    theButton.gameObject.SetActive(true);
                    menuReturn.gameObject.SetActive(true);
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
        
        if (!Physics.CheckSphere(desiredDestination, 0.45f))
        {
                startLocation = transform.position;
                timer = 0;
                desiredLocation = desiredDestination;
                isCurrently = characterStates.Moving;
        }


        


    }

    private void setupBeam()
    {
        ITractorBeamable asteroidToMove = getMyAsteroid();
        if (asteroidToMove != null)
        {
            timer = 0;
            timer += Time.deltaTime;
            if (timer < 0.5f)
            {
                theBeam.gameObject.SetActive(true);
            }
            asteroidToMove.beamMeUP(this);
            isCurrently = characterStates.TractorBeamPowerUp;
            currentAsteroid = asteroidToMove;
        }


    }

    private ITractorBeamable getMyAsteroid()
    {
    // check immediately in front (must be empty!!)

        if (Physics.CheckSphere(transform.position + transform.forward, 0.2f))
            {
            return null;
        }

        // check 2 spaces in front for an ItractorBeamable

        Collider[] objectsHit = Physics.OverlapSphere(transform.position + 2 * transform.forward, 0.2f);
        foreach (Collider collider in objectsHit)
        {
            ITractorBeamable newObject = collider.GetComponent<ITractorBeamable>();

            if ( newObject != null )
            {
                return newObject;
            }
        }

        return null;
    }

    private void setupRotation(Vector3 direction)
    {
        if (Vector3.Dot(transform.forward, direction) < 0.9f)
        {
            startRotation = transform.rotation;
            timer = 0;
            desiredRotation = Quaternion.LookRotation(direction);
            if (Vector3.Dot(transform.forward, direction) < -0.9f) multiplier = 2;
            else multiplier = 1;

            isCurrently = characterStates.Rotating;
        }

    }

    private bool isInsideArea(Vector3 desiredDestination)
    {
        return (desiredDestination.x >= 0) && (desiredDestination.z >= 0) && (desiredDestination.x <= max_width) && (desiredDestination.z <= max_depth);
    }
}



