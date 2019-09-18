using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlamerFlame : StatSystem
{
    private CircleCollider2D CC2D;
    public float flameLifeTime = .2f;

    private void Start()
    {
        SoundManagerScript.PlaySound("flame");
        CC2D = GetComponent<CircleCollider2D>();
        Destroy(this.gameObject, flameLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Flame hit something!");
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(4);
            
        }
        if (col.gameObject.tag == "Obstacle")
        {
            
        }
        if (col.gameObject.tag == "NoCollideProjectile" || col.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
    }
}