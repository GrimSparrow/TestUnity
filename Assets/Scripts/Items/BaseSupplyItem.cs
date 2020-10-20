using System.ComponentModel;
using UnityEngine;

[DisplayName("Комплектующие")]
public class BaseSupplyItem : BaseItem
{
    #region Fields

    [SerializeField]
    private int energyСonsumption;

    #endregion

    #region Properties

    public int EnergyСonsumption
    {
        get => energyСonsumption;
        set => energyСonsumption = value;
    }

    #endregion
}
