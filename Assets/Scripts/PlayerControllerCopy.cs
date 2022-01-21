using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps; 
using UnityEngine;

public class PlayerControllerCopy : MonoBehaviour
{
    public enum State {
        LIGHT,
        DARK,
        DEAD, 
    }

    public float moveSpeed = 5f; 
    public float jumpSpeed = 12f; 
    public float switchHeight = 2f; 
    public int jumpDirection = 1; 
    public State state; 
    public bool isGrounded = false; 

    // Tilemaps 
    public Tilemap Tilemap_NormalBlocks; 
    public Tilemap Tilemap_LightBlocks; 
    public Tilemap Tilemap_DarkBlocks; 

    // Sprites
    public Sprite darkSprite; 
    public Sprite lightSprite; 

    // Start is called before the first frame update
    void Start()
    {
        state = State.LIGHT; 

        Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = true; 
        Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
        Jump(); 

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f); 
        transform.position += movement * Time.deltaTime * moveSpeed; 

        if (Input.GetButtonDown("Switch") && isGrounded ) 
        {
            Switch(); 
        }
    }

    void Jump() 
    {
        if (Input.GetButtonDown("Jump") && isGrounded )
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpDirection * jumpSpeed), ForceMode2D.Impulse); 
        }
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

            // Show tilemaps
            Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = false; 
            Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = true; 

            Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = true; 
            Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = false; 

        }

        else if (state == State.DARK) 
        {
            state = State.LIGHT; 

            // Flip player 
            this.gameObject.GetComponent<SpriteRenderer>().sprite = lightSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = false;
            transform.position = new Vector3(transform.position.x, transform.position.y + switchHeight, transform.position.z); 
            jumpDirection = 1; 

            // Show tilemaps
            Tilemap_LightBlocks.GetComponent<TilemapRenderer>().enabled = true; 
            Tilemap_DarkBlocks.GetComponent<TilemapRenderer>().enabled = false; 

            Tilemap_LightBlocks.GetComponent<TilemapCollider2D>().enabled = false; 
            Tilemap_DarkBlocks.GetComponent<TilemapCollider2D>().enabled = true; 
        }

        // Flip gravity
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = - this.gameObject.GetComponent<Rigidbody2D>().gravityScale; 
        
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        switch(collision.collider.tag)
        {
            case "Normal Block": 
            case "Light Block":
            case "Dark Block":
                isGrounded = true; 

            break; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        switch(collision.collider.tag)
        {
            case "Normal Block": 
            case "Light Block":
            case "Dark Block":
                isGrounded = false; 

            break; 
        }
    }
}
