using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

    public GameObject[] objets;

    public static DontDestroy instance;
    public GameObject[] objs;

    void Awake(){

        objs = GameObject.FindGameObjectsWithTag("_music");

        if (instance != null)
        {
            Debug.LogWarning("Instance de DontDestroy inexistante");
            return;
        }
        
        instance = this;

        if (objs.Length > 1)
        {
            Destroy(objs[1]);
        }

        foreach (var element in objs)
        {
            DontDestroyOnLoad(element);
        }

        foreach (var element in objets)
        {
            DontDestroyOnLoad(element);
        }
       
    }

    public void RemoveFromDontDestroy()
    {
        foreach (var element in objets)
        {
            SceneManager.MoveGameObjectToScene(element,SceneManager.GetActiveScene());
        }
        foreach (var element in objs)
        {
            SceneManager.MoveGameObjectToScene(element,SceneManager.GetActiveScene());
        }
    }
}
