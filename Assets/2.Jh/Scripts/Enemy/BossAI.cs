using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAI : MonoBehaviour
{
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;
    private GameObject player;
    float angle = 0f;
    float angleSpeed = 80f; // 회전 속도
    float maxAngle = 60f; // 범위 제한
    int dir = 1; // 방향 (1 = 오른쪽, -1 = 왼쪽)
    public EnemyTypes type;
    
    EnemyHealth health;
    bool isMoving = true;
    bool isPatternRunning = false; // ⭐ 패턴 중복 방지

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        // ⭐ 같은 오브젝트에 붙어있는 Health 가져오기
        health = GetComponent<EnemyHealth>();

        // ⭐ 여기!
        if (health != null)
        {
            health.OnDead += StopAI;
        }
    }


    void Update()
    {
        Move();
        angleUpdate();
    }

    void Move()
    {
        if (!isMoving) return;

        transform.Translate(0, -5 * Time.deltaTime, 0);

        if (transform.position.y <= 3)
        {
            isMoving = false;

            CancelInvoke(); // ⭐ 기존 예약 제거
            Invoke(nameof(Think), 2f);
        }
    }

    void Think()
    {
        if (isPatternRunning) return; // ⭐ 중복 방지

        CancelInvoke(); // ⭐ 혹시 남아있는 Invoke 제거

        isPatternRunning = true;

        patternIndex = (patternIndex + 1) % 4;
        curPatternCount = 0;

        switch (patternIndex)
        {
            case 0:
                FireForward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireForward()
    {
        Debug.Log("앞으로 4발 발사");
        GameObject bulletL1 = PoolManager.Instance.MakeObj("bulletBossC");
        GameObject bulletL2 = PoolManager.Instance.MakeObj("bulletBossC");
        GameObject bulletR1 = PoolManager.Instance.MakeObj("bulletBossC");
        GameObject bulletR2 = PoolManager.Instance.MakeObj("bulletBossC");
        Vector3 L1Pos = new Vector3(transform.position.x + 0.6f, transform.position.y - 1.5f, transform.position.z);
        Vector3 L2Pos = new Vector3(transform.position.x + 0.8f, transform.position.y - 1.5f, transform.position.z);
        Vector3 R1Pos = new Vector3(transform.position.x - 0.6f, transform.position.y - 1.5f, transform.position.z);
        Vector3 R2Pos = new Vector3(transform.position.x - 0.8f, transform.position.y - 1.5f, transform.position.z);

        bulletL1.transform.position = L1Pos;
        bulletL2.transform.position = L2Pos;
        bulletR1.transform.position = R1Pos;
        bulletR2.transform.position = R2Pos;

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke(nameof(FireForward), 2f);
        }
        else
        {
            EndPattern();
        }
    }

    void FireShot()
    {
        Debug.Log("플레이어 방향으로 샷건");
        int bulletCount = 5;
        float spreadAngle = 40f;

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);


        Vector3 baseDir = (player.transform.position - transform.position).normalized;

        for (int index = 0; index < bulletCount; index++)
        {
            GameObject bullet = PoolManager.Instance.MakeObj("bulletBossA");

            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            bullet.transform.position = bulletPos;
            bullet.transform.rotation = Quaternion.identity;

            float angle = startAngle + (angleStep * index);

            // ⭐ 방향 회전
            Vector3 dir = Quaternion.Euler(0, 0, angle) * baseDir;

            BulletStraightToPlayer b = bullet.GetComponent<BulletStraightToPlayer>();

            b.speed = 5;
            if (b != null)
            {
                b.Init(dir); // ⭐ 방향 전달
            }
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke(nameof(FireShot), 3.5f);
        }
        else
        {
            EndPattern();
        }
    }

    void FireArc()
    {
        //부채꼴

        GameObject bullet = PoolManager.Instance.MakeObj("bulletBossA");

        Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z);
        bullet.transform.position = bulletPos;
        bullet.transform.rotation = Quaternion.identity;

        // ⭐ 이미 계산된 angle 사용
        Vector3 dirVec = Quaternion.Euler(0, 0, angle) * Vector3.down;

        var b = bullet.GetComponent<BulletStraightToPlayer>();
        if (b != null)
        {
            b.Init(dirVec);
        }

        b.speed = 5;


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke(nameof(FireArc), 0.2f);
        }
        else
        {
            EndPattern();
        }
    }

    void angleUpdate()
    {
        // ⭐ 각도는 여기서 계속 움직임
        angle += angleSpeed * dir * Time.deltaTime;

        if (angle > maxAngle)
        {
            angle = maxAngle;
            dir = -1;
        }
        else if (angle < -maxAngle)
        {
            angle = -maxAngle;
            dir = 1;
        }
    }

    void FireAround()
    {
        Debug.Log("원 형태로 전체 공격");
        int roundNumA = 30;
        int roundNumB = 20;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
        for (int i = 0; i < roundNum; i++)
        {
            GameObject bullet = PoolManager.Instance.MakeObj("bulletBossB");

            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            bullet.transform.position = bulletPos;
            bullet.transform.rotation = Quaternion.identity;

            Vector3 dirVec = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / roundNum),
                Mathf.Sin(Mathf.PI * 2 * i / roundNum));

            Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
            var b = bullet.GetComponent<BulletStraightToPlayer>();
            b.speed = 2;
            if (b != null)
            {
                b.Init(dirVec);
            }
        }


        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke(nameof(FireAround), 0.7f);
        }
        else
        {
            EndPattern();
        }
    }

    void EndPattern()
    {
        isPatternRunning = false;

        CancelInvoke(); // ⭐ 혹시 남은 Invoke 제거
        Invoke(nameof(Think), 3f);
    }
    
    void StopAI()
    {
        CancelInvoke();
        enabled = false;
    }
    void OnDisable()
    {
        CancelInvoke();

        // ⭐ 이벤트 해제 (중요)
        if (health != null)
        {
            health.OnDead -= StopAI;
        }
    }
}