using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonHandler : MonoBehaviour
{

    private Button button;
    private WeaponData CurrentWeapon;

    public void BuyUpgrade(int btnIndex)
    {
        var playerStats = ItemStat.instance.stats.playerStats;
        var homeStats = ItemStat.instance.stats.homeStats;

        switch (btnIndex)
        {
            //button bullet speed
            case 0:
                if (playerStats.coin >= 100)
                {
                    playerStats.coin -= 100;
                    playerStats.bulletSpeed += 3;
                }
                else
                {
                    Debug.Log("Coin kurang untuk upgrade max home health.");
                }
                break;
            //button reload time 
            case 1:
                if (playerStats.coin >= 150)
                {
                    playerStats.coin -= 150;
                    playerStats.reloadTime -= 0.5f;
                }
                else
                {
                    Debug.Log("Coin kurang untuk upgrade armor.");
                }

                break;
            //button capacity
            case 2:
                if (playerStats.coin >= 120)
                {
                    playerStats.coin -= 120;
                    playerStats.maxBulletCapacity += 2;
                    Debug.Log("Reload speed meningkat!");
                }
                else
                {
                    Debug.Log("Coin kurang untuk upgrade reload speed.");
                }
                break;
            case 3:
                if (playerStats.coin >= 250)
                {
                    playerStats.coin -= 250;
                    
                }
                break;

            default:
                Debug.LogWarning("Index upgrade tidak dikenali!");
                break;
        }
    }
}
