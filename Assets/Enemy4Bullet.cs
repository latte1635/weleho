using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Bullet : MonoBehaviour
{
    private CircleCollider2D CC2D;
    public float enemy3BulletLifeTime = 1.5f;
    public int enemy4BulletDamage = 3;

    private void Start()
    {
        CC2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, enemy3BulletLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag == "EnemyProjectile" || col.gameObject.tag == "Water")
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
        if (col.gameObject.tag == "NoCollideProjectile")
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(enemy4BulletDamage);
            SoundManagerScript.PlaySound("playerhit");
            Destroy(this.gameObject, 0f);
        }
        if (col.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject, 0f);
        }
    }
}
