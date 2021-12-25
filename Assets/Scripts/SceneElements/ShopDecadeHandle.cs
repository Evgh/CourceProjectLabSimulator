using System;
using UnityEngine;

public class ShopDecadeHandle : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Color _startColor;
    private Quaternion _startRotation;
    private Quaternion _tempRotation;
    private int _tempValue;
    private bool _isSelected;
    private int _value;

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            _renderer.material.color = _isSelected ? Color.green : _startColor;
        }
    }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _startColor = _renderer.material.color;
        _startRotation = transform.rotation;

        ShopComplexItem.Inst.SaveState += SaveState;
        ShopComplexItem.Inst.ResetToSavedState += ApplySavedState;
        ShopComplexItem.Inst.Reset += ResetChanges;
        ShopComplexItem.Inst.UnselectAll += ClearSelection;
    }


    public int Increace()
    {
        if (_value < 9)
        {
            _value++;
            transform.Rotate(Vector3.left, 22.4f);

            if (_value == 6) // костыль, чтобы не переделывать модели
                transform.Rotate(Vector3.left, 22.4f);

            return 1;
        }
        return 0;
    }

    public int Decrease()
    {
        if (_value > 0)
        {
            _value--;
            transform.Rotate(Vector3.right, 22.4f);

            if (_value == 6) // костыль, чтобы не переделывать модели
                transform.Rotate(Vector3.right, 22.4f);

            return 1;
        }
        return 0;
    }

    private void SaveState()
    {
        _tempRotation = _renderer.transform.rotation;
        _tempValue = _value;
    }

    private void ApplySavedState()
    {
        _renderer.transform.rotation = _tempRotation;
        _value = _tempValue;
    }

    private void ResetChanges()
    {
        _renderer.transform.rotation = _startRotation;
        _value = 0;
    }

    private void ClearSelection()
    {
        IsSelected = false;
    }
}