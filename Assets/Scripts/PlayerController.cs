using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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

    public Transform groundCheckLeft;

    public Transform groundCheckRight;
    private BoxCollider2D boxCollider2d;

    //[SerializeField] private LayerMask layerMask;


    private void Awake() {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
    }

    // Sprites
    public Sprite darkSprite; 
    public Sprite lightSprite; 


  


    // Start is called before the first frame update
    void Start()
    {
        state = State.LIGHT; 
    }

    // Update is called once per frame

    void Update()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);
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
            this.gameObject.GetComponent<SpriteRenderer>().sprite = darkSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            transform.position = new Vector3(transform.position.x, transform.position.y - switchHeight, transform.position.z); 
            state = State.DARK; 
            jumpDirection = -1; 
            groundCheckRight.position = new Vector3(groundCheckRight.position.x, groundCheckRight.position.y + 1.01f, groundCheckRight.position.z); 
            groundCheckLeft.position = new Vector3(groundCheckLeft.position.x, groundCheckLeft.position.y + 1.01f, groundCheckLeft.position.z); 
        }

        else if (state == State.DARK) 
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = lightSprite; 
            this.gameObject.GetComponent<SpriteRenderer>().flipY = false;
            transform.position = new Vector3(transform.position.x, transform.position.y + switchHeight, transform.position.z); 
            state = State.LIGHT; 
            jumpDirection = 1; 
            groundCheckRight.position = new Vector3(groundCheckRight.position.x, groundCheckRight.position.y - 1.01f, groundCheckRight.position.z);
            groundCheckLeft.position = new Vector3(groundCheckLeft.position.x, groundCheckLeft.position.y - 1.01f, groundCheckLeft.position.z);
        }

        // Flip gravity
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = - this.gameObject.GetComponent<Rigidbody2D>().gravityScale; 
        
        
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Ground") 
        {
            isGrounded = true; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag == "Ground") 
        {
            isGrounded = false; 
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down * .1f, layerMask);
        return raycastHit2d.collider != null;
    }
*/
}
