using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{

    public GameObject[] objs;

    public static DontDestroy instance;
    void Awake(){
        //objs = GameObject.FindGameObjectsWithTag("_music");

        if (instance != null)
        {
            Debug.LogWarning("Instance de DontDestroy inexistante");
            return;
        }

        instance = this;

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        foreach (var element in objs)
        {
            DontDestroyOnLoad(element);
        }
       
    }

    public void RemoveFromDontDestroy()
    {
        foreach (var element in objs)
        {
            SceneManager.MoveGameObjectToScene(element,SceneManager.GetActiveScene());
        }
    }
}
