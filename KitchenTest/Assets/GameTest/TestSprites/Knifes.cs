using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EzySlice;

public class Knifes : BasicObj
{
    public Material matCross;
    private bool UseMouse = false;
    private float knifeY;
    //相机原位
    private Vector3 CameraPos, CameraRot;
    private Transform MainCamera;

    private void Awake()
    {
        MainCamera = GameObject.Find("Main Camera").transform;
    }
    private void Update()
    {
        if (UseMouse)
        {
            StopSlice();
            ObjSlice();
        }
    }
    public override void PickObjs()
    {
        base.PickObjs();
        this.transform.DOLocalRotate(new Vector3(0, -90, 0), 0.1f);
        MC.ChangeState(MouseControl.State.HasTools);
    }

    public override void UseTools(GameObject Obj)
    {
        MC.PickObj.transform.parent = null;
        MC.PickObj = null;
        //MC.CanClick = false;
        GameController.Instance.PlayerPause();

        this.GetComponent<Collider>().enabled = false;
        this.transform.localEulerAngles = new Vector3(0, 90, 0);
        knifeY = Obj.transform.position.y + 0.3f;

        transform.DOMove(new Vector3(Obj.transform.position.x, knifeY, Obj.transform.position.z), 0.3f).
            OnComplete(() => {
                Ray ray = new Ray(transform.position, -transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cutting board")))
                {
                    CameraPos = MainCamera.position;
                    CameraRot = MainCamera.eulerAngles;

                    Transform T = hit.collider.transform.GetChild(1).transform;
                    MainCamera.DOMove(T.position, 0.3f);
                    MainCamera.DORotate(T.eulerAngles, 0.3f);
                    Cursor.lockState = CursorLockMode.None;

                    UseMouse = true;
                }
            });
    }

    private void ObjSlice()
    {
        //鼠标移动
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = screenPos.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //transform.position = worldPos;
        transform.position = new Vector3(worldPos.x, knifeY, worldPos.z);
       
        //切割代码
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cutting board")))
        {
            if (Input.GetMouseButtonDown(0))
            {
                UseMouse = false;
                transform.DOMoveY(hit.collider.transform.position.y + 0.1f, 0.3f).OnComplete(() =>
                {
                    //切割物体
                    Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.05f, 0.005f), transform.rotation, LayerMask.GetMask("CutFoods"));
                    foreach (Collider c in colliders)
                    {
                        Destroy(c.gameObject);
                        SlicedHull hull = c.gameObject.Slice(transform.position, transform.forward);
                        if (hull != null)
                        {
                            GameObject lower = hull.CreateLowerHull(c.gameObject, matCross);
                            GameObject upper = hull.CreateUpperHull(c.gameObject, matCross);
                            GameObject[] objs = new GameObject[] { lower, upper };
                            foreach (GameObject obj in objs)
                            {
                                obj.AddComponent<Rigidbody>();
                                obj.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous;
                                obj.AddComponent<MeshCollider>().convex = true;
                                obj.AddComponent<Foods>();
                                obj.layer = 13;
                            }
                        }
                    }

                    transform.DOMoveY(knifeY, 0.3f).
                        OnComplete(() => { UseMouse = true; });
                });
            }
        }

    }

    private void StopSlice()
    {
        if (Input.GetMouseButtonDown(1) && UseMouse)
        {
            UseMouse = false;

            MainCamera.DOMove(CameraPos, 0.3f);
            MainCamera.DORotate(CameraRot, 0.3f);
            Cursor.lockState = CursorLockMode.Locked;

            PickObjs();

            GameController.Instance.PlayerPlay();
        }
    }

}
