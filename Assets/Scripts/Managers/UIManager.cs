using UnityEngine;

public class UIManager : MonoBehaviour
{
    // default UI
    [SerializeField] private GameObject _hint;
    [SerializeField] private GameObject _cursor;

    // Flexable UI
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private GameObject _reochordPanel;
    [SerializeField] private GameObject _hintFlexible;

    void Awake()
    {
        LabItem.FocusChanged += OInIsFocusedChangedHandler;
        ShopComplexItem.ShopFocused += OnShopFocused;
        ReochordComplexItem.ReochordFocused += OnReochordFocused;
    }

    private void OInIsFocusedChangedHandler(bool isFocused, IInteractable target)
    {
        _hint.SetActive(isFocused);
        _cursor.SetActive(!isFocused);

        _shopPanel.SetActive(false);
        _reochordPanel.SetActive(false);
        _hintFlexible.SetActive(false);
    }

    private void OnShopFocused(bool isFocused, IInteractable target)
    {
        _hint.SetActive(false);
        _reochordPanel.SetActive(false);

        _shopPanel.SetActive(isFocused);
        _hintFlexible.SetActive(isFocused);
        _cursor.SetActive(!isFocused);
    }

    private void OnReochordFocused(bool isFocused, IInteractable target)
    {
        _hint.SetActive(false);
        _shopPanel.SetActive(false);

        _reochordPanel.SetActive(isFocused);
        _hintFlexible.SetActive(isFocused);
        _cursor.SetActive(!isFocused);
    }
}