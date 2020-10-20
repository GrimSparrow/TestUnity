using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShipScheme", menuName = "Ships/CreateShip")]
public class ShipSchemeGenerator : ScriptableObject
{
    #region Fields

    [SerializeField] 
    private Texture2D schemeImage;
    
    [SerializeField] 
    private GameObject slotPrefab;
    
    [SerializeField]
    private List<SlotSaveData> save = new List<SlotSaveData>();

    #endregion

    #region Properties

    public int Width => schemeImage.width;
    
    public int Height => schemeImage.height;

    #endregion

    #region Simple Save

    public List<SlotSaveData> Save
    {
        get => save;
        set => save = value;
    }

    #endregion

    #region Main functionality

    public GameObject[,] GenerateScheme(Transform parent)
    {
        
        var slotGrid = new GameObject[schemeImage.width, schemeImage.height];;
        
        for (var x = 0; x < schemeImage.width; x++)
        {
            for (var y = 0; y < schemeImage.height; y++)
            {
                slotGrid[x,y] = GenerateCell(x, y, parent);
            }
        }

        return slotGrid;
    }

    private GameObject GenerateCell(int x, int y, Transform parent)
    {
        var pixelColor = schemeImage.GetPixel(y,x);
        var result = Instantiate(slotPrefab, parent, true);
        
        if (pixelColor.a == 0)
        {
            var slot = result.GetComponent<Slot>();
            slot.GetComponent<Image>().color = Color.clear;
            slot.IsBlocked = true;
        }

        result.GetComponent<RectTransform>().localScale = Vector3.one;
        result.transform.name = $"{x},{y}";
        result.GetComponent<Slot>().GridPos = new IntVector2(x, y);

        return result;
    }

    #endregion
}


