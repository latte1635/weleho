using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grenade : MonoBehaviour
{
    public GameObject fragPrefab;
    private CircleCollider2D CC2D;
    public float grenadeLifeTime;
    private float nadeLifeTime;

    void Start()
    {
        CC2D = GetComponent<CircleCollider2D>();
        nadeLifeTime = grenadeLifeTime;
    }

    void Update()
    {
        
        nadeLifeTime -= Time.deltaTime;
        if (nadeLifeTime < 0)
        {
            Explode();
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Grenade hit something!");
        if (col.gameObject.tag == "NoCollideProjectile" || col.gameObject.tag == "Water")
        {
            Explode();
            Physics2D.IgnoreCollision(CC2D, col.collider, true);
        }
    }

    void Explode()
    {
        for (int i = 0; i < 20; i++)
        {
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            GameObject frags = Instantiate(fragPrefab, transform.position + (Vector3)(direction * 0.5f), Quaternion.identity);
            frags.GetComponent<Rigidbody2D>().velocity = direction * 30f;
        }
        Destroy(this.gameObject, 0f);
    }
}
