using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonFocusItem : LabItem
{
    public override void RaiceFocusChanged(bool value, IInteractable sender)
    {
        return;
    }
}
