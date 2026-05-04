using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.y > 1f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
