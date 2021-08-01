using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public delegate void Interactive(GameObject Obj);

public class MouseControl : MonoBehaviour
{
    [SerializeField]
    //��ȡ������
    public GameObject PickObj = null;
    //���ͼ��
    private MouseIcon MIcon;
    //��Ʒ��ȡ״̬
    public enum State
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
                LayerMask layer1 = 1 << 10 | 1 << 11 | 1 << 13 | 1 << 14;
                ClickAction(layer1);
                break;
            case State.HasFoods:
                LayerMask layer2 = 1 << 12 | 1 << 15 | 1 << 14;
                ClickAction(layer2);
                break;
            case State.HasPlate:
                LayerMask layer3 = 1 << 10 | 1 << 12 | 1 << 13;
                ClickAction(layer3);
                break;
            case State.HasTools:
                LayerMask layer4 = 1 << 13 | 1 << 12;
                ClickAction(layer4);
                break;

        }

    }

    private void ClickAction(LayerMask layer)
    {

        //���߼��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100, layer.value))
        {
            //�������ߣ�ֻ����scene��ͼ�в��ܿ���
            Debug.DrawLine(ray.origin, hitInfo.point);
            MIcon.ToClick();

            if (Input.GetMouseButtonUp(0))
            {

                GameObject gameObj = hitInfo.collider.gameObject;
                //��������
                //interactive(gameObj);
                /*if (state == State.HasPlate|| state == State.HasTools)
                {
                    PickObj.GetComponent<BasicObj>().UseTools(gameObj);
                }
                else
                {*/
                    gameObj.GetComponent<BasicObj>().UseObjs(state);
                //}
                
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
