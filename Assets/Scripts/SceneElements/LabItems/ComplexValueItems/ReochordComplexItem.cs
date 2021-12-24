using System;
using TMPro;
using UnityEngine;

public class ReochordComplexItem : LabItem
{
    private const string l1Message = "L1 = ";
    private const string l2Message = "L2 = ";
    private const string sm = " sm";
    private const int _generalLength = 90;

    public static event Action<bool, IInteractable> ReochordFocused;

    public static event Action SaveState;
    public static event Action ResetToSavedState;
    public static event Action Reset;

    [SerializeField] private TMP_Text _leftValue;
    [SerializeField] private TMP_Text _rightValue;

    private bool _isFocused;
    private int _delta;
    private int _tempDelta;

    protected override void Awake()
    {
        base.Awake();
        _delta = 45;
        UpdateUiOutput();

        SaveState += () =>
        {
            _tempDelta = _delta;
        };

        ResetToSavedState += () =>
        {
            SetDeltaValue(_tempDelta);
            UpdateUiOutput();
        };

        Reset += () =>
        {
            SetDeltaValue(45);
            UpdateUiOutput();
        };
    }

    public override void RaiceFocusChanged(bool isFocused, IInteractable target)
    {
        _isFocused = isFocused;
        InteractionManager.Inst.OnFocusChanged(isFocused, target);
        ReochordFocused?.Invoke(isFocused, target);

        if (isFocused)
        {
            SaveState?.Invoke();
        }
    }

    public override void OnUse()
    {
        base.OnQuit();
        RaiceFocusChanged(false, this);

        TaskManager.Inst.TaskDone(2);
    }

    public override void OnQuit()
    {
        ResetToSavedState?.Invoke();
        base.OnQuit();
    }

    void Update()
    {

        if (_isFocused && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            OnLeftMove();
        }

        if (_isFocused && (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            OnRightMove();
        }
    }


    public void OnLeftMove()
    {
        if(_delta > 0)
        {
            _renderer.transform.position += new Vector3((float)-1.24 / 45, 0, 0);
            _delta--;
            UpdateUiOutput();
        }
    }

    public void OnRightMove()
    {
        if(_delta < _generalLength)
        {
            _renderer.transform.position += new Vector3((float)1.24 / 45, 0, 0);
            _delta++;
            UpdateUiOutput();
        }
    }

    #region Private Methods
    private void SetDeltaValue(int value)
    {
        if (value < 0 || value > 90)
            return;

        while (_delta != value)
        {
            if (_delta < value)
            {
                OnRightMove();
            }
            else
            {
                OnLeftMove();
            }
        }
    }

    private void UpdateUiOutput()
    {
        _leftValue.text = l1Message + _delta + sm;
        _rightValue.text = l2Message + (_generalLength - _delta) + sm;
    }
    #endregion
}
