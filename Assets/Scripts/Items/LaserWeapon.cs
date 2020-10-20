using System.ComponentModel;
using UnityEngine;

[DisplayName("Лазеры")]
[CreateAssetMenu(fileName = "laserWeapon", menuName = "Items/Weapons/Create Laser Weapon")]
public class LaserWeapon : BaseWeaponItem
{
    #region Fields

    [SerializeField]
    private float shootDuration;

    #endregion

    #region Properties

    public float ShootDuration
    {
        get => shootDuration;
        set => shootDuration = value;
    }

    #endregion

    #region Main Functionality

    protected override float CalculateDamage()
    {
        return Damage * ShootDuration;
    }

    #endregion
}
