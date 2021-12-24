using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class LabItemDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;
    private bool _isPushingEvent = true;

    private void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        InteractionManager.IsFocusedChanged += OnIsFocusedChangedHandler;
        InteractionManager.SelectedChanged += OnSelectedChangedHandler;

        _dropdown.onValueChanged.AddListener(OnValueChangedHandler);
    }


    private void Start()
    {
        InteractionManager.Items.ForEach(item => _dropdown.options.Add(new TMP_Dropdown.OptionData(item.ItemName)));
    }

    private void OnIsFocusedChangedHandler(bool isFocused, IInteractable _)
    {
        _dropdown.interactable = !isFocused;
    }

    private void OnSelectedChangedHandler(IInteractable obj)
    {
        if (obj == null)
        {
            _dropdown.value = 0;
            return;
        }

        var name = obj.ItemName;
        for (int i = 0; i < _dropdown.options.Count; i++)
        {
            if (_dropdown.options[i].text == name)
            {
                _dropdown.value = i;
                return;
            }
        }

        _dropdown.value = 0;
    }

    private void OnValueChangedHandler(int idx)
    {
        
    }
}