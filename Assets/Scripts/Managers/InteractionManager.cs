using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static event Action<bool, IInteractable> IsFocusedChanged;
    public static event Action<IInteractable> SelectedChanged;

    public static InteractionManager Inst { get; private set; }

    [SerializeField] private GameObject _cursor;

    private IInteractable _selected;
    private List<IInteractable> _items;

    private bool _isFocused;
    private bool _isMouseLocked = true;

    public bool IsFocused
    {
        get => _isFocused;
        set
        {
            if (_isFocused != value)
            {                
                _selected.RaiceFocusChanged(value, _selected);
            }
        }
    }

    public IInteractable Selected
    {
        get => _selected;
        set
        {
            if (_selected == value) return;

            _selected?.OnInteractStop();
            _selected = value;
            _selected?.OnInteractStart();

            SelectedChanged?.Invoke(_selected);
        }
    }

    public static List<IInteractable> Items => Inst._items;

    public void OnFocusChanged(bool value, IInteractable sender)
    {
        _isFocused = value;
        IsFocusedChanged?.Invoke(value, sender);
    }

    private void Awake()
    {
        Inst = this;

        _items = new List<IInteractable>();

        var allObjects = FindObjectsOfType<GameObject>();

        foreach (var item in allObjects)
        {
            if (item.TryGetComponent(out IInteractable interactable))
            {
                Items.Add(interactable);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Selected != null && !IsFocused)
            {
                IsFocused = true;
            }
        }

        if (IsFocused && Input.GetKeyDown(KeyCode.E))
        {
            Selected?.OnUse();
        }

        if (IsFocused && Input.GetKeyDown(KeyCode.Q))
        {
            IsFocused = false;
            Selected?.OnQuit();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UpdateMouseLocked();
        }
    }

    private void UpdateMouseLocked()
    {
        _isMouseLocked = !_isMouseLocked;

        Cursor.lockState = _isMouseLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_isMouseLocked;

        _cursor.SetActive(_isMouseLocked);
    }
}