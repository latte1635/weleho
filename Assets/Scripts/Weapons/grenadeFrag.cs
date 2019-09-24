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
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(60);
            Destroy(gameObject, 0f);
        }
        if (col.gameObject.CompareTag("Obstacle") || col.gameObject.CompareTag("CaveWall"))
        {
            //CaveGen.MineWallTile();
            Destroy(gameObject, 0f);
        }
        if (col.gameObject.CompareTag("Water"))
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
    }
}

