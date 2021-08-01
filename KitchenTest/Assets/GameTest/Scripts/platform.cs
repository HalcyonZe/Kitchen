using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{

    private GameObject CameraObj;
    private CameraController CameraCon;


    private void Awake()
    {
        CameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        CameraCon = CameraObj.GetComponent<CameraController>();
    }

    private void OnMouseDown()
    {
        /*if(CameraCon.GetState())
        {
            GameObject Obj = CameraObj.transform.GetChild(1).gameObject;
            Obj.transform.parent = this.transform;
            Obj.transform.position = this.transform.GetChild(0).transform.position;
            Obj.GetComponent<Box>().SetRb();
        }*/
    }

}
