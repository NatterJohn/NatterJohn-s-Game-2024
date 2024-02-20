using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovementScript : MonoBehaviour, ITractorBeamable
{
    public void beamMeUP(charactermovementscript charactermovementscript)
    {
      transform.position = (transform.position + charactermovementscript.transform.position)/2;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
