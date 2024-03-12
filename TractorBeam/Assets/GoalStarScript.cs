using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalStarScript : MonoBehaviour
{
    private float rotationSpeed = 90;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        
    }
}
