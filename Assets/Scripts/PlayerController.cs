using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Weapon Settings")]
    public WeaponData[] weapons; 
    public Transform firePoint;
    private int currentWeaponIndex = 0;
    private WeaponData CurrentWeapon => weapons[currentWeaponIndex];
    private PlayerStats playerStats;
    public UIManager bulletsUIManager; 

    [Header("Reloading State")]
    private bool isReloading = false;
    private int currentBullet;
    [SerializeField] private float pointerRadius = 2f;
    [SerializeField] private Transform pointer;
    [SerializeField] private SpriteRenderer pointerRenderer;

    private void Start()
    {
        if (weapons == null || weapons.Length == 0)
        {
            Debug.LogError("Tidak ada WeaponData yang di-assign di Inspector.");
            return;
        }

        playerStats = ItemStat.instance.stats.playerStats;
        UIManager bulletsUIManager = GetComponent<UIManager>();
        EquipWeapon(currentWeaponIndex);
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;
        AimAtMouse();
        HandleShooting();
    }

    private void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 direction = mousePosition - transform.position;
        transform.up = direction;
        if (pointer != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pointer.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void HandleShooting()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0) && currentBullet > 0 && !isReloading)
        {
            Shoot();
            currentBullet--;
            bulletsUIManager.RemoveBulletUI();
        }
    }

    public void HandleReload()
    {
        if (currentBullet < playerStats.maxBulletCapacity && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
            return;
        if (!weapons[index].hasThisWeapon)
        {
            Debug.Log("gak punya senjata ini!");
            return;
        }
        currentWeaponIndex = index;
        currentBullet = playerStats.maxBulletCapacity;
        isReloading = false;
        if (pointerRenderer != null && CurrentWeapon.weaponSprite != null)
        {
            pointerRenderer.sprite = CurrentWeapon.weaponSprite;
        }
    }


    public void HandleWeaponSwitch()
    {
        int startingIndex = currentWeaponIndex;
        do
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weapons.Length)
            {
                currentWeaponIndex = 0;
            }

            if (weapons[currentWeaponIndex].hasThisWeapon)
            {
                EquipWeapon(currentWeaponIndex);
                Debug.Log("Switched to weapon index: " + currentWeaponIndex);
                return;
            }
        } while (currentWeaponIndex != startingIndex);

        Debug.Log("Tidak ada senjata lain yang dimiliki.");
    }

    private void Shoot()
    {
        bulletsUIManager.RemoveBulletUI();
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
        isReloading = false;
        bulletsUIManager.GenerateBulletsUI();
        Debug.Log("Reloaded!");
    }
    public void ResetAmmoToMax()
    {
        currentBullet = playerStats.maxBulletCapacity;
        bulletsUIManager.GenerateBulletsUI();
    }
    
}
