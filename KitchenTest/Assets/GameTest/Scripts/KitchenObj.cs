using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KitchenObj : MonoBehaviour
{

    private GameObject CameraObj;
    private MouseController MouseObj;
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        CameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        MouseObj = CameraObj.GetComponent<MouseController>();
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void PickUp()
    {
        this.transform.DOMove(CameraObj.transform.GetChild(0).position, 0.1f).
                        OnComplete(() => {
                            rb.isKinematic = true;
                            MouseObj.ChangePickObj(this.gameObject);
                            this.transform.parent = CameraObj.transform;
                        });
    }*/


}
