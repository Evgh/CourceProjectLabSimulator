public interface IInteractable
{
    public string ItemName { get; set; }

    public void OnInteractStart();
    public void OnInteractStop();
    public void OnUse();
    public void OnQuit();
    public void RaiceFocusChanged(bool isFocused, IInteractable target);
}