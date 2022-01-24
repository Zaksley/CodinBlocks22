
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyBis : MonoBehaviour
{

    public GameObject[] objets;

    //public static DontDestroy instance;

    void Awake(){

/*
        if (instance != null)
        {
            Debug.LogWarning("Instance de DontDestroy inexistante");
            return;
        }
        */
        
        foreach (var element in objets)
        {
            DontDestroyOnLoad(element);
        }
       
    }

/*
    public void RemoveFromDontDestroy()
    {
        foreach (var element in objets)
        {
            SceneManager.MoveGameObjectToScene(element,SceneManager.GetActiveScene());
        }

    }
    */
}
