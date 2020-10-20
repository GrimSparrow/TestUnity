using System.ComponentModel;
using UnityEngine;

[DisplayName("Баллистика")]
[CreateAssetMenu(fileName = "BallisticWeapon", menuName = "Items/Weapons/Create Ballistic Weapon")]
public class BallistickWeapon : BaseWeaponItem
{
    #region Fields

    [SerializeField]
    private float bulletSpeed;

    #endregion

    #region Properties

    public float BulletSpeed
    {
        get => bulletSpeed;
        set => bulletSpeed = value;
    }

    #endregion

    #region Main Functionality

    protected override float CalculateDamage()
    {
        return Damage * BulletSpeed;
    }

    #endregion
}
