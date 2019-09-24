using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Currently not being used, might get deleted
public class SniperBullet : MonoBehaviour
{

    public float sniperLifeTime = 2f;

    private void Start()
    {
        Destroy(this.gameObject, sniperLifeTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Bullet hit something!");
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<StatSystem>().TakeDamage(10);
        }
        if (col.gameObject.CompareTag("Obstacle") || col.gameObject.CompareTag("CaveWall"))
        {
            Destroy(this.gameObject, 0f);
        }
        if (col.gameObject.CompareTag("NoCollideProjectile") || col.gameObject.CompareTag("Water"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<CircleCollider2D>(), col.collider, true);
        }
    }
}
