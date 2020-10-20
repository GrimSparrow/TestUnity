using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void Callback();

public class ConfirmationWindow : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Button decline;
    
    [SerializeField]
    private Button accept;

    [SerializeField] 
    private TextMeshProUGUI message;

    private Callback callback;

    #endregion

    #region Unity Events

    void OnEnable()
    {
        decline.onClick.AddListener(Decline);
        accept.onClick.AddListener(Accept);
    }

    #endregion

    #region Main Functionality

    public void Initialize(string message, Callback callback = null)
    {
        this.callback = callback;
        this.message.SetText(message);
    }

    private void Accept()
    {
        callback?.Invoke();
        
        Destroy(gameObject);
    }
    
    private void Decline()
    {
        Destroy(gameObject);
    }

    #endregion
}
