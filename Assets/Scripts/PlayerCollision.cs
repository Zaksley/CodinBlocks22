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
    public Sprite SwitchLight; 
    public Sprite SwitchDark; 
    

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

        audioHandler.soundpickup();
    }

    private void SwitchLaser(Tilemap laser, Collider2D collision)
    { 
        laser.GetComponent<TilemapRenderer>().enabled = false; 
        laser.GetComponent<TilemapCollider2D>().enabled = false; 

        collision.GetComponent<SpriteRenderer>().flipX = true; 
    }

    private void SwitchPlayer(PlayerController.State Color)
    {
        if (Color == PlayerController.State.LIGHT)
        {
            GameObject[] switches = GameObject.FindGameObjectsWithTag("SwitchPlayer");
            foreach (GameObject switchplayer in switches)
            {
                switchplayer.GetComponent<SpriteRenderer>().sprite = SwitchDark;
            } 
        }
        else if (Color == PlayerController.State.DARK)
        {

            GameObject[] switches = GameObject.FindGameObjectsWithTag("SwitchPlayer");
            foreach (GameObject switchplayer in switches)
            {
                switchplayer.GetComponent<SpriteRenderer>().sprite = SwitchLight;
            } 
        }

        this.GetComponent<PlayerController>().Switch(); 
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
                if (SceneManager.GetActiveScene().buildIndex != maxLevel){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    audioHandler.soundportal();
                } 
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
                if (SceneManager.GetActiveScene().buildIndex != maxLevel){
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
                    audioHandler.soundportal();
                }
                else
                {
                    // Go endgame 
                }
            }
        }

        else if (collision.gameObject.CompareTag("DeathCollider"))
        {
            this.GetComponent<PlayerController>().Restart();  
        }

        else if (collision.gameObject.CompareTag("SwitchBlue"))
        {
            SwitchLaser(LaserBlue, collision);
        }

        else if (collision.gameObject.CompareTag("SwitchRed"))
        {
            SwitchLaser(LaserRed, collision);
        }

        else if (collision.gameObject.CompareTag("SwitchPlayer"))
        {
            SwitchPlayer(this.GetComponent<PlayerController>().state); 
        }

        else if (collision.gameObject.CompareTag("LaserBlue"))
        {
            if (this.GetComponent<PlayerController>().state == PlayerController.State.DARK) {
                this.GetComponent<PlayerController>().Restart(); 
            }
        }

        else if (collision.gameObject.CompareTag("LaserRed")){
            if (this.GetComponent<PlayerController>().state == PlayerController.State.LIGHT) 
            {
                this.GetComponent<PlayerController>().Restart(); 
            }
        }
    }
}