using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class ShopComplexItem : LabItem, ISaveable
{
    public static event Action<bool, IInteractable> ShopFocused;
    public static event Action<int> CapacityChanged;
    public static new ShopComplexItem Inst { get; private set; }

    private const string message = "������� ��������: ";

    public event Action SaveState;
    public event Action ResetToSavedState;
    public event Action Reset;
    public event Action UnselectAll;

    [SerializeField] private List<ShopDecadeHandle> _handles;
    [SerializeField] private TMP_Text _currentValueText;

    private bool _isFocused;
    private int _currentValue;
    private int _tempValue;
    private int _currentIndex;

    protected override void Awake()
    {
        base.Awake();

        ShopComplexItem.Inst = this;
        _currentIndex = 0;
        _currentValue = 0;
        UpdateUiOutput();

        SaveState += () => 
        { 
            _tempValue = _currentValue; 
        };

        ResetToSavedState += () =>
        {
            _currentValue = _tempValue;
            UpdateUiOutput();
        };

        Reset += () =>
        {
            _currentValue = 0;
            UpdateUiOutput();
        };
    }

    private void Update()
    {
        if(_isFocused && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            OnSelectNextDecadeHandle();
        }

        if(_isFocused && (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            OnSelectPrevDecadeHandle();
        }

        if(_isFocused && (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            OnIncreaceCurrectHandleValue();
        }

        if (_isFocused && (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            OnDecreaceCurrectHandleValue();
        }
    }

    #region Overriden Methods
    public override void RaiceFocusChanged(bool value, IInteractable sender)
    {
        _isFocused = value;
        InteractionManager.Inst.OnFocusChanged(value, sender);
        ShopFocused?.Invoke(value, sender);

        UnselectAll?.Invoke();
        if (value)
        {
            SaveState?.Invoke();

            _handles[_currentIndex].IsSelected = true;
            base.OnInteractStop(); // �������, ����� ������� ���������� ����
        }
    }

    public override void OnUse()
    {
        base.OnQuit();
        RaiceFocusChanged(false, this);

        TaskManager.Inst.TaskDone(1);
        CapacityChanged?.Invoke(_currentValue);
    }

    public override void OnQuit()
    {
        ResetToSavedState?.Invoke();
        base.OnQuit();
    }

    #endregion

    #region Public Methods
    public void OnSelectNextDecadeHandle()
    {
        UnselectAll?.Invoke();
        _currentIndex = ++_currentIndex % _handles.Count;
        _handles[_currentIndex].IsSelected = true;
    }

    public void OnSelectPrevDecadeHandle()
    {
        UnselectAll?.Invoke();
        _currentIndex = _currentIndex - 1 >= 0 ? --_currentIndex : _handles.Count - 1;
        _handles[_currentIndex].IsSelected = true;
    }

    public void OnIncreaceCurrectHandleValue()
    {
        _currentValue += _handles[_currentIndex].Increace() * (int)Math.Pow(10, _currentIndex);
        UpdateUiOutput();
    }

    public void OnDecreaceCurrectHandleValue()
    {
        _currentValue -= _handles[_currentIndex].Decrease() * (int)Math.Pow(10, _currentIndex);
        UpdateUiOutput();
    }

    #endregion

    #region Private Methods
    private void UpdateUiOutput()
    {
        _currentValueText.text = message + _currentValue + " F";
    }
    #endregion
}
