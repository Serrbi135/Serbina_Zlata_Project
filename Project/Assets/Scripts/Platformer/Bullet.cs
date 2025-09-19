using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; 
    public float lifetime = 1f; 
    public int damage = 1; 
    private Vector2 direction;

    void Start()
    {
        
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage); 
            Destroy(gameObject); 
        }
    }
}
