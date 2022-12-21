using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
   [SerializeField] public float speed = 4.0f; //Walking speed of player can be edited in inspector
   [SerializeField] public float jumpForce = 5.0f; //Jumping force of player can be edited in inspector
   [SerializeField] public float doubleJumpForce = 4.0f; //Double jump force can be edited in inspector
   [SerializeField] private bool isGrounded = false; //Bool to check if player has jumped or is on ground
   [SerializeField] private bool hasDoubleJumped = false; //Bool to check if player has jump and for use with double jump method
   [SerializeField] private bool isAlive = true; //Bool to check if player is alive
   [SerializeField] public GameObject knife; //GameObject to be edited in inspector
   [SerializeField] Transform throwKnife; //Transform for knife GameObject
   [SerializeField] AudioClip throwKnifeClip;
   [SerializeField] AudioSource throwKnifeAudio;

   private Rigidbody2D rb; //Setting up Rigidbody2D for use with player movement
   public Animator animator;
   public BoxCollider2D myFeet;
   //Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        if(!isAlive){return;} //Returns if isAlive is false
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

    //Using new unity input method for getting "OnFire" input and shooting projectile
    void OnFire(InputValue value)
    {
        Instantiate(knife, throwKnife.position, transform.rotation);
        throwKnifeAudio.PlayOneShot(throwKnifeClip); //Plays audio clip
    }
    
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
            //Add sound here
        }
        else if (Input.GetButtonDown("Jump") && !isGrounded && !hasDoubleJumped)
        {
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
            hasDoubleJumped = true; //Disables double jump
            //Add a slightly diff jump sound here
        }
    }
    
    // Using the below to check to see if player has jump or is on the ground. When the player activates a collision with an object tagged "Ground"
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false); //Disables jump animation if grounded 
        }

        else if (collider.gameObject.CompareTag("Enemy")) //Adding this here to check if player collides with enemy
        {
            Death();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Ground"))
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

    void Death()
    {
        isAlive = false;
        animator.SetTrigger("isDying"); //Enables dying animation
        StartCoroutine(ReloadScene()); //Added wait time before reloading
        //Add sound here
        
    }

    IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<GameSession>().PlayerDeath();
    }



}
