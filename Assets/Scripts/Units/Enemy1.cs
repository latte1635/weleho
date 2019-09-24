using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : MonoBehaviour
{

    public float moveSpeed;
    
    private Rigidbody2D myRigidbody;
    
    public int enemy1Damage = 6;

    private bool moving;

    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;
    private StatSystem statSystem;

    void Awake()
    {
        statSystem = gameObject.GetComponent<StatSystem>();
        statSystem.SetCharacterType(1);
    }
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        timeBetweenMoveCounter = timeBetweenMove;
        timeToMoveCounter = timeToMove;
        statSystem.UpdateCharacterUI();
    }

    
    void Update()
    {
        //healthUI.transform.position = transform.position + healthUIOffset;

        statSystem.UpdateCharacterUI();

        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = moveDirection;

            if(timeToMoveCounter < 0)
            {
                moving = false;
                timeBetweenMoveCounter = timeBetweenMove;
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;

            if(timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = timeToMove;

                moveDirection = new Vector3(Random.Range(-1f, 1f) * moveSpeed, Random.Range(-1f, 1f) * moveSpeed, 0f);

            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (statSystem.GetCharacterType() == 0)
        {
            
            col.gameObject.GetComponent<StatSystem>().TakeDamage(enemy1Damage);
            SoundManagerScript.PlaySound("playerhit");
        }
    }
}
