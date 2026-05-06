using System;
using System.Diagnostics;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    //에너미 프리팹
    public GameObject enemyAPrefab;
    public GameObject enemyBPrefab;
    public GameObject enemyCPrefab;

    public GameObject BossPrefab;

    //아이템 프리팹
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;

    public GameObject itemBoomPrefab;

    //플레이어 총알 관련 프리팹
    public GameObject bulletPlayerAPrefab;

    public GameObject bulletPlayerBPrefab;

    //팔로워 총알 관련 프리팹
    public GameObject bulletFollowerPrefab;

    //에너미 관련 총알 프리팹
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletBossAPrefab;
    public GameObject bulletBossBPrefab;
    public GameObject bulletBossCPrefab;

    GameObject[] enemyA;
    GameObject[] enemyB;
    GameObject[] enemyC;
    GameObject[] enemyBoss;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletFollower;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletBossA;
    GameObject[] bulletBossB;
    GameObject[] bulletBossC;

    private GameObject[] targetPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //에너미
        enemyA = new GameObject[20];
        enemyB = new GameObject[20];
        enemyC = new GameObject[20];
        enemyBoss = new GameObject[2];

        //아이템
        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        //플레이어 총알
        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[50];
        bulletFollower = new GameObject[100];

        //에너미 총알
        bulletEnemyB = new GameObject[100];

        //보스 총알
        bulletBossA = new GameObject[100];
        bulletBossB = new GameObject[100];
        bulletBossC = new GameObject[100];
        Generate();
    }

    //미리 생성하는 코드
    void Generate()
    {
        //에너미
        for (int index = 0; index < enemyA.Length; index++)
        {
            enemyA[index] = Instantiate(enemyAPrefab);
            enemyA[index].SetActive(false);
            enemyA[index].transform.SetParent(transform);
        }
        for (int index = 0; index < enemyB.Length; index++)
        {
            enemyB[index] = Instantiate(enemyBPrefab);
            enemyB[index].SetActive(false);
            enemyB[index].transform.SetParent(transform);
        }
        for (int index = 0; index < enemyC.Length; index++)
        {
            enemyC[index] = Instantiate(enemyCPrefab);
            enemyC[index].SetActive(false);
            enemyC[index].transform.SetParent(transform);
        }
        for (int index = 0; index < enemyBoss.Length; index++)
        {
            enemyBoss[index] = Instantiate(BossPrefab);
            enemyBoss[index].SetActive(false);
            enemyBoss[index].transform.SetParent(transform);
        }
        //아이템
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].SetActive(false);
            itemCoin[index].transform.SetParent(transform);
        }

        for (int index = 0; index < itemPower.Length; index++)
        {
            itemPower[index] = Instantiate(itemPowerPrefab);
            itemPower[index].SetActive(false);
            itemPower[index].transform.SetParent(transform);
        }

        for (int index = 0; index < itemBoom.Length; index++)
        {
            itemBoom[index] = Instantiate(itemBoomPrefab);
            itemBoom[index].SetActive(false);
            itemBoom[index].transform.SetParent(transform);
        }

        // 플레이어 총알
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
            bulletPlayerA[index].transform.SetParent(transform);
        }

        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
            bulletPlayerB[index].transform.SetParent(transform);
        }

        //팔로워 총알
        for (int index = 0; index < bulletFollower.Length; index++)
        {
            bulletFollower[index] = Instantiate(bulletFollowerPrefab);
            bulletFollower[index].SetActive(false);
            bulletFollower[index].transform.SetParent(transform);
        }

        //에너미 총알
        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[index].SetActive(false);
            bulletEnemyB[index].transform.SetParent(transform);
        }

        //보스 총알
        for (int index = 0; index < bulletBossA.Length; index++)
        {
            bulletBossA[index] = Instantiate(bulletBossAPrefab);
            bulletBossA[index].SetActive(false);
            bulletBossA[index].transform.SetParent(transform);
        }

        for (int index = 0; index < bulletBossB.Length; index++)
        {
            bulletBossB[index] = Instantiate(bulletBossBPrefab);
            bulletBossB[index].SetActive(false);
            bulletBossB[index].transform.SetParent(transform);
        }

        for (int index = 0; index < bulletBossC.Length; index++)
        {
            bulletBossC[index] = Instantiate(bulletBossCPrefab);
            bulletBossC[index].SetActive(false);
            bulletBossC[index].transform.SetParent(transform);
        }
    }

    //
    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "bulletPlayerA":
                targetPool = bulletPlayerA;
                break;
            case "bulletPlayerB":
                targetPool = bulletPlayerB;
                break;
            case "bulletFollower":
                targetPool = bulletFollower;
                break;
            case "bulletBossA":
                targetPool = bulletBossA;
                break;
            case "bulletBossB":
                targetPool = bulletBossB;
                break;
            case "bulletBossC":
                targetPool = bulletBossC;
                break;
            case "bulletEnemyB":
                targetPool = bulletEnemyB;
                break;
            case "enemyA":
                targetPool = enemyA;
                break;
            case "enemyB":
                targetPool = enemyB;
                break;
            case "enemyC":
                targetPool = enemyC;
                break;
            case "enemyBoss":
                targetPool = enemyBoss;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;
    }
}