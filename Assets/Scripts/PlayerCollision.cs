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

    //Lasers
    public Tilemap LaserBlue;
    public Tilemap LaserRed; 
    private BoxCollider2D LaserRedBox; 
    private BoxCollider2D LaserBlueBox; 

    

    void Start(){
        doorBox = door.GetComponent<BoxCollider2D>(); 
        doorLight = door.GetComponent<Light2D>(); 
        LaserRedBox = LaserRed.GetComponent<BoxCollider2D>();
        LaserBlueBox = LaserRed.GetComponent<BoxCollider2D>();

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

    private void SwitchLaserBlue(Collider2D collision)
    { 
        LaserBlueBox.enabled = false; 
    }

    private void SwitchLaserRed(Collider2D collision)
    { 
        LaserRedBox.enabled = false; 
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
            Respawn(); 
        }

        else if (collision.gameObject.CompareTag("SwitchBlue"))
        {
            SwitchLaserBlue(collision);
        }

        else if (collision.gameObject.CompareTag("SwitchRed"))
        {
            SwitchLaserRed(collision);
        }

        else if (collision.gameObject.CompareTag("LaserBlue"))
        {
            if (this.GetComponent<PlayerController>().state == PlayerController.State.LIGHT) {
                Respawn(); 
            }
        }

        else if (collision.gameObject.CompareTag("LaserRed")){
            if (this.GetComponent<PlayerController>().state == PlayerController.State.DARK) 
            {
                Respawn(); 
            }
        }
    }


    private void Respawn()
    {
        audioHandler.fadeneg();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}