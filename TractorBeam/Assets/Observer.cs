using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Reflection;

public class Observer : MonoBehaviour
{
    public static UnityEvent onLog = new UnityEvent();

    List<MakeMeAppear> allBlocks;

    private void Start()
    {
        allBlocks = new List<MakeMeAppear>();
        GameObject[] allGameObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);   
        foreach (GameObject gameObject in allGameObjects)
        {
            MakeMeAppear gameObjectScript = gameObject.GetComponent<MakeMeAppear>();
            if (gameObjectScript != null)
            {
                allBlocks.Add(gameObjectScript);
            }
        }
        MakeAllBlocksAppear();

    }

    private void Update()
    {
       //MakeAllBlocksAppear();   
    }
    IEnumerable<MakeMeAppear> GetAll()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(MakeMeAppear)))
            .Select(type => Activator.CreateInstance(type) as MakeMeAppear);
    }
    private IEnumerator Log()
    {
        yield return new WaitForSeconds(2f);

       

        //Debug.Log("I am testing");
        //onLog.Invoke();
    }

    private void MakeAllBlocksAppear()
    {
        foreach (MakeMeAppear block in allBlocks)
            block.MakeAppear();
    }
}
