using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
   [SerializeField] public float speed = 5.0f; //Walking speed of player can be edited in inspector
   [SerializeField] public float jumpForce = 6.0f; //Jumping force of player can be edited in inspector
   [SerializeField] public float doubleJumpForce = 3.0f; //Double jump force can be edited in inspector
   [SerializeField] private bool isGrounded = false; //Bool to check if player has jumped or is on ground
   [SerializeField] private bool hasDoubleJumped = false; //Bool to check if player has jump and for use with double jump method
   private Rigidbody2D rb; //Setting up Rigidbody2D for use with player movement
   public Animator animator; 
   Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        PlayerMovement();
        FlipSprite();
        PlayerJump();
    }

    
    //Gets user input might delete this later
    //void GetInput(InputValue value)
    //{
    //    moveInput = value.Get<Vector2>();
    //    Debug.Log(moveInput);
    //}

    void PlayerMovement()
    {
        //Uses unity input method to get "Horizontal" movement might change this to use new input method
        float horizontalInput = Input.GetAxis("Horizontal"); 
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isWalking", playerHasHorizontalSpeed);
    }

    void PlayerJump()
    {
        //Checking for player input using unity input with "Jump" again might change this to new input method
        if (Input.GetButtonDown("Jump") && isGrounded) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            hasDoubleJumped = false; //Allows for doulbe jump by setting bool to false
            animator.SetBool("isJumping", true); //enables jump animation
        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && !hasDoubleJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            hasDoubleJumped = true; //Disables double jump
        }
    }
    
    // Using the below to check to see if player has jump or is on the ground. When the player activates a collision with an object tagged "Ground"
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false); //disables jump animation if grounded 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void FlipSprite()
    {
        //This is supposed to check if the player is currently moving on x axis
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        
        if (playerHasHorizontalSpeed)
        {
            //Changing the scale of the player so that it flips when going in a direction
            transform.localScale = new Vector2 (Mathf.Sign(rb.velocity.x), 1f); 
        }
        
    }
}
