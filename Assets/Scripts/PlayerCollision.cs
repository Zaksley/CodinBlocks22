using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using UnityEngine.SceneManagement; 
using UnityEngine.Experimental.Rendering.Universal;
public class PlayerCollision : MonoBehaviour
{
    public GameObject door; 
    private BoxCollider2D doorBox; 
    private Light2D doorLight; 
    private float intensity;
    public int maxLevel; 
    private PlayerController player; 
    public Animator doorAnimator;

    private GameObject getGO;
    public AudioHandler audioHandler;

    

    void Start(){
        doorBox = door.GetComponent<BoxCollider2D>(); 
        doorLight = door.GetComponent<Light2D>(); 

        player = this.GetComponent<PlayerController>(); 

        //Music 
        getGO = GameObject.Find("MainSource");
        audioHandler = getGO.GetComponent<AudioHandler>();
    }

    void Update() 
    {
    }

    private void TakeKey(Collider2D collision)
    {
        Destroy(collision.gameObject);  
        this.GetComponent<PlayerController>().gotKey = true; 
        doorBox.enabled = true; 
        door.GetComponent<NoCollisionDoor>().setIntensity(); 
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        if (collision.gameObject.CompareTag("FireKey"))
        {
            if (player.state == PlayerController.State.DARK)
            { 
                TakeKey(collision);
                doorAnimator.SetInteger("key", 1);
            }
        }

        else if (collision.gameObject.CompareTag("BlueKey"))
        {
            if (player.state == PlayerController.State.LIGHT) 
            {
                TakeKey(collision);
                doorAnimator.SetInteger("key", 1);
            }
        }

        else if (collision.gameObject.CompareTag("DoorBlue"))
        {
            if (this.GetComponent<PlayerController>().gotKey && this.GetComponent<PlayerController>().state == PlayerController.State.LIGHT)
            {
                if (SceneManager.GetActiveScene().buildIndex != maxLevel) 
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
                else
                {
                    // Go endgame 
                }
            }
        }

        else if (collision.gameObject.CompareTag("DoorRed"))
        {
            if (this.GetComponent<PlayerController>().gotKey && this.GetComponent<PlayerController>().state == PlayerController.State.DARK)
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
            audioHandler.fadeneg();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }
}