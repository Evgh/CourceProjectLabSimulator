using UnityEngine;

[RequireComponent(typeof(InteractionManager), typeof(FirstPersonController))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _focusCam;
    [SerializeField] private Camera _shopCam;

    private Camera _mainCamera;
    private InteractionManager _interaction;
    private FirstPersonController _controller;

    public InteractionManager Interaction => _interaction;

    private void Awake()
    {
        _interaction = GetComponent<InteractionManager>();
        _controller = GetComponent<FirstPersonController>();
        _mainCamera = Camera.main;

        InteractionManager.IsFocusedChanged += OnIsFocusedChangedHandler;
        ShopComplexItem.ShopFocused += OnShopFocused;
    }

    private void Update()
    {
        if (!_interaction.IsFocused)
        {
            UpdateSelected();
        }
    }

    private void UpdateSelected()
    {
        var cam = Camera.main;

        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var interactable = hit.transform.GetComponent<IInteractable>();

            // Can be null
            _interaction.Selected = interactable;

            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
    }

    private void OnIsFocusedChangedHandler(bool isFocused, IInteractable interactable)
    {
        _controller.enabled = !isFocused;
        _mainCamera.gameObject.SetActive(!isFocused);
        _focusCam.gameObject.SetActive(isFocused);

        var pos = ((MonoBehaviour)interactable).transform.position;
        _focusCam.transform.position = pos + Vector3.back * 1.5f + Vector3.up * 1.5f;
        _focusCam.transform.LookAt(pos);
    }

    private void OnShopFocused(bool isFocused, IInteractable interactable)
    {
        _shopCam.gameObject.SetActive(isFocused);

        _controller.enabled = !isFocused;
        _mainCamera.gameObject.SetActive(!isFocused);
        _focusCam.gameObject.SetActive(false);
    }
}

