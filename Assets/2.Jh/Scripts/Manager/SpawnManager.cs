using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] destinationPoints;

    StageData stageData;

    void Start()
    {
        stageData = EnemyDataManager.Instance.StageData;

        if (stageData == null)
        {
            Debug.LogError("StageData 없음");
            return;
        }

        StartCoroutine(StartStage());
    }

    IEnumerator StartStage()
    {
        foreach (var wave in stageData.waves)
        {
            yield return StartCoroutine(SpawnWave(wave));
        }

        Debug.Log("스테이지 종료");
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        foreach (var enemyData in wave.enemies)
        {
            yield return new WaitForSeconds(enemyData.delay);

            SpawnEnemy(enemyData);
        }
    }

    void SpawnEnemy(EnemyData data)
    {
        string enemyName = GetEnemyName(data.enemyType);

        GameObject enemy = PoolManager.Instance.MakeObj(enemyName);

        // ⭐ null 방어
        if (enemy == null)
        {
            Debug.LogError($"스폰 실패: {enemyName}");
            return;
        }

        // ⭐ 보스는 따로 처리
        if (data.enemyType == EnemyTypes.Boss)
        {
            SpawnBoss(enemy);
            return;
        }

        // ⭐ JSON point 사용
        int point = data.point;

        if (point < 0 || point >= spawnPoints.Length)
        {
            Debug.LogError($"point 범위 오류: {point}");
            return;
        }

        enemy.transform.position = spawnPoints[point].position;

        SetEnemyDirection(enemy, point);
    }

    // ⭐ 보스 전용 스폰
    void SpawnBoss(GameObject boss)
    {
        Debug.Log("🔥 보스 등장!");

        // 화면 위 중앙
        boss.transform.position = new Vector3(0f, 4.5f, 0f);
        boss.transform.rotation = Quaternion.identity;

        
    }

    // ⭐ enum → string
    string GetEnemyName(EnemyTypes type)
    {
        switch (type)
        {
            case EnemyTypes.A: return "enemyA";
            case EnemyTypes.B: return "enemyB";
            case EnemyTypes.C: return "enemyC";
            case EnemyTypes.Boss: return "enemyBoss";
        }
        return "enemyA";
    }

    // ⭐ 방향 설정
    void SetEnemyDirection(GameObject enemy, int point)
    {
        if (point == 5 || point == 6)
        {
            int ranDest = Random.Range(0, 2);

            Vector3 dir = destinationPoints[ranDest].position - enemy.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            enemy.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
        else if (point == 7)
        {
            int ranDest = Random.Range(2, 4);

            Vector3 dir = destinationPoints[ranDest].position - enemy.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            enemy.transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
        else
        {
            enemy.transform.rotation = Quaternion.identity;
        }
    }

    // ⭐ (옵션) 보스 등장 연출
    IEnumerator BossEntrance(GameObject boss)
    {
        Vector3 start = new Vector3(0, 6f, 0);
        Vector3 target = new Vector3(0, 3.5f, 0);

        boss.transform.position = start;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            boss.transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }
    }
}