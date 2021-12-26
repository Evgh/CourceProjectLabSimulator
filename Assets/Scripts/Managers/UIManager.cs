using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }

    // default UI
    [SerializeField] private GameObject _defaultHintLabMode;
    [SerializeField] private GameObject _defaultHint;
    [SerializeField] private GameObject _cursor;

    // Flexable UI
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _reochordPanel;
    [SerializeField] private GameObject _hintFlexible;

    private bool _isMouseLocked = true;
    private bool _isFocused = false;

    void Awake()
    {
        Inst = this;

        LabItem.FocusChanged += OInIsFocusedChangedHandler;
        ShopComplexItem.ShopFocused += OnShopFocused;
        ReochordComplexItem.ReochordFocused += OnReochordFocused;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeMouseLockedState();
        }
    }

    private void OInIsFocusedChangedHandler(bool isFocused, IInteractable target)
    {
        _isFocused = isFocused;

        _defaultHintLabMode.SetActive(isFocused);
        _defaultHint.SetActive(!isFocused);

        _hintFlexible.SetActive(false);
        _shopPanel.SetActive(false);
        _reochordPanel.SetActive(false);

        UpdateCursorStateOnFocusChanged();
    }

    private void OnShopFocused(bool isFocused, IInteractable target)
    {
        _isFocused = isFocused;

        _shopPanel.SetActive(isFocused);
        _hintFlexible.SetActive(isFocused);
        _defaultHint.SetActive(!isFocused);

        _defaultHintLabMode.SetActive(false);
        _reochordPanel.SetActive(false);

        UpdateCursorStateOnFocusChanged();
    }

    private void OnReochordFocused(bool isFocused, IInteractable target)
    {
        _isFocused = isFocused;

        _reochordPanel.SetActive(isFocused);
        _hintFlexible.SetActive(isFocused);
        _defaultHint.SetActive(!isFocused);

        _defaultHintLabMode.SetActive(false);
        _shopPanel.SetActive(false);

        UpdateCursorStateOnFocusChanged();
    }

    private void ChangeMouseLockedState()
    {
        if (!_isFocused)
        {
            _isMouseLocked = !_isMouseLocked;
            UpdateMouseLockedState();   
        }
    }

    private void UpdateCursorStateOnFocusChanged()
    {
        _isMouseLocked = !_isFocused;
        UpdateMouseLockedState();
    }

    private void UpdateMouseLockedState()
    {
        Cursor.lockState = _isMouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_isMouseLocked;

        _cursor.SetActive(_isMouseLocked);
    }
}