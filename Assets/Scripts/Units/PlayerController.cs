using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class GA_Stats
{
    public float  PlayTimeSeconds               { get; set; }
    public float   PlayerHealth                 { get; set; }
    public float   PlayerMana                   { get; set; }
    public int     HealthPotionsCollected       { get; set; }
    public int     ManaPotionsCollected         { get; set; }
    
    public int     Mobkills                     { get; set; }
    public int     WeaponUsedSword              { get; set; }
    public int     WeaponUsedPistol             { get; set; }
    public int     WeaponUsedRifle              { get; set; }
    public int     WeaponUsedFlame              { get; set; }
    public int     WeaponUsedGrenade            { get; set; }
    public int     TimesDug                     { get; set; }
}

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
    public GA_Stats gaStats;
    public DateTime timerStart;
    
    void Awake()
    {
        gaStats = new GA_Stats();
        timerStart = DateTime.Now;
        statSystem = gameObject.GetComponent<StatSystem>();
        statSystem.SetCharacterType(0);
        
        GameAnalytics.Initialize();
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Game start");
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
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        anim.SetBool("Walking", walking);
        anim.SetFloat("SpeedX", Input.GetAxisRaw("Horizontal"));
        anim.SetFloat("SpeedY", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("LastMoveX", lastMove.x);
        anim.SetFloat("LastMoveY", lastMove.y);
        
    }
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.CompareTag("NoCollideProjectile"))
        {
            Physics2D.IgnoreCollision(CC2D, coll.collider, true);
        }
        
    }

    private void SendGaInfo()
    {
        DateTime timerEnd = DateTime.Now;
        GetComponent<PlayerController>().gaStats.PlayTimeSeconds = (float)(timerEnd - timerStart).TotalSeconds;

        gaStats.PlayerHealth = GetComponent<StatSystem>().health;
        gaStats.PlayerMana = GetComponent<Shooting>().currentMana;
        
        
        GameAnalytics.NewDesignEvent("Play time in seconds", gaStats.PlayTimeSeconds);
        GameAnalytics.NewDesignEvent("Player health when game ended", gaStats.PlayerHealth);
        GameAnalytics.NewDesignEvent("Player mana when game ended", gaStats.PlayerMana);
        GameAnalytics.NewDesignEvent("Health potions collected", gaStats.HealthPotionsCollected);
        GameAnalytics.NewDesignEvent("Mana potions collected", gaStats.ManaPotionsCollected);
        GameAnalytics.NewDesignEvent("Mobs killed", gaStats.Mobkills);
        GameAnalytics.NewDesignEvent("Sword swings", gaStats.WeaponUsedSword);
        GameAnalytics.NewDesignEvent("Magic bolts thrown", gaStats.WeaponUsedPistol);
        GameAnalytics.NewDesignEvent("Magic missiles thrown", gaStats.WeaponUsedRifle);
        GameAnalytics.NewDesignEvent("Flame magic thrown", gaStats.WeaponUsedFlame);
        GameAnalytics.NewDesignEvent("Grenades thrown", gaStats.WeaponUsedGrenade);
    }

    public void OnGameEnd(bool won)
    {
        switch (won)
        {
            case false:
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Game Ended, Player lost");
                break;
            case true:
                GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Game Ended, Player won");
                break;
        }
        SendGaInfo();
    }
}


