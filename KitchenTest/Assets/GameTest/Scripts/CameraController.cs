using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {

    //镜头移动
    public Transform player;
    private float mouseX, mouseY; //获取鼠标移动的值
    public float mouseSensitivity; //鼠标灵敏度
    public float xRotation;

    public bool CanCamera = true;

    public static CameraController Instance;
    private Vector3 CameraPos, CameraRot;

    private void Awake()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update () {

        if (CanCamera)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);

            player.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }

    }

    public void GetCamera()
    {
        CameraPos = this.transform.position;
        CameraRot = this.transform.eulerAngles;
    }

    public void SetCamera()
    {
        this.transform.DOMove(CameraPos, 0.3f);
        this.transform.DORotate(CameraRot, 0.3f);
    }
    
}