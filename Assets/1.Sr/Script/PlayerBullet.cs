using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.y > 1f)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.GetComponent<EnemyHealth>();
            if (health != null)
                health.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
