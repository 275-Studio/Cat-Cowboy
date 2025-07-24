using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    public HomeStats homeStats;
    public PlayerStats playerStats;
    public ItemUpgrade itemUpgrade;
}

[System.Serializable]
public class HomeStats
{
    public float maxHealthHome;
    public float currentHealthHome;
    public float homeArmor;
}

[System.Serializable]
public class PlayerStats
{
    public float reloadTime;
    public float bulletSpeed;
    public int maxBulletCapacity;
    public int coin;
    public int destroyedBarrels;
}

[System.Serializable]
public class ItemUpgrade
{
    public int bomb;
    public float timeFrezee;
}
[System.Serializable]
public class BarrelData
{
    public GameObject barrelPrefab;
    public int jumlah;
}
[System.Serializable]
public class ChestData
{
    public GameObject chestPrefab;
    public int jumlah;
}
