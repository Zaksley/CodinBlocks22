using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 

public class PlayerCollision : MonoBehaviour
{
    void Start(){

    }

    void Update() {
        
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("Key"))
        {
            Destroy(collision.gameObject);  
            this.GetComponent<PlayerController>().gotKey = true; 
        }

        else if (collision.gameObject.CompareTag("Door"))
        {
            if (this.GetComponent<PlayerController>().gotKey)
            {
                //Destroy(collision.gameObject);  
            }
        }
    }

}