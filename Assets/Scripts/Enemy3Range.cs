using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Range : MonoBehaviour
{
    private Enemy3 parent;

    private void Start()
    {
        parent = GetComponentInParent<Enemy3>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.target = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.target = null;
        }
    }

}
