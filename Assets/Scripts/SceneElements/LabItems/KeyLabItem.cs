using DG.Tweening;
using System;
using UnityEngine;

public class KeyLabItem : LabItem
{
    private const int unlockKeyTaskNum = 0;
    private const int lockKeyTaskNum = 4;

    public static event Action<bool> KeyChanged;

    [SerializeField] private GameObject _key;

    private bool _keyLocked;

    public override void OnUse()
    {
        Debug.Log("OnClick Key");

        if (!_keyLocked)
        {
            if (TaskManager.Inst.CanTaskBeDone(unlockKeyTaskNum))
            {
                _key.transform.DORotate(Vector3.up * -30f, 0.5f).SetRelative(true);
                //_key.transform.Rotate(Vector3.up, -30f);
                TaskManager.Inst.TaskDone(unlockKeyTaskNum);
                _keyLocked = true;

                KeyChanged?.Invoke(_keyLocked);
            }
        }
        else
        {
            if (TaskManager.Inst.CanTaskBeDone(lockKeyTaskNum))
            {
                _key.transform.DORotate(Vector3.up * 30f, 0.5f).SetRelative(true);
                TaskManager.Inst.TaskDone(lockKeyTaskNum);
                _keyLocked = false;

                KeyChanged?.Invoke(_keyLocked);
            }
        }
    }
}
