using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public delegate void Interactive(GameObject Obj);

public class MouseControl : MonoBehaviour
{
    [SerializeField]
    //拿取的物体
    public GameObject PickObj = null;
    //鼠标图案
    private MouseIcon MIcon;
    //物品拿取状态
    public enum State
    {
        Nothing,
        HasFoods,
        HasPlate,
        HasTools,
        HasPan,
    }
    [SerializeField]
    private State state = State.Nothing;

    public bool CanClick = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MIcon = GameObject.Find("Mouse").GetComponent<MouseIcon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PickObj != null&&(/*PickObj.layer==11||*/ PickObj.layer == 14))
        {
            //PickObj.transform.position = this.transform.GetChild(0).position;
            PickObj.transform.localRotation = Quaternion.Euler(0, -90, this.transform.localEulerAngles.x);
        }
        if (CanClick)
        {
            MouseState();

        }
    }

    private void MouseState()
    {
        switch (state)
        {
            case State.Nothing:
                LayerMask layer1 = LayerMask.GetMask("Foods") | LayerMask.GetMask("Tools") |
                                   LayerMask.GetMask("CutFoods") | LayerMask.GetMask("Plate");
                ClickAction(layer1);
                break;
            case State.HasFoods:
                LayerMask layer2 = LayerMask.GetMask("Plane") | LayerMask.GetMask("UsefulPlane") |
                                   LayerMask.GetMask("Plate");
                ClickAction(layer2);
                break;
            case State.HasPlate:
                LayerMask layer3 = LayerMask.GetMask("Foods") | LayerMask.GetMask("Plane") | 
                                   LayerMask.GetMask("CutFoods")| LayerMask.GetMask("ResultPlane");
                ClickAction(layer3);
                break;
            case State.HasTools:
                LayerMask layer4 = LayerMask.GetMask("CutFoods") | LayerMask.GetMask("Plane");
                ClickAction(layer4);
                break;
            case State.HasPan:
                LayerMask layer5 = LayerMask.GetMask("Foods") | LayerMask.GetMask("Plane") |
                                   LayerMask.GetMask("CutFoods") | LayerMask.GetMask("Plate");
                ClickAction(layer5);
                break;

        }

    }

    private void ClickAction(LayerMask layer)
    {

        //射线检测
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100, layer.value))
        {
            //划出射线，只有在scene视图中才能看到
            Debug.DrawLine(ray.origin, hitInfo.point);
            MIcon.ToClick();

            if (Input.GetMouseButtonUp(0))
            {
                GameObject gameObj = hitInfo.collider.gameObject;

                if (PickObj == null)
                {
                    gameObj.GetComponent<BasicObj>().ObjPick();
                }
                else
                {
                    
                    PickObj.GetComponent<BasicObj>().ObjFunction(gameObj);
                }
                //gameObj.GetComponent<BasicObj>().UseObjs(state);
                MIcon.ToPoint();
            }

        }
        else
        {
            MIcon.ToPoint();
        }
    }

    public State GetState()
    {
        return state;
    }
    public void ChangeState(State istate)
    {
        state = istate;
    }
}
