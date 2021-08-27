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
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Collider>().enabled = false;
        this.transform.DOMove(MC.transform.GetChild(0).position, 0.1f).
                        OnComplete(() => {  
                            MC.PickObj = this.gameObject;
                            this.transform.parent = MC.transform;                            
                        });
        this.transform.DOLocalRotate(new Vector3(0, -90, 0), 0.1f);
        MC.ChangeState(MouseControl.State.HasTools);
    }

    public override void UseTools(GameObject Obj)
    {
        MC.PickObj.transform.parent = null;
        MC.PickObj = null;

        GameController.Instance.PlayerPause();

        this.GetComponent<Collider>().enabled = false;
        this.transform.localEulerAngles = new Vector3(0, 90, 0);
        knifeY = Obj.transform.position.y + 0.3f;

        transform.DOMove(new Vector3(Obj.transform.position.x, knifeY, Obj.transform.position.z), 0.3f).
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

    private void ObjSlice()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);

        //鼠标移动
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = screenPos.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //transform.position = worldPos;
        transform.position = new Vector3(worldPos.x, knifeY, worldPos.z);

        //旋转
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -1.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, 1.0f);
        }

        //切割代码
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("UsefulPlane")))
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
                        FoodItem foodItem = c.GetComponent<Foods>().m_foodItem;
                        Destroy(c.gameObject);
                        //Debug.Log(foodItem.handleScoreDic.Count);
                        if (!foodItem.handleScoreDic.ContainsKey("cut"))
                        {
                            foodItem.handleScoreDic.Add("cut", 5);
                        }
                        
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
                                obj.AddComponent<Foods>().foodInit(foodItem);
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
            this.transform.GetChild(0).gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

            PickObjs();

            GameController.Instance.PlayerPlay();
        }
    }

}
