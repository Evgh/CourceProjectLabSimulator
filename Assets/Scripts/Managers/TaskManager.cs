using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static event Action<int> TaskChanged;

    public static TaskManager Inst { get; private set; }

    [SerializeField] private List<string> _tasks;
    [SerializeField] private TMP_Text _taskText;

    private int _task = 0;
    private int _iteration = 0;

    public int TaskId
    {
        get => _task;
        private set
        {
            _task = value;
            TaskChanged?.Invoke(_task);
        }
    }

    private void Awake()
    {
        Inst = this;

        _taskText.text = _tasks[0];
    }

    public void TaskDone(int id)
    {
        if (TaskId != id) return;
        ++id;

        if (_tasks.Count < id + 1) return;

        var nextTask = _tasks[id];
        if (nextTask != null)
        {
            TaskId = id;

            _taskText.text = _tasks[id];
        }

        if(id == 3 && _iteration < 5)
        {
            _iteration++;
            TaskId = 1;
            _taskText.text = _tasks[TaskId];
        }
    }

    public bool CanTaskBeDone(int taskNum)
    {
        return taskNum == TaskId;
    }

    // УДАЛИ ЭТО КОГДА БУДЕШЬ ТАСКИ ДОБАВЛЯТЬ
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TaskDone(0);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TaskDone(1);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            TaskDone(2);
        }
    }
}