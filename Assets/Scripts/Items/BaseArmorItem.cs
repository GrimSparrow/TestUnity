using System.ComponentModel;
using UnityEngine;

[DisplayName("Защита")]
public class BaseArmorItem : BaseItem
{
    #region Fields

    [SerializeField]
    private int armor;

    #endregion

    #region Properties

    public int Armor
    {
        get => armor;
        set => armor = value;
    }

    #endregion

    #region Main Functionality

    public virtual float CalculateDefense()
    {
        return armor;
    }

    #endregion
}
