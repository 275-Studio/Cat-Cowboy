using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public float bulletSpeed;
    public int maxBulletCapacity;
    public float reloadTime;
    public GameObject bulletPrefab;
}
