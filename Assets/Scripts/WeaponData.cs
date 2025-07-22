using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Data/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public GameObject bulletPrefab;
    public bool hasThisWeapon;
}
