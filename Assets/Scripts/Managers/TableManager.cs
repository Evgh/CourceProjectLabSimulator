using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    public static TableManager Inst { get; private set; }

    [SerializeField] private GameObject _tableImage;
    [SerializeField] TMP_Text _tableButtonText;
    [SerializeField] TMP_Text _tableMainCapacity;

    [SerializeField] List<TMP_Text> _l1Inputs;
    [SerializeField] List<TMP_Text> _l2Inputs;
    [SerializeField] List<TMP_Text> _c0Inputs;
    [SerializeField] List<TMP_Text> _cXInputs;

    private bool _isTableShown = false;
    private int _labCapacity = 6318;

    public int CurrentVoltmeterValue { get; set; }

    private void Awake()
    {
        Inst = this;

        ClearAllInputs();

        ShopComplexItem.CapacityChanged += OnCapacityChanged;
        ReochordComplexItem.LengthChanged += OnLengthChanged;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnTableButtonClick();
        }
    }

    #region Public methods
    public void OnTableButtonClick()
    {
        _isTableShown = !_isTableShown;

        if (_isTableShown)
        {
            ShowTable();
            _tableButtonText.text = "Показать таблицу (нажмите <b>[T]</b>)";
        }
        else
        {
            HideTable();
            _tableButtonText.text = "Показать таблицу (нажмите <b>[T]</b>)";
        }
    }

    public void ShowTable()
    {
        _tableImage.SetActive(true);
    }

    public void HideTable()
    {
        _tableImage.SetActive(false);
    }
    #endregion

    #region Private Methods 
    private void OnCapacityChanged(int capacityValue)
    {
        _c0Inputs[TaskManager.Inst.Iteration].text = capacityValue.ToString();
        UpdateCapacityValues();
    }

    private void OnLengthChanged(int l1, int l2)
    {
        _l1Inputs[TaskManager.Inst.Iteration].text = l1.ToString();
        _l2Inputs[TaskManager.Inst.Iteration].text = l2.ToString();
        UpdateCapacityValues();
    }

    private void UpdateCapacityValues()
    {
        for(int i = 0; i < 3; i++)
        {
            double l1, l2, c0; 

            if (double.TryParse(_l1Inputs[i].text, out l1) && double.TryParse(_l2Inputs[i].text, out l2) && double.TryParse(_c0Inputs[i].text, out c0))
            {
                try
                {
                    var value = Math.Round(c0 * l2 / l1, 3);
                    _cXInputs[i].text = value.ToString();
                }
                catch
                {
                    _cXInputs[i].text = "-";
                }
            }
        }

        double c1, c2, c3;
        if(double.TryParse(_l1Inputs[0].text, out c1) && double.TryParse(_l2Inputs[1].text, out c2) && double.TryParse(_c0Inputs[2].text, out c3))
        {
            try
            {
                var value = Math.Round(c1+c2+c3/ 3, 3);
                _tableMainCapacity.text = value.ToString();
            }
            catch
            {
                _tableMainCapacity.text = "-";
            }
        }
    }

    private void ClearAllInputs()
    {
        foreach(var l1 in _l1Inputs)
            l1.text = string.Empty;

        foreach (var l2 in _l2Inputs)
            l2.text = string.Empty;

        foreach (var c0 in _c0Inputs)
            c0.text = string.Empty;

        foreach (var cx in _cXInputs)
            cx.text = string.Empty;

        _tableMainCapacity.text = string.Empty;
    }

    #endregion

}
