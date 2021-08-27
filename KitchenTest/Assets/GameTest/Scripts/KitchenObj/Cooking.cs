using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cooking : BasicObj
{
    public enum CookingTools
    {
        salt,
        pepper,
    }

    public CookingTools m_cooking;

    private bool UseMouse = false;
    private float ObjY = 0;

    private void FixedUpdate()
    {
        
        if (UseMouse)
        {
            ObjCooking();
            StopCooking();
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public override void PickObjs()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Collider>().enabled = false;

        this.transform.DOMove(MC.transform.GetChild(0).position, 0.1f).
                        OnComplete(() => {
                            MC.PickObj = this.gameObject;
                            this.transform.parent = MC.transform;
                        });
        

        MC.ChangeState(MouseControl.State.HasTools);
    }

    public override void UseTools(GameObject Obj)
    {
        MC.PickObj.transform.parent = null;
        MC.PickObj = null;
        GameController.Instance.PlayerPause();

        ObjY = Obj.transform.position.y + 0.3f;

        transform.DOMove(new Vector3(Obj.transform.position.x, ObjY, Obj.transform.position.z), 0.3f).
            OnComplete(() => {
                Ray ray = new Ray(transform.position, -transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UsefulPlane")))
                {
                    CameraController.Instance.GetCamera();

                    Transform T = hit.collider.transform.GetChild(1).transform;

                    CameraController.Instance.transform.DOMove(T.position, 0.3f);
                    CameraController.Instance.transform.DORotate(T.eulerAngles, 0.3f);

                    Cursor.lockState = CursorLockMode.None;

                    UseMouse = true;
                }
            });
    }

    private void ObjCooking()
    {
        this.transform.eulerAngles = new Vector3(180, 0, 0);
        // Û±Í“∆∂Ø
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = screenPos.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //transform.position = worldPos;
        transform.position = new Vector3(worldPos.x, ObjY, worldPos.z);


        if (Input.GetMouseButtonDown(0))
        {
            transform.DOShakePosition(0.5f,new Vector3(0,0.2f,0));
            Ray ray = new Ray(transform.position, transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("CutFoods")))
            {
                //Debug.Log("hhh");
                GameObject obj = hit.transform.gameObject;
                SetCooking(obj);
            }
        }

    }

    private void SetCooking(GameObject obj)
    {
        switch(m_cooking)
        {
            case CookingTools.salt:
                obj.GetComponent<Foods>().m_foodItem.handleScoreDic.Add("salt", 5);
                break;
            case CookingTools.pepper:
                obj.GetComponent<Foods>().m_foodItem.handleScoreDic.Add("pepper", 5);
                break;
        }
    }

    private void StopCooking()
    {
        if (Input.GetMouseButtonDown(1) && UseMouse)
        {
            UseMouse = false;

            Cursor.lockState = CursorLockMode.Locked;

            PickObjs();

            GameController.Instance.PlayerPlay();
        }
    }

}
