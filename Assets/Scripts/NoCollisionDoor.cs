using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class NoCollisionDoor : MonoBehaviour
{
    private float intensity; 

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<BoxCollider2D>().enabled = false; 
        intensity = this.GetComponentInChildren<Light2D>().intensity; 
        this.GetComponentInChildren<Light2D>().intensity = 0; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setIntensity()
    {
        this.GetComponentInChildren<Light2D>().intensity = intensity; 
    }
}
