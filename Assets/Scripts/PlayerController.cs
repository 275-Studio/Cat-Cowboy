using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    private const int maxBulletCapacity = 10;
    public int bulletCapacity;

    [Header("Reloading")]
    private bool isReloading = false;
    private bool isOutOfAmmo = false;

    private void Start()
    {
        bulletCapacity = maxBulletCapacity;
    }

    private void Update()
    {
        AimAtMouse();
        HandleShooting();
        HandleReload();
    }

    private void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && bulletCapacity > 0 && !isReloading)
        {
            Shoot();
            bulletCapacity--;

            if (bulletCapacity == 0 && !isOutOfAmmo)
            {
                Debug.Log("Out of Bullets!");
                isOutOfAmmo = true;
            }
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletCapacity < maxBulletCapacity && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        Transform spawnPoint = firePoint != null ? firePoint : transform;
        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = spawnPoint.up * bulletSpeed;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(1f);
        bulletCapacity = maxBulletCapacity;
        isOutOfAmmo = false;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
