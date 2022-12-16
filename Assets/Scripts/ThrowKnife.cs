using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    [SerializeField] public float speed = 15.0f; //speed of projectile
    private float xScale; //Creating float of xScale to use for directional fire of projectile
    public Rigidbody2D rb;
    PlayerMove player; //Getting PlayerMove script from Player
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMove>();
        xScale = player.transform.localScale.x * speed; //Getting the Scale of the player for directional projectile fire
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2 (xScale, 0f); //Updating postion of projectile. using xScale from above for direction of projectile
        FlipSprite();
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
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        Destroy(gameObject);
    }
       
}
