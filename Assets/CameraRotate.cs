using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private Transform lookAtTransform;
    [SerializeField]
    private Transform rotateY;
    float y;
    [SerializeField]
    private Transform rotateX;
    float x;
    [SerializeField]
    private Transform zoomY;
    float z;

    //设置默认数值
    private float mouseX;
    private float mouseY = 0;
    private float mouseScrollWheel = -3.0f;

    private float t;
    void Start()
    {
        RotateX(mouseY);
        ZoomY(mouseScrollWheel);
    }


    void Update()
    {
        ControlCamera();
    }
    private void LateUpdate()
    {
        FollowTarget(cameraTransform, targetTransform);
        LookAtTarget(cameraTransform, targetTransform);
    }
    private void LookAtTarget(Transform transformCamera, Transform transformTarget)
    {
        transformCamera.LookAt(transformTarget);
    }
    private void FollowTarget(Transform transformCamera, Transform transformTarget)
    {
        transformCamera.position = transformTarget.position;
    }

    private void ControlCamera()
    {
        if (Input.GetMouseButton(0))
        {
            mouseX += Input.GetAxis("Mouse X");
            Debug.Log(mouseX);
            RotateY(mouseX);
            mouseY -= Input.GetAxis("Mouse Y");
            RotateX(mouseY);
        }
        mouseScrollWheel -= Input.GetAxis("Mouse ScrollWheel");
        //限制相机的位置范围，别钻进去
        mouseScrollWheel = Mathf.Clamp(mouseScrollWheel, -10.0f, -1.0f);
        ZoomY(mouseScrollWheel);
    }
    private void RotateY(float ry)
    {
        rotateY.localRotation = Quaternion.Euler(new Vector3(0, ry, 0));
    }
    private void RotateX(float rx)
    {
        rotateX.localRotation = Quaternion.Euler(new Vector3(rx, 0, 0));
    }
    private void ZoomY(float ry)
    {
        zoomY.localPosition = new Vector3(0, ry, 0);
    }
}