using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public float swordLifeTime = 0.01f;
    public int swordDamage = 13;
    private CircleCollider2D CC2D;

    private void Start()
    {
        SoundManagerScript.PlaySound("swordswing");

        CC2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, swordLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Enemy1 enemy = col.collider.GetComponent<Enemy1>();
        if (col.gameObject.tag == "NoCollideProjectile" || col.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
        else if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(swordDamage);
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
        else if (col.gameObject.tag == "Obstacle" || col.gameObject.tag == "CaveWall")
        {
            Destroy(gameObject, 0f);
        }
    }
}
