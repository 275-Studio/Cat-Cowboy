using System;
using Unity.VisualScripting;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public HomeStats homeStats;
    private void Start()
    {
        homeStats = ItemStat.instance.stats.homeStats;
        homeStats.currentHealthHome = homeStats.maxHealthHome;
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
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
