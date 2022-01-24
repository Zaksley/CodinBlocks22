using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

    public GameObject[] objets;


    GameObject[] objs;

    void Awake(){

        objs = GameObject.FindGameObjectsWithTag("_music");
        
        
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
 
        }
        DontDestroyOnLoad(this.gameObject);


/*
        foreach (var element in objs)
        {
            DontDestroyOnLoad(element);
        }

        foreach (var element in objets)
        {
            DontDestroyOnLoad(element);
        }
       */
    }

    public void RemoveFromDontDestroy()
    {
        /*
        foreach (var element in objets)
        {
            SceneManager.MoveGameObjectToScene(element,SceneManager.GetActiveScene());
        }
        */
        foreach (var element in objs)
        {
            SceneManager.MoveGameObjectToScene(element,SceneManager.GetActiveScene());
        }
    }
}
