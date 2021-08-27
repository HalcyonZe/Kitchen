using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Pan : BasicObj
{
    public enum PanState
    {
        Normal,
        Blanching,
        Move,
    }
    public PanState m_panState = PanState.Blanching ;

    private Dictionary<GameObject, float> _foodDic = new Dictionary<GameObject, float>();
    private float PanY = 0;

    private void FixedUpdate()
    {
        JudgeFood();
        PanStateUpdate();
    }

    private void JudgeFood()
    {
        if (_foodDic.Count > 0)
        {
            for (int i = 0; i < _foodDic.Count; i++)
            {
                GameObject food = _foodDic.ElementAt(i).Key;
                if (food.transform.parent != this.transform)
                {
                    _foodDic.Remove(food);
                }          
                else if(food.transform.position.y < this.transform.position.y)
                {
                    _foodDic.Remove(food);
                    food.transform.parent = null;
                }
            }
        }
    }

    private void PanStateUpdate()
    {
        switch (m_panState)
        {
            case PanState.Blanching:
                Blanching();
                break;
            case PanState.Normal:

                break;
            case PanState.Move:
                PanMove();
                StopMove();
                break;
        }
    }

    private void Blanching()
    {
        if (_foodDic.Count > 0)
        {
            for(int i=0;i<_foodDic.Count;i++)
            {
                GameObject food = _foodDic.ElementAt(i).Key;
                if (food.transform.parent != this.transform)
                {
                    _foodDic.Remove(food);
                }
                else
                {
                    if (_foodDic[food] < 5.0f)
                    {
                        _foodDic[food] += Time.fixedDeltaTime;
                    }
                    else
                    {
                        if (!food.GetComponent<Foods>().m_foodItem.handleScoreDic.ContainsKey("blanching"))
                        {
                            food.GetComponent<Foods>().m_foodItem.handleScoreDic.Add("blanching", 5);
                        }
                    }
                }
            }
        }
    }

    private void PanMove()
    {
        //Êó±êÒÆ¶¯
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = screenPos.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, PanY, worldPos.z);

        //Ðý×ª
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward,-1.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward, 1.0f);
        }
    }

    private void StopMove()
    {
        if (Input.GetMouseButtonDown(1) && m_panState==PanState.Move)
        {
            m_panState = PanState.Normal;

            Cursor.lockState = CursorLockMode.Locked;

            PickObjs();

            GameController.Instance.PlayerPlay();
        }
    }

    public override void PickObjs()
    {
        base.PickObjs();
        MC.ChangeState(MouseControl.State.HasPan);
        m_panState = PanState.Normal;
    }

    public override void UseTools(GameObject Obj)
    {
        Obj.transform.DOMove(this.transform.GetChild(0).position, 0.1f).
            OnComplete(() => {
                Obj.transform.parent = this.transform;

                if (!_foodDic.ContainsKey(Obj))
                {
                    float cookTime = 0;
                    _foodDic.Add(Obj, cookTime);
                }
            });

    }

    public override void PutObjs()
    {
        GameObject obj = MC.PickObj;
        
        obj.transform.DOMove(this.transform.GetChild(0).position, 0.3f).
            OnComplete(() => {
                obj.GetComponent<Rigidbody>().isKinematic = false;
                obj.GetComponent<Collider>().enabled = true;
                obj.transform.parent = this.transform;

                if (!_foodDic.ContainsKey(obj))
                {
                    float cookTime = 0;
                    _foodDic.Add(obj, cookTime);
                }

                MC.PickObj = null;
            });
        MC.ChangeState(MouseControl.State.Nothing);

    }

    public override void SendObjs(GameObject Obj)
    {
        MC.PickObj.transform.parent = null;
        MC.PickObj = null;

        GameController.Instance.PlayerPause();

        //this.GetComponent<Collider>().enabled = false;
        this.transform.localEulerAngles = new Vector3(0, 90, 0);
        PanY = Obj.transform.position.y + 0.5f;

        transform.DOMove(new Vector3(Obj.transform.position.x, PanY, Obj.transform.position.z), 0.3f).
            OnComplete(() =>
            {

                CameraController.Instance.GetCamera();

                Cursor.lockState = CursorLockMode.None;

                m_panState = PanState.Move;

            });
    }

    /*private void OnCollisionExit(Collision collision)
    {
        if (_foodDic.Keys.Contains(collision.gameObject))
        {
            collision.transform.parent = null;
            _foodDic.Remove(collision.gameObject);
        }
    }*/

}
