using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltmeterArrowLabItem : MonoBehaviour
{
    private bool _isKeyLocked = false;

    private float _zeroRotation = 48;
    private float _maxRotation = -48;
    private Tween _moveTween;

    void Awake()
    {
        KeyLabItem.KeyChanged += OnKeyChanged;
        TableManager.VoltmeterValueChanged += OnVoltmeterValueChanged;
    }


    private void OnKeyChanged(bool value)
    {
        if (value)
        {
            _isKeyLocked = value;
            OnVoltmeterValueChanged(80);
        }
        else
        {
            OnVoltmeterValueChanged(0);
        }
        _isKeyLocked = value;
    }

    private void OnVoltmeterValueChanged(float value)
    {
        if (value > 100 || value < 0)
            return;

        float delta;
        if (_isKeyLocked)
        {
            _moveTween?.Complete();

            value = 100 - value;

            //var delta = (_zeroRotation - _maxRotation) * (100 - value) * 0.1;

            if(value > 50)
            {
                value -= 50;
                delta = _zeroRotation * value*2 * 0.01f;
                
            }
            else if(value < 50)
            {
                delta = _maxRotation * (50 - value)*2 * 0.01f;
            }
            else
            {
                delta = 0;
            }

            _moveTween = transform.DORotate(Vector3.forward * (float)delta, 1.1f).SetEase(Ease.InElastic);
        }
    }

}
