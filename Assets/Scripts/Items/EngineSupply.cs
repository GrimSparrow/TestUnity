using System.ComponentModel;
using UnityEngine;

[DisplayName("Двигатели")]
[CreateAssetMenu(fileName = "Engine", menuName = "Items/Supply/Create Engine")]
public class EngineSupply : BaseSupplyItem
{
    #region Fields

    [SerializeField]
    private float rotationSpeed;

    #endregion

    #region Properties

    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }

    #endregion
}
