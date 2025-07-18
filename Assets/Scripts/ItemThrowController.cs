using System.Collections;
using UnityEngine;

public class ItemThrowController : MonoBehaviour
{
    public GameObject ItemThrow;
    public Transform[] spawnPoint;
    public Transform playerPoint;

    void Start()
    {
        StartCoroutine(spawnRandom());        
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    IEnumerator spawnRandom()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            int randomRange = Random.Range(0, spawnPoint.Length);
            Transform spawnPoints = spawnPoint[randomRange];

            GameObject itemThrow = Instantiate(ItemThrow, spawnPoints.position, Quaternion.identity);

            Vector2 direction = (playerPoint.position - spawnPoints.position).normalized;

            Rigidbody2D rb = itemThrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * 5f; 
            }
        }
    }
}