using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponData[] weapons; 
    public Transform firePoint;
    private int currentWeaponIndex = 0;
    private WeaponData CurrentWeapon => weapons[currentWeaponIndex];

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

        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        AimAtMouse();
        HandleShooting();
        HandleReload();
        HandleWeaponSwitch();
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
                Debug.Log("Out of Bullets!");
                isOutOfAmmo = true;
            }
        }
    }

    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentBullet < CurrentWeapon.maxBulletCapacity && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Ganti senjata dengan tombol Q
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
            EquipWeapon(currentWeaponIndex);
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
        {
            Debug.LogWarning("Index senjata tidak valid.");
            return;
        }

        currentBullet = weapons[index].maxBulletCapacity;
        isReloading = false;
        isOutOfAmmo = false;

        Debug.Log($"Weapon switched to: {weapons[index].name}");
    }

    private void Shoot()
    {
        Transform spawnPoint = firePoint != null ? firePoint : transform;
        GameObject bullet = Instantiate(CurrentWeapon.bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = spawnPoint.up * CurrentWeapon.bulletSpeed;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(CurrentWeapon.reloadTime);
        currentBullet = CurrentWeapon.maxBulletCapacity;
        isOutOfAmmo = false;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
