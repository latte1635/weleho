using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed;
    private Vector2 moveDirection;
    private Transform player;
    private Rigidbody2D rb;
    private CircleCollider2D CC2D;
    private float lastsFor;
    public float startLastsFor;

    public int MissileDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastsFor = startLastsFor;
        CC2D = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = (player.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "NoCollideProjectile")
        {
            Destroy(col.gameObject);
            DestroyProjectile();
        }
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(MissileDamage);
            SoundManagerScript.PlaySound("playerhit");
            DestroyProjectile();
        }
        if (col.gameObject.tag == "Obstacle")
        {
            DestroyProjectile();
        }

    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
