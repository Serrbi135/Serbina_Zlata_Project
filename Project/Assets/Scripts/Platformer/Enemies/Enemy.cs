using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public GameObject prefabDie;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    
    void Die()
    {
        Destroy(gameObject);
        Instantiate(prefabDie, transform.position, Quaternion.identity);
    }

    
}
