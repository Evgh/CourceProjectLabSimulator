using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class ReochordComplexItem : LabItem, ISaveable
{
    public static event Action<bool, IInteractable> ReochordFocused;
    public static event Action<int, int> LengthChanged;
    public static new ReochordComplexItem Inst { get; private set; }

    private const string l1Message = "L1 = ";
    private const string l2Message = "L - L1 = ";
    private const string sm = " sm";
    private const int _generalLength = 90;

    public event Action SaveState;
    public event Action ResetToSavedState;
    public event Action Reset;

    [SerializeField] private TMP_Text _leftValue;
    [SerializeField] private TMP_Text _rightValue;

    private bool _isFocused;
    private int _delta;
    private int _tempDelta;

    private Tween _moveTween;

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

    void Update()
    {

        if (_isFocused && (Input.GetKey(KeyCode.Comma) || Input.GetKey(KeyCode.LeftArrow)))
        {
            OnLeftMove();
        }

        if (_isFocused && (Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.RightArrow)))
        {
            OnRightMove();
        }
    }


    #region Public Overriden Methods
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
        LengthChanged?.Invoke(_delta, _generalLength - _delta);
    }

    public override void OnQuit()
    {
        ResetToSavedState?.Invoke();
        LengthChanged?.Invoke(_delta, _generalLength - _delta);
        base.OnQuit();
    }

    #endregion

    #region Public Methods

    public void OnLeftMove()
    {
        if (_delta > 0)
        {
            //_renderer.transform.position += new Vector3((float)-1.24 / 45, 0, 0);
            _moveTween?.Complete();
            _moveTween = transform.DOMove(Vector3.right * ((float)-1.24 / 45f), 0.1f)
                .SetEase(Ease.Linear)
                .SetRelative(true);

            _delta--;
            LengthChanged?.Invoke(_delta, _generalLength - _delta);
            UpdateUiOutput();
        }
    }

    public void OnRightMove()
    {
        if (_delta < _generalLength)
        {
            //_renderer.transform.position += new Vector3((float)1.24 / 45, 0, 0);
            _moveTween?.Complete();
            _moveTween = transform.DOMove(Vector3.right * ((float)1.24 / 45f), 0.1f)
                .SetEase(Ease.Linear)
                .SetRelative(true);
            _delta++;
            LengthChanged?.Invoke(_delta, _generalLength - _delta);
            UpdateUiOutput();
        }
    }
    #endregion

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
