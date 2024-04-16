using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeMeAppear : MonoBehaviour
{
    enum BLOCKSTATE { Small, growing, Big, Shrinking}
    BLOCKSTATE state = BLOCKSTATE.Small;

    private float scale = 0.01f;
    float timer, TIMEFORTRANSITION = 1;
    private void Start()
    {
        transform.localScale = scale *Vector3.one;
    }



    internal void Update()
    {
        switch(state)
        {
            case BLOCKSTATE.Small:
                break;

            case BLOCKSTATE.growing:
                timer += Time.deltaTime;
                scale = Mathf.Lerp(0f, 1f, timer / TIMEFORTRANSITION);
                transform.localScale = scale *Vector3.one;
                if (timer > TIMEFORTRANSITION)
                {
                    state = BLOCKSTATE.Big;
                    transform.localScale = Vector3.one;
                    scale = 1f;
                }
                break;

            case BLOCKSTATE.Big:

                break;

            case BLOCKSTATE.Shrinking:

                break;  

        }
    }

    internal void MakeAppear()
    {
        state = BLOCKSTATE.growing;
        timer = 0f;
    }
    private void OnEnable()
    {
        Observer.onLog.AddListener(MakeAppear);
    }
    private void OnDisable()
    {
        Observer.onLog.AddListener(MakeAppear);
    }
}
