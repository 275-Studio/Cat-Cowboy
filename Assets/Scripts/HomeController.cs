using System;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public HomeStats homeStats;

    [Header("Home Level Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite[] homeLevelSprites;

    private void Start()
    {
        homeStats = ItemStat.instance.stats.homeStats;
        homeStats.currentHealthHome = homeStats.maxHealthHome;
    }
    public void UpdateHomeSprite(int index)
    {
        if (index >= 0 && index < homeLevelSprites.Length)
        {
            Sprite newSprite = homeLevelSprites[index];
            if (newSprite != null)
            {
                Debug.Log("Mengganti sprite ke: " + newSprite.name);
                spriteRenderer.sprite = newSprite;

                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    collider.size = newSprite.bounds.size;
                    collider.offset = newSprite.bounds.center;
                }
            }
            else
            {
                Debug.LogWarning("Sprite level " + index + " kosong.");
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            homeStats.currentHealthHome -= 10;
            Debug.Log(homeStats.currentHealthHome);
            Destroy(other.gameObject);
        }
    }
}