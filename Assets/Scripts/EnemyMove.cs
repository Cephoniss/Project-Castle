using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 1.0f; //Walking speed of enemy can be edited in inspector
    private Rigidbody2D rb; //Setting up Rigidbody2D for use with movement
    public Animator animator;
    bool isDead = false; 

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stops enemy movement
        EnemyMovement();
    }

    //The below is checking if it collides with the player or an object
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("isATK");//attack animation goes here
            Debug.Log("doing attack animation");
        }
         
        else if (collider.gameObject.CompareTag("Knife"))
        {
            EnemyDeath();
        }
        
        else //flips the enemy sprite when it triggers an object
        {
        speed = -speed;
        FlipSprite();
        }
        
        
        
    }
       void FlipSprite()
    {
        //This is supposed to check if the enemy is currently moving on x axis
        bool enemyHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        
        if (enemyHasHorizontalSpeed)
        {
            //Changing the scale of the player so that it flips when going in a direction
            transform.localScale = new Vector2 (-(Mathf.Sign(rb.velocity.x)), 1f); 
        }
        
    }

    void EnemyMovement()
    {
        if(!isDead)
        {
            rb.velocity = new Vector2(speed, 0f); //Enemy will automatically move horizontally
        } 

        else
        {
            rb.velocity = Vector2.zero;  //Stops enemy movement when isDead is true
        }
        
    }
    void EnemyDeath()
    {
        isDead = true;
        animator.SetTrigger("isDying");
        Debug.Log("Dying");
        StartCoroutine(Deletebody());
        //Add  sound  here
    }
    IEnumerator Deletebody()
   {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
   }
}
