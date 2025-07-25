using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonHandler : MonoBehaviour
{
    public WeaponData[] weaponDataList;
    private int[] weaponPrices = { 0, 250, 500 };
    public HomeController homeController;
    public PlayerStats playerStats;
    public HomeStats homeStats;
	public UIManager uiManager;

    private void Start()
    {
        playerStats = ItemStat.instance.stats.playerStats;
        homeStats = ItemStat.instance.stats.homeStats;
    }

    public void BuyUpgrade(int btnIndex)
    {
        //  Upgrade Buttons
        switch (btnIndex)
        {
            case 0: // bullet speed
                UpgradeBulletSpeed();
                break;

            case 1: // reload time
                UpgradeReloadTime();
                break;

            case 2: // max bullet capacity
                UpgradeBulletCapacity();
                break;

            // buat beli weapon
            case 3:
            case 4:
            case 5:
                BuyWeapon(btnIndex - 3); 
                break;
            //upgrade building health
            case 6:
            case 7:
            case 8:
                UpgradeBuilding(btnIndex - 6);
                break;
            // buy time freeze skill
            case 9:
                BuySkillTimeSlow();
                break;
            case 10:
                BuySkillBomb();
                break;
            default:
                Debug.LogWarning("Index upgrade/senjata tidak dikenali!");
                break;
        }
    }

    private void BuyWeapon(int index)
    {
        if (index < 0 || index >= weaponDataList.Length)
        {
            Debug.LogWarning("Index senjata tidak valid.");
            return;
        }

        var weapon = weaponDataList[index];
        int price = weaponPrices[index];

        if (weapon.hasThisWeapon)
        {
            Debug.Log($"Senjata {weapon.weaponName} sudah dimiliki.");
            return;
        }

        if (playerStats.coin >= price)
        {
            playerStats.coin -= price;
            weapon.hasThisWeapon = true;
            Debug.Log($"Berhasil membeli senjata: {weapon.weaponName}");
        }
        else
        {
            Debug.Log("Koin tidak cukup untuk membeli senjata.");
        }
    }
    private void UpgradeBuilding(int index)
    {
        int[] upgradePrices = { 0, 300, 600 };
        float[] upgradeHealthLevels = { 100f, 175f, 250f };

        if (index < 0 || index >= upgradePrices.Length || index >= upgradeHealthLevels.Length)
        {
            Debug.LogWarning("Index upgrade bangunan tidak valid.");
            return;
        }

        if (homeStats.maxHealthHome >= upgradeHealthLevels[index])
        {
            Debug.Log("Upgrade ini sudah dilakukan.");
            return;
        }

        int price = upgradePrices[index];

        if (playerStats.coin >= price)
        {
            playerStats.coin -= price;
            homeStats.maxHealthHome = upgradeHealthLevels[index];
            homeStats.currentHealthHome = homeStats.maxHealthHome;

            Debug.Log($"Upgrade base ke level {index}. MaxHealthHome sekarang: {homeStats.maxHealthHome}");

            if (homeController != null)
                homeController.UpdateHomeSprite(index);
        }
        else
        {
            Debug.Log("Koin tidak cukup untuk upgrade bangunan.");
        }
    }
    private void UpgradeBulletCapacity()
    {
        int maxUpgrade = 5;
        int minCapacity = 10;
        int capacityPerUpgrade = 2;
        int maxCapacity = minCapacity + (maxUpgrade * capacityPerUpgrade);

        if (playerStats.coin >= 120 && playerStats.maxBulletCapacity < maxCapacity)
        {
            playerStats.coin -= 120;
            playerStats.maxBulletCapacity += capacityPerUpgrade;
            Debug.Log("Kapasitas peluru meningkat!");

            uiManager.GenerateBulletCapacityUI();
            uiManager.GenerateBulletsUI();
        }
        else if (playerStats.maxBulletCapacity >= maxCapacity)
        {
            Debug.Log("Sudah mencapai kapasitas maksimal.");
        }
        else
        {
            Debug.Log("Coin kurang untuk upgrade kapasitas peluru.");
        }
    }

    private void UpgradeBulletSpeed()
    {
        float maxBulletSpeed = 18f; 
        float step = 3f;

        if (playerStats.coin >= 100 && playerStats.bulletSpeed < maxBulletSpeed)
        {
            playerStats.coin -= 100;
            playerStats.bulletSpeed = Mathf.Min(playerStats.bulletSpeed + step, maxBulletSpeed);

            Debug.Log("Bullet speed ditingkatkan ke: " + playerStats.bulletSpeed);
            uiManager.GenerateBulletSpeedUI(); 
        }
        else
        {
            Debug.Log("Tidak bisa upgrade bullet speed lebih lanjut atau koin tidak cukup.");
        }
    }

    private void UpgradeReloadTime()
    {
        float minReloadTime = 0.5f;
        float step = 0.3f;

        if (playerStats.coin >= 150 && playerStats.reloadTime - step >= minReloadTime)
        {
            playerStats.coin -= 150;
            playerStats.reloadTime -= step;

            Debug.Log("Reload time upgraded to: " + playerStats.reloadTime);
            uiManager.GenerateReloadSpeedUI(); 
        }
        else
        {
            Debug.Log("Tidak bisa upgrade reload time lebih lanjut atau koin tidak cukup.");
        }
    }
    private void BuySkillTimeSlow()
    {
        var upgrade = ItemStat.instance.stats.itemUpgrade;

        if (upgrade.timeFrezee <= 0f)
        {
            int skillPrice = 400;

            if (playerStats.coin >= skillPrice)
            {
                playerStats.coin -= skillPrice;
                upgrade.timeFrezee = 1f;
                Debug.Log("Skill Time Freeze berhasil dibeli!");
            }
            else
            {
                Debug.Log("Koin tidak cukup untuk membeli skill Time Freeze.");
            }
        }
        else
        {
            Debug.Log("Skill Time Freeze sudah dimiliki.");
        }
    }

    private void BuySkillBomb()
    {
        var upgrade = ItemStat.instance.stats.itemUpgrade;
        if (upgrade.bomb <= 0)
        {
            int skillPrice = 200;
            if (playerStats.coin >= skillPrice)
            {
                playerStats.coin -= skillPrice;
                upgrade.bomb += 1;
                Debug.Log("Skill Bomb udah dibeli");
            }
            else
            {
                Debug.Log("Koin e kurang");
            }
        }
        else
        {
            Debug.Log("Udah punya skill bomb");
        }
    }

}
