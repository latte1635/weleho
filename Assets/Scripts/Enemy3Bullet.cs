using System.Collections;
using UnityEngine;


public class Enemy3Bullet : StatSystem
{
    private CircleCollider2D CC2D;
    public float enemy3BulletLifeTime = 1.5f;
    public int enemy3BulletDamage = 20;

    private void Start()
    {
        CC2D = GetComponent<CircleCollider2D>();
        Destroy(gameObject, enemy3BulletLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if ( col.gameObject.CompareTag("EnemyProjectile") || col.gameObject.CompareTag("Water"))
        {
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
        if(col.gameObject.CompareTag("NoCollideProjectile"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(enemy3BulletDamage);
            SoundManagerScript.PlaySound("playerhit");
            Destroy(this.gameObject, 0f);
        }
        if (col.gameObject.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject, 0f);
        }
    }
}
