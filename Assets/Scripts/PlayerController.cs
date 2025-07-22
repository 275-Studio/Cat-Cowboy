using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponData[] weapons; 
    public Transform firePoint;
    private int currentWeaponIndex = 0;
    private WeaponData CurrentWeapon => weapons[currentWeaponIndex];
    private PlayerStats playerStats;

    [Header("Reloading State")]
    private bool isReloading = false;
    private bool isOutOfAmmo = false;
    private int currentBullet;

    private void Start()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Tidak ada WeaponData yang di-assign di Inspector.");
            return;
        }

        playerStats = ItemStat.instance.stats.playerStats;

        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        AimAtMouse();
        HandleShooting();
        HandleReload();
    }

    private void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        transform.up = direction;
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && currentBullet > 0 && !isReloading)
        {
            Shoot();
            currentBullet--;

            if (currentBullet == 0 && !isOutOfAmmo)
            {
                isOutOfAmmo = true;
            }
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentBullet < playerStats.maxBulletCapacity && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }
    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
        {
            return;
        }

        currentBullet = playerStats.maxBulletCapacity;
        isReloading = false;
        isOutOfAmmo = false;
    }

    private void Shoot()
    {
        Transform spawnPoint = firePoint != null ? firePoint : transform;

        if (currentWeaponIndex == 1)
        {
            float spreadAngle = 15f;
            int bulletCount = 3;

            float startAngle = -spreadAngle;
            float angleStep = spreadAngle;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, angle) * spawnPoint.rotation;

                GameObject bullet = Instantiate(CurrentWeapon.bulletPrefab, spawnPoint.position, rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = bullet.transform.up * playerStats.bulletSpeed;
                }
            }
        }
        else
        {
            GameObject bullet = Instantiate(CurrentWeapon.bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = spawnPoint.up * playerStats.bulletSpeed;
            }
        }
    }


    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(ItemStat.instance.stats.playerStats.reloadTime);
        currentBullet = playerStats.maxBulletCapacity;
        isOutOfAmmo = false;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
