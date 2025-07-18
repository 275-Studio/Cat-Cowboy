using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void Start()
    {
        StartCoroutine(spawnBullets());
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    IEnumerator spawnBullets()
    {
        yield return new WaitForSeconds(2f);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = transform.up * 5;
        StartCoroutine(spawnBullets());
    }
}
