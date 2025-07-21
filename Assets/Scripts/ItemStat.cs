using System;
using TMPro;
using UnityEngine;

public class ItemStat : MonoBehaviour
{
    public Stats stats;
    public static ItemStat instance;
    public TextMeshProUGUI text;
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
    }

    void Start()
    {
        stats = new Stats
        {
            homeStats = new HomeStats
            {
                maxHealthHome = 100f,
                homeArmor = 10f
            },
            playerStats = new PlayerStats
            {
                coin = 100,
                reloadTime = 1f,
            },
            itemUpgrade = new ItemUpgrade
            {
                bomb = 0,
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        text.text = stats.homeStats.maxHealthHome.ToString();
    }

    public void buyUpdate(int btnIndex)
    {
        if (btnIndex == 0)
        {
            // if()
            Debug.Log("max home health added!");
            maxHomeStats();
        }
        else if(btnIndex == 1)
        {
            
        }
        else if(btnIndex == 2)
        {
            
        }
    }

    private void maxHomeStats()
    {
        stats.homeStats.maxHealthHome += 10;
    }

    private void maxReloadSpeed()
    {
        stats.playerStats.reloadTime += 0.5f;
    }

    private void homeArmot()
    {
        stats.homeStats.homeArmor += 1;
    }
}
