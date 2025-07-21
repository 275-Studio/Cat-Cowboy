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

            Destroy(gameObject);
        }
    }
}