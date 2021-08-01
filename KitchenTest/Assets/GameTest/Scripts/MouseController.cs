using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MouseController : MonoBehaviour
{

    //拿取的物体
    private GameObject PickObj = null;
    //鼠标图案
    private MouseIcon MIcon;
    //物品拿取状态
    private enum State
    {
        Nothing,
        HasFoods,
        HasPlate,
        HasTools,
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
                LayerMask layer1 = 1 <<10| 1 <<11| 1 <<13| 1 <<14;
                ClickAction(layer1);
                break;
            case State.HasFoods:
                LayerMask layer2 = 1 << 12 | 1 << 15;
                ClickAction(layer2);
                break;
            case State.HasPlate:
                LayerMask layer3 = 1 << 10 | 1 << 12 | 1 << 13;
                ClickAction(layer3);
                break;
            case State.HasTools:
                LayerMask layer4 =  1 << 13 | 1 << 12;
                ClickAction(layer4);
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
                //交互函数
                Interactive(gameObj);

                MIcon.ToPoint();
            }

        }
        else
        {
            MIcon.ToPoint();
        }
    }
    //交互函数
    private void Interactive(GameObject gameObj)
    {
        if (state == State.Nothing)
        {
            PickSomething(gameObj);
        }
        else
        {
            switch (gameObj.layer)
            {
                case 10:
                    UseTools(gameObj);
                    break;
                case 12:
                    PutSomething(gameObj);
                    break;
                case 13:
                    UseTools(gameObj);
                    break;
                case 15:
                    PutSomething(gameObj);
                    break;
            }            
        }
        
    }
    //拿起物品
    public void PickSomething(GameObject obj)
    {
        //Destroy(obj.GetComponent<Rigidbody>());
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.DOMove(this.transform.GetChild(0).position, 0.1f).
                        OnComplete(() => {
                            //obj.GetComponent<Rigidbody>().isKinematic = true;    
                            
                            PickObj = obj;                            
                            obj.transform.parent = this.transform;
                            if (obj.layer == 11|| obj.layer == 14)
                            {
                                obj.transform.DOLocalRotate(new Vector3(0, -90, 0),0.2f);
                                if (obj.layer == 11) { obj.GetComponent<Collider>().enabled = false; }
                            }
                        });        
        
        if(obj.layer==10)
        {
            state = State.HasFoods;
        }
        else if(obj.layer==14)
        {
            state = State.HasPlate;
        }
        else
        {
            state = State.HasTools;
        }
        CanClick = true;

    }
    //使用物品
    private void UseTools(GameObject gameObj) {
        
        if(PickObj.tag=="Knife")
        {   PickObj.transform.parent = null;
            CanClick = false;
            GameController.Instance.PlayerPause();
            PickObj.GetComponent<Knife>().UseTools(gameObj);
        }        
        else if (PickObj.tag == "Plate")
        {
            PickObj.GetComponent<Plate>().UseTools(gameObj);
        }
    }
    //放下物品
    private void PutSomething(GameObject obj) 
    {
        PickObj.transform.parent = null;
        PickObj.transform.DOMove(obj.transform.GetChild(0).position, 0.3f).
            OnComplete(()=> {
                //PickObj.AddComponent<Rigidbody>();
                PickObj.GetComponent<Rigidbody>().isKinematic = false;
                PickObj.GetComponent<Collider>().enabled = true;
                if (PickObj.layer == 10 && obj.layer == 15) { PickObj.layer = 13; }
                PickObj = null;
                state = State.Nothing; 
            });
        //PickObj.GetComponent<Rigidbody>().isKinematic = false;
        
    }
    //手中物品移动
    /*private void PickObjMove( )
    {
        if(PickObj!=null)
        {
            PickObj.transform.position = this.transform.GetChild(0).position;
            PickObj.transform.rotation = this.transform.GetChild(0).rotation;
        }
    }*/
    
}
