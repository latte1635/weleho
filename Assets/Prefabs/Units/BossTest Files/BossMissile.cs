using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissile : StatSystem
{
    public float speed;

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private CircleCollider2D CC2D;
    private float lastsFor;
    public float startLastsFor;

    public int bossMissileDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveDirection = (player.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        lastsFor = startLastsFor;
        CC2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (lastsFor <= 0)
        {
            DestroyProjectile();
            lastsFor = startLastsFor;
        }
        else
        {
            lastsFor -= Time.deltaTime;
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "NoCollideProjectile")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(bossMissileDamage);
            SoundManagerScript.PlaySound("playerhit");
            Destroy(this.gameObject, 0f);
        }
        if (col.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject, 0f);
        }

    }

    void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    
}

