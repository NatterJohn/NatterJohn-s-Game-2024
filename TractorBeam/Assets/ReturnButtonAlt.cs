using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButtonAlt : MonoBehaviour
{

    public void MenuReturnAlt()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
