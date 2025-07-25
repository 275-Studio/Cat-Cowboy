using System;
using TMPro;
using UnityEngine;

public class ItemStat : MonoBehaviour
{
    public Stats stats;
    public static ItemStat instance;
    private PlayerStats playerStats;
    private HomeStats homeStats;
    public TextMeshProUGUI coinText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }   
        stats = new Stats
        {
            homeStats = new HomeStats
            {
                maxHealthHome = 100f,
                currentHealthHome = 100f,
                homeArmor = 10f
            },
            playerStats = new PlayerStats
            {
                coin = 100,
                reloadTime = 2f,
                bulletSpeed = 3f,
                maxBulletCapacity = 10,
                score = 0,
                destroyedBarrel = 0,
            },
            itemUpgrade = new ItemUpgrade
            {
                bomb = 0,
                timeFrezee = 0f,
            }
        };
    }

    void Start()
    {
        playerStats = stats.playerStats;
        homeStats = stats.homeStats;

    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = playerStats.coin.ToString();
    }
}
