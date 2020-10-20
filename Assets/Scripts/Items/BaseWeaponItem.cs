using System.ComponentModel;
using UnityEngine;

[DisplayName("Оружие")]
public class BaseWeaponItem : BaseItem
{
    #region Fields

    [SerializeField]
    private float damage;

    #endregion

    #region Properties

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    #endregion

    #region MainFunctionality

    protected virtual float CalculateDamage()
    {
        return Damage;
    }

    #endregion
}
