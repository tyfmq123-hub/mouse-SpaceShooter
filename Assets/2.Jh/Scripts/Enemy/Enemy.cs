using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float maxShotDelay;
    public float curShotDelay;
    public Vector3 moveDir;
    GameObject player;


    

    public EnemyTypes type;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Move();
        if (EnemyTypes.C == type)
        {
            Shoot();
            Reload();
        }
    }

    void Move()
    {
        Vector3 pos = this.transform.position;
        transform.Translate(0, -speed * Time.deltaTime, 0);
        if (pos.x < -4 || pos.x > 4 || pos.y < -5 || pos.y > 8)
        {
            gameObject.SetActive(false);
        }
    }


    void Shoot()
    {
        if (curShotDelay < maxShotDelay) return;

        GameObject bullet = PoolManager.Instance.MakeObj("bulletEnemyB");
        GameObject bullet2 = PoolManager.Instance.MakeObj("bulletEnemyB");

        // ⭐ 로컬 → 월드 변환
        Vector3 APos1 = transform.TransformPoint(new Vector3(-0.4f, -0.5f, 0));
        Vector3 APos2 = transform.TransformPoint(new Vector3(0.4f, -0.5f, 0));

        bullet.transform.position = APos1;
        bullet2.transform.position = APos2;

        // ⭐ 방향도 총구 기준으로 계산
        Vector3 dir = (player.transform.position - APos1).normalized;
        Vector3 dir2 = (player.transform.position - APos2).normalized;

        BulletStraightToPlayer b = bullet.GetComponent<BulletStraightToPlayer>();
        BulletStraightToPlayer b2 = bullet2.GetComponent<BulletStraightToPlayer>();

        if (b != null && b2 != null)
        {
            b.Init(dir);
            b2.Init(dir2);
        }

        curShotDelay = 0;
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }
    
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            /*PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(1);
            }*/
        }
    }
}