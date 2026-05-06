using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;

    public OwnerType owner;

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (owner == OwnerType.Player && other.CompareTag("Enemy"))
        {
            Debug.Log("Hit");
            other.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            gameObject.SetActive(false);
        }
        else if (owner == OwnerType.Enemy && other.CompareTag("Player"))
        {
            other.GetComponent<Player>()?.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}


