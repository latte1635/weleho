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
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(4);
            
        }
        if (col.gameObject.CompareTag("Obstacle"))
        {
            
        }
        if (col.gameObject.CompareTag("NoCollideProjectile") || col.gameObject.CompareTag("Water"))
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
    }
}