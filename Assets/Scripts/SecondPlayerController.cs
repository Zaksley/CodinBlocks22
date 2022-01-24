using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using UnityEngine.SceneManagement; 
using UnityEngine.Experimental.Rendering.Universal;

public class SecondPlayerController : MonoBehaviour
{
    public enum State {
        LIGHT,
        DARK,
        DEAD, 
    }

    
    public float switchHeight = 2f; 
    public State state; 

     public bool pause = false;

    // Move
    public float moveSpeed = 5f; 
    public bool gotKey = false; 
    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    //jump
    public float jumpSpeed = 12f; 
    public int jumpDirection = 1; 
    public bool isGrounded = false; 
    public Transform groundCheck;
    public float groundCheckRadius;
    private float groundOffsetY = 1.02f; 
    public LayerMask collisionLayers;



    // Sprites
    public Sprite darkSprite; 
    public Sprite lightSprite; 


    //Animation
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Lights
    public Light2D personnalLight; 
    private Color red; 
    private Color blue; 
    private float offsetY = 0.219f; 
    private float offsetX = 0.048f;
    [SerializeField] private float lightIntensity_White = 0.7f; 
    [SerializeField] private float lightIntensity_Black = 0.2f; 
    public List<Light2D> BlueLights; 
    public List<Light2D> RedLights; 

    private int BlueIntensity = 1;
    private int RedIntensity = 1; 

    public GameObject player; 
    public float offSetPlayerX = 9f; 
    public float offSetPlayerY = 1.82f; 


    public static PlayerController instance;

    void Start()
    {
        state = State.DARK; 
        transform.position = player.transform.position + new Vector3(offSetPlayerX, -offSetPlayerY, 0); 

                // Show tilemaps
        animator.SetInteger("Dark", 1);

        // Lights
        personnalLight.color = red; 
    }


    void Update()
    {
        if (Input.GetButtonDown("Switch") && isGrounded ) 
        {
            Switch();
            TranslatePlayer();  
        }
        Jump();

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f); 
        transform.position -= movement * Time.deltaTime * moveSpeed; 

        if (Input.GetButtonDown("Switch") && isGrounded ) 
        {
            Switch();
            TranslatePlayer();  
        }

        flip(-movement.x);

        float characterVelocity = Mathf.Abs(movement.x);
        animator.SetFloat("Speed", characterVelocity);
    }


    void flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Jump() 
    {
        if (Input.GetButtonDown("Jump") && isGrounded )
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpDirection * jumpSpeed), ForceMode2D.Impulse); 
        }
    }

    public void TranslatePlayer() 
    {
        if (this.gameObject.GetComponent<SpriteRenderer>().flipY == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = darkSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            jumpDirection = -1; 

            transform.position = new Vector3(transform.position.x, transform.position.y - switchHeight, transform.position.z); 
            groundCheck.position = new Vector3(groundCheck.position.x, groundCheck.position.y + 1.02f, groundCheck.position.z); 
            GameObject.Find("PersonnalLight").transform.position = new Vector3(transform.position.x + offsetX, transform.position.y - offsetY, transform.position.z);
        }
        else if (this.gameObject.GetComponent<SpriteRenderer>().flipY == false)
        {   
            this.gameObject.GetComponent<SpriteRenderer>().sprite = lightSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = false;
            jumpDirection = 1; 

            transform.position = new Vector3(transform.position.x, transform.position.y + switchHeight, transform.position.z); 
            groundCheck.position = new Vector3(groundCheck.position.x, groundCheck.position.y - 1.02f, groundCheck.position.z);
            GameObject.Find("PersonnalLight").transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);
        }

                // Flip gravity
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = - this.gameObject.GetComponent<Rigidbody2D>().gravityScale; 
    }

    public void Switch() 
    {
        if (state == State.LIGHT)
        {
            state = State.DARK; 
        
            // Show tilemaps
            animator.SetInteger("Dark", 1);

            // Lights
            personnalLight.color = red; 

        }

        else if (state == State.DARK) 
        {
            state = State.LIGHT; 

            animator.SetInteger("Dark", 0);

            // Lights
            personnalLight.color = blue; 
        }
    }
}
