using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletStraightToPlayer : MonoBehaviour
{
    
    public float speed = 5f;

    Vector3 moveDir;

    public void Init(Vector3 dir)
    {
        moveDir = dir.normalized;
    }

    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()?.TakeDamage(1);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Debug.Log(moveDir);
        Vector3 pos = this.transform.position;
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
        if (pos.x < -4 || pos.x > 4 || pos.y < -5 || pos.y > 5)
        {
            gameObject.SetActive(false);
        }
    }
}
