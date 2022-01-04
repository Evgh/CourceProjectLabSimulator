using System;
using UnityEngine;

public class FlyingCamera : MonoBehaviour
{
    public static event Action<bool> FlyingCameraDisabled;

    [SerializeField] Transform targetPos;

    [SerializeField] GameObject _controller;
    [SerializeField] GameObject _taskPanel;
    [SerializeField] GameObject _cursor;
    [SerializeField] GameObject _cursorHelpPanel;
    [SerializeField] GameObject _goToLabPanel;
    [SerializeField] GameObject _dropDown;
    [SerializeField] GameObject _tableButton;


    int sensivity = 6;
    public float scrollSpeed = 10;
    public float minDistance = 1;
    public float maxDistance = 79;

    private float minY = 1.2f;
    private float maxY = 3;
    private float minX = 0;
    private float maxX = 3.8f;
    private float minZ = -8.5f;
    private float maxZ = -1.6f;

    private bool isEnabled = true;


    bool ControlDistance(float distance)
    {
        if (distance > 1 && distance < 79)
            return true;
        return false;
    }

    Vector3 CorrectPosition(Vector3 newPosition)
    {
        if (newPosition.x < minX)
            newPosition.x = minX;

        if (newPosition.x > maxX)
            newPosition.x = maxX;

        if (newPosition.y < minY)
            newPosition.y = minY;

        if (newPosition.y > maxY)
            newPosition.y = maxY;

        if (newPosition.z > maxZ)
            newPosition.z = maxZ;

        if (newPosition.z < minZ)
            newPosition.z = minZ;

        return newPosition;
    }

    void Update()
    {
        //move Camera
        float moveKeysAD = Input.GetAxis("Horizontal");
        float moveKeysWS = Input.GetAxis("Vertical");
        if ((moveKeysAD != 0 || moveKeysWS != 0) && isEnabled)
        {
            Vector3 newpos = transform.position +
            (transform.TransformDirection(new Vector3(moveKeysAD, 0, 0)) + Vector3.up * moveKeysWS) / sensivity;
            if (ControlDistance(Vector3.Distance(newpos, targetPos.position)))
                transform.position = CorrectPosition(newpos);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && isEnabled)
        {
            Vector3 newpos = transform.position +
            transform.TransformDirection(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * scrollSpeed);
            if (ControlDistance(Vector3.Distance(newpos, targetPos.position)))
                transform.position = CorrectPosition(newpos);
        }

        //rotate around
        if (Input.GetMouseButton(1) && isEnabled)
        {
            transform.RotateAround(targetPos.position, Vector3.up, Input.GetAxis("Mouse X") * sensivity);
            transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * sensivity);

            transform.position = CorrectPosition(transform.position);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            _controller.SetActive(true);
            _cursorHelpPanel.SetActive(true);
            _cursor.SetActive(true);
            _taskPanel.SetActive(true);
            _dropDown.SetActive(true);
            _tableButton.SetActive(true);

            _goToLabPanel.SetActive(false);
            gameObject.SetActive(false);
            isEnabled = false;

            FlyingCameraDisabled?.Invoke(isEnabled);
        }
    }
}
