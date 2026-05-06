using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefabA;
    public GameObject bulletPrefabB;
    public Transform firePoint;
    public float fireRate = 0.2f;
    public float spreadOffset = 0.03f;

    public GameObject BoomEffectPrefab;
    public GameObject followerPrefab;

    Animator anim;
    string currentAnim;
    float nextFireTime;
    [SerializeField, Range(0, 2)] int powerLevel = 0;
    [SerializeField] int boomCount = 0;
    public int lives = 3;

    List<Follower> activeFollowers = new List<Follower>();

    static readonly Vector3[] followerOffsets =
    {
        new Vector3(0.5f, 0, 0),
        new Vector3(1.0f, 0, 0),
        new Vector3(1.5f, 0, 0),
    };

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();

        if (Input.GetKeyDown(KeyCode.B))
            UseBoom();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, v, 0).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (h > 0)
            PlayAnim("Right");
        else if (h < 0)
            PlayAnim("Left");
        else
            PlayAnim("Idel");
    }

    void Fire()
    {
        if (!Input.GetMouseButton(0) || Time.time < nextFireTime) return;

        nextFireTime = Time.time + fireRate;

        switch (powerLevel)
        {
            case 0:
                Instantiate(bulletPrefabA, firePoint.position, firePoint.rotation);
                break;
            case 1:
                Vector3 left  = firePoint.position - firePoint.right * spreadOffset;
                Vector3 right = firePoint.position + firePoint.right * spreadOffset;
                Instantiate(bulletPrefabA, left,  firePoint.rotation);
                Instantiate(bulletPrefabA, right, firePoint.rotation);
                break;
            case 2:
                Instantiate(bulletPrefabB, firePoint.position, firePoint.rotation);
                break;
        }

        for (int i = 0; i < activeFollowers.Count; i++)
            activeFollowers[i].TryFire();
    }

    void UseBoom()
    {
        if (boomCount <= 0) return;
        boomCount--;

        if (BoomEffectPrefab != null)
        {
            GameObject effect = Instantiate(BoomEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 3f);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
                health.TakeDamage(100);
        }

        BulletStraightToPlayer[] bullets = FindObjectsByType<BulletStraightToPlayer>(FindObjectsSortMode.None);
        foreach (BulletStraightToPlayer bullet in bullets)
            bullet.gameObject.SetActive(false);
    }

    public void PowerUp()
    {
        powerLevel = Mathf.Min(powerLevel + 1, 2);

        if (activeFollowers.Count < 3)
        {
            int index = activeFollowers.Count;
            Transform followTarget = index == 0 ? transform : activeFollowers[index - 1].transform;
            Vector3 spawnPos = followTarget.position + followerOffsets[0];
            GameObject obj = Instantiate(followerPrefab, spawnPos, Quaternion.identity);
            Follower follower = obj.GetComponent<Follower>();
            follower.Init(followTarget, followerOffsets[0]);
            activeFollowers.Add(follower);
        }
    }

    public void TakeDamage(int damage)
    {
        lives -= damage;
        if (lives <= 0)
            gameObject.SetActive(false);
    }

    public void GetBoom()
    {
        boomCount++;
    }

    void PlayAnim(string animName)
    {
        if (currentAnim == animName) return;
        currentAnim = animName;
        anim.Play(animName);
    }
}
