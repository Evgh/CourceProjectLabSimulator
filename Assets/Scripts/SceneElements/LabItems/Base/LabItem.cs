using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
public class LabItem : MonoBehaviour, IInteractable
{
    public static event Action<bool, IInteractable> FocusChanged;
    public static LabItem Inst { get; private set; }


    protected MeshRenderer _renderer;
    private Color _startColor;


    [SerializeField] private string _itemName;
    public string ItemName { get => _itemName; set => _itemName = value; }

    protected virtual void Awake()
    {
        Inst = this;

        _renderer = GetComponent<MeshRenderer>();
        _startColor = _renderer.material.color;
    }

    public virtual void OnInteractStart()
    {
        Debug.Log("InteractStart");
        _renderer.material.color = Color.red;
    }

    public virtual void OnInteractStop()
    {
        Debug.Log("InteractStop");
        _renderer.material.color = _startColor;
    }

    public virtual void OnUse()
    {
        Debug.Log("OnClick");
    }

    public virtual void OnQuit()
    {
        Debug.Log("OnQuit");
    }

    public virtual void RaiceFocusChanged(bool value, IInteractable sender)
    {
        Debug.Log("Focused");
        InteractionManager.Inst.OnFocusChanged(value, sender);

        FocusChanged?.Invoke(value, sender);
    }
}