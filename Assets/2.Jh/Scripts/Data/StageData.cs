using System;
using UnityEngine;

[Serializable]
public class StageData
{
    public WaveData[] waves;
}

[Serializable]
public class WaveData
{
    public EnemyData[] enemies;
}

[Serializable]
public class EnemyData
{
    public float delay;
    public EnemyTypes enemyType;
    public int point;
}