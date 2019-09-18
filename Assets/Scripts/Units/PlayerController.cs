using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    private Transform playerPos;
    private CapsuleCollider2D CC2D;

    
    private StatSystem statSystem;
    
    public float moveSpeed;
    private float saveMoveSpeed;
    public float runSpeed;

    //walking is used in animations to determine if the character is walking
    private bool walking;

    //lastMove saves the direction the player moved in before stopping to decide idle standing direction
    private Vector2 lastMove;
  

    //???
    public static string dir;

    
    private static bool playerExists;

    // Start is called before the first frame update
    public void Start()
    {
        CC2D = GetComponent<CapsuleCollider2D>();
        saveMoveSpeed = moveSpeed;
        playerPos = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        statSystem = GetComponent<StatSystem>();


        statSystem.UpdateCharacterUI();

        //checks if player is already in scene
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    

    // Update is called once per frame
    void Update()
    {

        statSystem.UpdateCharacterUI();

        //change move speed with shift key
        if (Input.GetKey(KeyCode.LeftShift))
        {

            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = saveMoveSpeed;
        }
        


        //if no directional move button is pressed, character doesn't move
        walking = false;

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            //Give rigidbody2d of player vertical axis velocity
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * moveSpeed);
            walking = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //Give rigidbody2d of player horizontal axis velocity
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal")* moveSpeed, rb.velocity.y);
            walking = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            
        }
        
        if(Input.GetAxisRaw("Horizontal") < 0.5 && Input.GetAxisRaw("Horizontal") > -0.5)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        if (Input.GetAxisRaw("Vertical") < 0.5 && Input.GetAxisRaw("Vertical") > -0.5)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }

        
        //Choose animations to play based on player input
        anim.SetBool("Walking", walking);
        anim.SetFloat("SpeedX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("SpeedY", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        //If collides into projectiles, ignore hitboxes
        if(coll.gameObject.tag == "NoCollideProjectile")
        {
            Physics2D.IgnoreCollision(CC2D, coll.collider, true);
        }
        
    }
    
}


