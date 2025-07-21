using System;
using Unity.VisualScripting;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public HomeStats homeStats;
    private void Start()
    {
        homeStats = ItemStat.instance.stats.homeStats;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            homeStats.maxHealthHome -= 10;
            Debug.Log(homeStats.maxHealthHome);
            Destroy(other.gameObject);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
