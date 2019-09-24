using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public GameObject projectile;
    private Transform player;
    public float speed;
    private Rigidbody2D myRigidbody;
    public float timeBetweenMove;
    private float timeBetweenMoveCounter;
    public float timeToMove;
    private float timeToMoveCounter;
    private Vector3 moveDirection;
    private StatSystem statSystem;
    public int enemy1Damage = 10;
    private bool moving;
    private float timeBetweenShots;
    public float startTimeBetweenShots;

    void Awake()
    {
        gameObject.GetComponent<StatSystem>().SetCharacterType(2);
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
        myRigidbody = GetComponent<Rigidbody2D>();
        timeBetweenMoveCounter = timeBetweenMove;
        timeToMoveCounter = timeToMove;

        statSystem = GetComponent<StatSystem>();


        statSystem.UpdateCharacterUI();
    }

    // Update is called once per frame
    void Update()
    {
        statSystem.UpdateCharacterUI();

        if (moving)
        {
            timeToMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = moveDirection;

            if (timeToMoveCounter < 0)
            {
                    moving = false;
                    timeBetweenMoveCounter = timeBetweenMove;
            }
        }
        else
        {
            timeBetweenMoveCounter -= Time.deltaTime;
            myRigidbody.velocity = Vector2.zero;

            if (timeBetweenMoveCounter < 0f)
            {
                moving = true;
                timeToMoveCounter = timeToMove;

                moveDirection = new Vector3(Random.Range(-1f, 1f) * speed, Random.Range(-1f, 1f) * speed, 0f);
            }
        }

            if (timeBetweenShots <= 0 && player != null)
            {
                Vector3 direction = (player.position - transform.position);
                float angle = (Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 4) * Mathf.Rad2Deg;
                //Create projectile and define its orientation
                Instantiate(projectile, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));

                timeBetweenShots = startTimeBetweenShots;
            }
            else
            {
                timeBetweenShots -= Time.deltaTime;
            }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundManagerScript.PlaySound("playerhit");
            col.gameObject.GetComponent<StatSystem>().TakeDamage(enemy1Damage);

        }
        if (col.gameObject.tag == "Obstacle")
        {
            //Destroy(this.gameObject, 0f);
        } 
        if (col.gameObject.tag == "NoCollideProjectile")
        {
            Destroy(col.gameObject);
            //Destroy(gameObject);
        }
    }
}
