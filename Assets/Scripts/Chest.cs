using UnityEngine;

public class Chest : MonoBehaviour
{
    public int getCoin = 10;
    private PlayerStats playerStats;

    public void TakeHit()
    {
        playerStats = ItemStat.instance.stats.playerStats;
        playerStats.coin += getCoin;
        Destroy(gameObject);
    }
}
