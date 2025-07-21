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
    public float homeArmor;
}

[System.Serializable]
public class PlayerStats
{
    public float reloadTime;
    public int coin;
}

[System.Serializable]
public class ItemUpgrade
{
    public int bomb;
}
[System.Serializable]
public class BarrelData
{
    public GameObject barrelPrefab;
    public int jumlah;
}

