using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using DG.Tweening;

public class Knife : MonoBehaviour
{
    public Material matCross;
    //private LineRenderer line;
    private bool UseMouse = false;

    private float knifeY;
    //相机原位
    private Vector3 CameraPos, CameraRot;

    private Transform MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.Find("Main Camera").transform;
        //line = this.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (UseMouse)
        {
            StopSlice();
            ObjSlice();
        }
    }

    //切割函数
    private void ObjSlice()
    {       
            //鼠标移动
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = screenPos.z;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            //transform.position = worldPos;
            transform.position = new Vector3(worldPos.x, knifeY, worldPos.z);

            //绘制线段
            /*Ray ray = new Ray(transform.position,-transform.up);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,Mathf.Infinity,LayerMask.GetMask("Solid")))
            {
                //Debug.Log("hhhhh");
                line.SetPosition(0, new Vector3(transform.position.x-1, hit.collider.transform.position.y, transform.position.z));
                line.SetPosition(1, new Vector3(transform.position.x+1, hit.collider.transform.position.y, transform.position.z));
            }*/

            //切割代码
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cutting board")))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    UseMouse = false;
                    transform.DOMoveY(hit.collider.transform.position.y+0.1f, 0.3f).OnComplete(() =>
                    {
                        //切割物体
                        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.05f, 0.005f), transform.rotation, LayerMask.GetMask("CutFoods"));
                        //Debug.Log(colliders.Length);
                        foreach (Collider c in colliders)
                        {
                            Destroy(c.gameObject);
                            //GameObject[] objs = c.gameObject.SliceInstantiate(transform.position, transform.up);
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

    public void UseTools(GameObject gameObj)
    {
        //Destroy( GetComponent<Rigidbody>());
        this.GetComponent<Collider>().enabled = false;

        this.transform.localEulerAngles = new Vector3(0, 90, 0);

        knifeY = gameObj.transform.position.y + 0.3f;
        transform.DOMove(new Vector3(gameObj.transform.position.x, knifeY, gameObj.transform.position.z), 0.3f).
            OnComplete(()=> { 
                Ray ray = new Ray(transform.position, -transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Cutting board")))
                {
                    //Transform Tcamera = GameObject.Find("Main Camera").transform;
                    CameraPos = MainCamera.position;
                    CameraRot = MainCamera.eulerAngles;

                    Transform T = hit.collider.transform.GetChild(1).transform;
                    MainCamera.DOMove(T.position,0.3f);
                    MainCamera.DORotate(T.eulerAngles, 0.3f);
                    Cursor.lockState = CursorLockMode.None;

                    UseMouse = true;
                }
            });
        
    }

    private void StopSlice()
    {
        if (Input.GetMouseButtonDown(1)&&UseMouse)
        {
            UseMouse = false;

            MainCamera.DOMove(CameraPos,0.3f);
            MainCamera.DORotate(CameraRot, 0.3f);
            Cursor.lockState = CursorLockMode.Locked;

            MainCamera.GetComponent<MouseController>().PickSomething(this.gameObject);

            GameController.Instance.PlayerPlay();
        }
    }

}
