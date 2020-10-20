using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipViewer : MonoBehaviour
{
    #region Fields
    
    [SerializeField] 
    private GridLayoutGroup grid;

    [SerializeField] private GameObject container;

    private List<ShipSchemeGenerator> ships;
    
    private GameObject[,] slotGrid;
    
    private ShipSchemeGenerator shipScheme;

    private int counter = 0;
    
    #endregion

    #region Properties

    public GameObject[,] SlotGrid
    {
        get => slotGrid;
        set => slotGrid = value;
    }

    #endregion

    #region Main Functionality

    public void CreateSlots(ShipSchemeGenerator ship)
    {
        Clear();

        ShipController.Instance.CurrentShipScheme = ship;
        grid.constraintCount = ship.Width;
        slotGrid = ship.GenerateScheme(transform);

        Resize(ship);
        
        if (ship.Save != null && ship.Save.Count > 0)
        {
            ShipController.Instance.LoadSave(ship.Save);
        }
    }

    private void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Resize(ShipSchemeGenerator ship)
    {
        float width = container.GetComponent<RectTransform>().rect.width;
        var t = container.GetComponent<GridLayoutGroup>();
        var si = (width - t.padding.left * 2 - (ship.Width - 1) * t.spacing.x) / ship.Width;
        Vector2 newSize = new Vector2(si, si);
        container.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }

    #endregion
}
