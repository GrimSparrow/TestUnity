using System.ComponentModel;
using UnityEngine;

[DisplayName("Сталь")]
[CreateAssetMenu(fileName = "SteelArmor", menuName = "Items/Armors/Steel/Create Steel Armor")]
public class SteelArmor : BaseArmorItem
{
    #region Fields

    [SerializeField]
    private float toughness;

    #endregion

    #region Properties

    public float Toughness
    {
        get => toughness;
        set => toughness = value;
    }

    #endregion

    #region Main Functionality

    public override float CalculateDefense()
    {
        return Armor * Toughness;
    }

    #endregion
}
