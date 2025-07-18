using System;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    public float homeHealth = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Barrel"))
        {
            homeHealth -= 10;
            Debug.Log(homeHealth);
            Destroy(other.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
