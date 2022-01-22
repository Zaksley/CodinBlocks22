using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using UnityEngine.SceneManagement; 
public class PlayerCollision : MonoBehaviour
{
    public GameObject door; 
    private BoxCollider2D doorBox; 
    public int maxLevel; 
    private PlayerController player; 

    void Start(){
        doorBox = door.GetComponent<BoxCollider2D>(); 
        player = this.GetComponent<PlayerController>(); 
    }

    void Update() 
    {
    }

    private void TakeKey(Collider2D collision)
    {
        Destroy(collision.gameObject);  
        this.GetComponent<PlayerController>().gotKey = true; 
        doorBox.enabled = true; 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("FireKey"))
        {
            if (player.state == PlayerController.State.DARK) 
                TakeKey(collision);
        }

        else if (collision.gameObject.CompareTag("BlueKey"))
        {
            if (player.state == PlayerController.State.LIGHT) 
                TakeKey(collision);
        }

        else if (collision.gameObject.CompareTag("Door"))
        {
            if (this.GetComponent<PlayerController>().gotKey)
            {
                if (SceneManager.GetActiveScene().buildIndex != maxLevel) 
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
                else
                {
                    // Go endgame 
                }
            }
        }

        else if (collision.gameObject.CompareTag("DeathCollider"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }

    }

}