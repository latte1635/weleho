using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Transform playerPos;
    private CapsuleCollider2D CC2D;

    public float moveSpeed;
    private float saveMoveSpeed;
    public float runSpeed;
    private bool walking;
    private Vector2 lastMove;
    private static bool playerExists;
    private StatSystem statSystem;

    void Awake()
    {
        statSystem = gameObject.GetComponent<StatSystem>();
        statSystem.SetCharacterType(0);
        
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "taso1");
    }

    // Start is called before the first frame update
    public void Start()
    {
        
        CC2D = GetComponent<CapsuleCollider2D>();
        saveMoveSpeed = moveSpeed;
        playerPos = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
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

        if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            //Give rigidbody2d of player vertical axis velocity
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal")* moveSpeed, Input.GetAxisRaw("Vertical") * moveSpeed);
            walking = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
            
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
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
        if(coll.gameObject.CompareTag("NoCollideProjectile"))
        {
            Physics2D.IgnoreCollision(CC2D, coll.collider, true);
        }
        
    }
    
}


