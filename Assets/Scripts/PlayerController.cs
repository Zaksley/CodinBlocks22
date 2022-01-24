using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using UnityEngine.SceneManagement; 
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
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


    // Tilemaps 
    public Tilemap Tilemap_NormalBlocks; 
    public Tilemap Tilemap_LightBlocks; 
    public Tilemap Tilemap_DarkBlocks; 

    public Tilemap Tilemap_LightSprites; 
    public Tilemap Tilemap_DarkSprites; 

    public TileBase DarkTile;
    public TileBase LightTile;

    // Sprites
    public Sprite darkSprite; 
    public Sprite lightSprite; 


    //Animation
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Lights
    public Light2D globalLight;
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

    // Musics
    private GameObject getGO;
    public AudioHandler audioHandler;

    // Start is called before the first frame update


    public static PlayerController instance;
    void Awake(){
        //objs = GameObject.FindGameObjectsWithTag("_music");

        if (instance != null)
        {
            Debug.LogWarning("Instance de Player inexistante");
            return;
        }

        instance = this;
    }
    void Start()
    {
        state = State.LIGHT; 
        globalLight.intensity = lightIntensity_White; 

        blue = new Color32( 0x3A, 0xB6, 0xD9, 0xFF ); 
        red = new Color32( 0xA7, 0x0A, 0x0A, 0xFF ); 

        // Tilemaps 
        Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = false; 
        Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = true; 
        Tilemap_LightSprites.GetComponent<TilemapRenderer>().enabled = false; 
        Tilemap_DarkSprites.GetComponent<TilemapRenderer>().enabled = true; 

        Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = true; 
        Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = false;

        //Tilemap_NormalBlocks = GetComponent<Tilemap>(); 


        
        BlueLights = Tilemap_LightBlocks.GetComponent<CreateLights_Tilemap>().Lights; 
        RedLights = Tilemap_DarkBlocks.GetComponent<CreateLights_Tilemap>().Lights; 
        SetIntensity_Light(BlueLights, 0);
        SetIntensity_Light(RedLights, RedIntensity);
        // get audio game obj
        getGO = GameObject.Find("MainSource");
        audioHandler = getGO.GetComponent<AudioHandler>();

        audioHandler.fadeneg();
    }


    void Update()
    {
        if (Input.GetButtonDown("Restart")) Restart(); 
        /*
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        Vector3 targetVelocity = new Vector2(horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        */

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        
        if (!pause)
        {
            Jump(); 
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f); 
            transform.position += movement * Time.deltaTime * moveSpeed; 
            if (Input.GetButtonDown("Switch") && isGrounded ) 
            {
                Switch();
                TranslatePlayer();  
            }

            flip(movement.x);

            float characterVelocity = Mathf.Abs(movement.x);
            animator.SetFloat("Speed", characterVelocity);
        }
        
        
        
    }

    public void stopMovement(){
        pause = true;
    }

    public void resumeMovement()
    {
        pause = false;
    }

    void Jump() 
    {
        if (Input.GetButtonDown("Jump") && isGrounded )
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpDirection * jumpSpeed), ForceMode2D.Impulse); 
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        // change music 
        audioHandler.fadeneg();
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

 
    public void TranslatePlayer() 
    {
        if (this.gameObject.GetComponent<SpriteRenderer>().flipY == false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = darkSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            jumpDirection = -1; 

            transform.position = new Vector3(transform.position.x, transform.position.y - switchHeight, transform.position.z); 
            groundCheck.position = new Vector3(groundCheck.position.x, groundCheck.position.y + 1.02f, groundCheck.position.z); 
            GameObject.Find("PersonnalLight").transform.position = new Vector3(transform.position.x + offsetX, transform.position.y - offsetY, transform.position.z);
        }
        else if (this.gameObject.GetComponent<SpriteRenderer>().flipY == true)
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
            Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = true; 
            Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = false; 
            Tilemap_LightSprites.GetComponent<TilemapRenderer>().enabled = true; 
            Tilemap_DarkSprites.GetComponent<TilemapRenderer>().enabled = false; 

            Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = false; 
            Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = true; 

            Tilemap_NormalBlocks.SwapTile(LightTile,DarkTile);

            animator.SetInteger("Dark", 1);

            // Lights
            globalLight.intensity = lightIntensity_Black; 
            personnalLight.color = red; 

            SetIntensity_Light(BlueLights, BlueIntensity);
            SetIntensity_Light(RedLights, 0);


            //Musics
            audioHandler.fadepos();
            audioHandler.sounddown();

        }

        else if (state == State.DARK) 
        {
            state = State.LIGHT; 

            // Show tilemaps
            Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = false; 
            Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = true; 
            Tilemap_LightSprites.GetComponent<TilemapRenderer>().enabled = false; 
            Tilemap_DarkSprites.GetComponent<TilemapRenderer>().enabled = true; 

            Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = true; 
            Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = false; 

            Tilemap_NormalBlocks.SwapTile(DarkTile, LightTile);
            animator.SetInteger("Dark", 0);

            // Lights
            globalLight.intensity = lightIntensity_White; 
            personnalLight.color = blue; 

            SetIntensity_Light(BlueLights, 0);
            SetIntensity_Light(RedLights, RedIntensity);

            //Musics
            audioHandler.fadeneg();
            audioHandler.soundup();
            
        }
    }

    private void SetIntensity_Light(List<Light2D> L, int intensity) 
    {
        for(int i=0; i<L.Count; i++)
        {
            L[i].intensity = intensity; 
        }
    }
}
