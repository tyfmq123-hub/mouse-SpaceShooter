using UnityEngine;
using System.Collections;
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
    public int boomCount = 0;
    public int lives = 3;
    bool isInvincible = false;
    Vector3 startPos;

    public static event System.Action OnPlayerDead;

    List<Follower> activeFollowers = new List<Follower>();

    static readonly Vector3[] followerOffsets =
    {
        new Vector3(0.5f, 0, 0),
        new Vector3(1.0f, 0, 0),
        new Vector3(1.5f, 0, 0),
    };

    Vector3 minBound, maxBound;

    void Awake()
    {
        anim = GetComponent<Animator>();
        startPos = transform.position;
        nextFireTime = fireRate;

        Camera cam = Camera.main;
        minBound = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxBound = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));
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

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBound.x, maxBound.x),
            Mathf.Clamp(transform.position.y, minBound.y, maxBound.y),
            0
        );

        if (h > 0)
            PlayAnim("Right");
        else if (h < 0)
            PlayAnim("Left");
        else
            PlayAnim("Idel");
    }

    void Fire()
    {
        if (!Input.GetMouseButton(0) || Time.time < nextFireTime || isInvincible) return;

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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInvincible)
        {
            collision.gameObject.SetActive(false);
            TakeDamage(1);
        }
    }

    void UseBoom()
    {
        if (boomCount <= 0) return;
        boomCount--;

        if (BoomEffectPrefab != null)
        {
            GameObject effect = Instantiate(BoomEffectPrefab, Vector3.zero, Quaternion.identity);
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
        if (isInvincible) return;

        lives -= damage;

        if (lives <= 0)
        {
            OnPlayerDead?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        isInvincible = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(1.5f);

        transform.position = startPos;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(0.3f);
        isInvincible = false;
    }

    public void ResetPlayer()
    {
        lives = 3;
        boomCount = 0;
        powerLevel = 0;
        isInvincible = false;
        currentAnim = "";

        foreach (Follower f in activeFollowers)
            if (f != null) Destroy(f.gameObject);
        activeFollowers.Clear();

        transform.position = startPos;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(true);

        StopAllCoroutines();
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
