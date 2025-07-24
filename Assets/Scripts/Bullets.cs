using System;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Barrel"))
        {
            Barrel barrel = other.gameObject.GetComponent<Barrel>();
            if (barrel != null)
            {
                barrel.TakeHit();
            }

            ItemStat.instance.stats.playerStats.destroyedBarrels += 50/2 * 3;

            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("chest"))
        {
            Chest chest = other.gameObject.GetComponent<Chest>();
            if (chest != null)
            {
                chest.TakeHit();
            }
            Destroy(gameObject);
        }
    }
}