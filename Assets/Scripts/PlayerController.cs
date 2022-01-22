using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using UnityEngine.SceneManagement; 

public class PlayerController : MonoBehaviour
{
    public enum State {
        LIGHT,
        DARK,
        DEAD, 
    }

    
    public float switchHeight = 2f; 
    public State state; 

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
    public LayerMask collisionLayers;


    // Tilemaps 
    public Tilemap Tilemap_NormalBlocks; 
    public Tilemap Tilemap_LightBlocks; 
    public Tilemap Tilemap_DarkBlocks; 

    public TileBase DarkTile;
    public TileBase LightTile;

    // Sprites
    public Sprite darkSprite; 
    public Sprite lightSprite; 


    //Animation
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        state = State.LIGHT; 

        Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = false; 
        Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = true; 

        Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = true; 
        Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = false;

        //Tilemap_NormalBlocks = GetComponent<Tilemap>(); 
    }

    // Update is called once per frame



    void Update()
    {
        if (Input.GetButtonDown("Restart")) Restart(); 
        
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        Vector3 targetVelocity = new Vector2(horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);
        

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        Jump(); 
        /*
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f); 
        transform.position += movement * Time.deltaTime * moveSpeed; 
        */
        if (Input.GetButtonDown("Switch") && isGrounded ) 
        {
            Switch(); 
        }

        flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    void Jump() 
    {
        if (Input.GetButtonDown("Jump") && isGrounded )
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpDirection * jumpSpeed), ForceMode2D.Impulse); 
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
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

    void Switch() 
    {
        if (state == State.LIGHT)
        {
            state = State.DARK; 

            // Flip player 
            this.gameObject.GetComponent<SpriteRenderer>().sprite = darkSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            transform.position = new Vector3(transform.position.x, transform.position.y - switchHeight, transform.position.z); 
            jumpDirection = -1; 
            groundCheck.position = new Vector3(groundCheck.position.x, groundCheck.position.y + 1.02f, groundCheck.position.z); 
        
            // Show tilemaps
            Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = true; 
            Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = false; 

            Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = false; 
            Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = true; 

            Tilemap_NormalBlocks.SwapTile(LightTile,DarkTile);

            animator.SetInteger("Dark", 1);

        }

        else if (state == State.DARK) 
        {
            state = State.LIGHT; 

            // Flip player 
            this.gameObject.GetComponent<SpriteRenderer>().sprite = lightSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = false;
            transform.position = new Vector3(transform.position.x, transform.position.y + switchHeight, transform.position.z); 
            jumpDirection = 1; 
            groundCheck.position = new Vector3(groundCheck.position.x, groundCheck.position.y - 1.02f, groundCheck.position.z);

            // Show tilemaps
            Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = false; 
            Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = true; 

            Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = true; 
            Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = false; 

            Tilemap_NormalBlocks.SwapTile(DarkTile, LightTile);
            animator.SetInteger("Dark", 0);
        }

        // Flip gravity
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = - this.gameObject.GetComponent<Rigidbody2D>().gravityScale; 
    }


    private void OnCollisionEnter2D(Collision2D other) 
    {
        
    }

    

}
