using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class grenadeFrag : StatSystem
{
    private CircleCollider2D CC2D;
    public float fragLifeTime = .2f;

    private void Start()
    {

        CC2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, fragLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Frag hit something!");
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(60);
            Destroy(gameObject, 0f);
        }
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "CaveWall")
        {
            //CaveGen.MineWallTile();
            Destroy(gameObject, 0f);
        }
        if (col.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
    }
}

