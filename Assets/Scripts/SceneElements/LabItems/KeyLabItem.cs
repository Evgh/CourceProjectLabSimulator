using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLabItem : LabItem
{
    private const int taskNum = 0;

    [SerializeField] private GameObject _key;

    public override void OnUse()
    {
        Debug.Log("OnClick Key");

        if (TaskManager.Inst.CanTaskBeDone(taskNum))
        {
            _key.transform.DORotate(Vector3.up * -30f, 0.5f)
                .SetRelative(true);
            //_key.transform.Rotate(Vector3.up, -30f);
            TaskManager.Inst.TaskDone(taskNum);
        }
    }
}
