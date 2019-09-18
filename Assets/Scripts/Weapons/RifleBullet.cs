﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : MonoBehaviour
{
    private CircleCollider2D CC2D;
    public float rifleBulletLifeTime = 2f;
    public int rifleBulletDamage = 10;


    private void Start()
    {
        SoundManagerScript.PlaySound("spell");

        CC2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, rifleBulletLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "NoCollideProjectile" || col.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(rifleBulletDamage);
            Destroy(gameObject, 0f);
            if (col.gameObject.name == "BossTest")
            {
                SoundManagerScript.PlaySound("bossHit");
            }
            else
            {
                SoundManagerScript.PlaySound("spellhit");
            }
        }
        if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "CaveWall" || col.gameObject.tag == "CaveUnbreakableWall")
        {
            
            Destroy(gameObject, 0f);
        }
    }
}