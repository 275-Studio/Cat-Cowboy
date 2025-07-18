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
		if(other.gameObject.CompareTag("Barrel")){
			Destroy(other.gameObject);
		}
    }
}
