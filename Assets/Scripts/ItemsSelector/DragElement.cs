using UnityEngine;
using UnityEngine.EventSystems;

public class DragElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Fields

    [SerializeField]
    private Sprite mainImage;

    [SerializeField]
    private IntVector2 itemSize;
    
    [SerializeField]
    private Transform defaultParentTransform;

    [SerializeField]
    private BaseItem item;
    
    private Transform dragParentTransform;
    
    private int siblingIndex;

    private CanvasGroup canvasGroup;

    private GameObject selected;

    #endregion

    #region Properties

    public Sprite MainImage
    {
        get => mainImage;
        set => mainImage = value;
    }

    public IntVector2 ItemSize
    {
        get => itemSize;
        set => itemSize = value;
    }

    public Transform DefaultParentTransform
    {
        get => defaultParentTransform;
        set
        {
            if (value != null)
            {
                defaultParentTransform = value;
            }
        }
    }

    public Transform DragParentTransform
    {
        get => dragParentTransform;
        set
        {
            if (value != null)
            {
                dragParentTransform = value;
            }
        }
    }

    public int SiblingIndex
    {
        get => siblingIndex;
        set
        {
            if (value > 0)
            {
                siblingIndex = value;
            }
        }
    }

    public BaseItem Item
    {
        get => item;
        set => item = value;
    }

    #endregion

    #region Unity Events

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    #endregion

    #region Drag Events

    public void OnBeginDrag(PointerEventData eventData)
    {
        ShipController.ItemSize = itemSize;
        canvasGroup.blocksRaycasts = false;
        selected = Instantiate(gameObject, DragParentTransform, true);
        ShipController.SelectedItem = selected;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
        selected.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(selected);
        ShipController.ItemSize = IntVector2.zero;
        ShipController.SelectedItem = null;
        canvasGroup.blocksRaycasts = true;
    }

    #endregion
}
