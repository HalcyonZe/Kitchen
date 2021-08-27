using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class BasicObj : MonoBehaviour
{
    protected MouseControl MC;

    // Start is called before the first frame update
    void Start()
    {
        MC = GameObject.Find("Main Camera").GetComponent<MouseControl>();
    }

    public void ObjPick()
    {
        PickObjs();
        /*switch (LayerMask.LayerToName(this.gameObject.layer))
        {
            case "Foods":
                PickObjs();
                break;
            case "Tools":
                PickObjs();
                break;
            case "Plate":
                PickObjs();
                break;
            case "CutFoods":
                PickObjs();
                break;
        }*/
    }

    public void ObjFunction(GameObject obj)
    {
        switch (LayerMask.LayerToName(obj.layer)) 
        {
            case "Plane":
                obj.GetComponent<BasicObj>().PutObjs();
                break;
            case "Plate":
                if (MC.GetState() == MouseControl.State.HasPan)
                {
                    SendObjs(obj);
                }
                else
                {
                    obj.GetComponent<BasicObj>().PutObjs();
                }
                break;
            case "UsefulPlane":
                obj.GetComponent<BasicObj>().PutObjs();
                break;
            case "Foods":
                UseTools(obj);
                break;
            case "CutFoods":
                UseTools(obj);
                break;
            case "ResultPlane":
                obj.GetComponent<BasicObj>().PutObjs();
                break;
        }
    }

    public void UseObjs(MouseControl.State state)
    {
        switch (state)
        {
            case MouseControl.State.Nothing:
                PickObjs();
                break;
            case MouseControl.State.HasFoods:
                PutObjs();
                break;
            case MouseControl.State.HasTools:
                if (this.gameObject.layer == 13) 
                { 
                    MC.PickObj.GetComponent<BasicObj>().UseTools(this.gameObject);
                }
                else 
                { 
                    PutObjs();
                }
                break;
            case MouseControl.State.HasPlate:
                if (this.gameObject.layer == 10 ||this.gameObject.layer == 13)
                {
                    MC.PickObj.GetComponent<BasicObj>().UseTools(this.gameObject);
                }
                else
                {
                    PutObjs();
                }
                break;
        }

    }

    public virtual void UseTools(GameObject Obj)
    {

    }

    public virtual void PickObjs()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.DOMove(MC.transform.GetChild(0).position, 0.1f).
                        OnComplete(() => {  
                            MC.PickObj = this.gameObject;
                            this.transform.parent = MC.transform;                            
                        });        
    }

    public virtual void PutObjs()
    {
        MC.PickObj.transform.parent = null;
        MC.PickObj.transform.DOMove(this.transform.GetChild(0).position, 0.3f).
            OnComplete(() => {
                MC.PickObj.GetComponent<Rigidbody>().isKinematic = false;
                MC.PickObj.GetComponent<Collider>().enabled = true;
                MC.PickObj = null;
            });
    }

    public virtual void SendObjs(GameObject Obj)
    {

    }
}
