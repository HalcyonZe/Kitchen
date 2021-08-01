using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Plates : BasicObj
{
    private List<GameObject> Foods;

    private void Awake()
    {
        Foods = new List<GameObject>();
    }

    private void Update()
    {
        if (Foods.Count > 0)
        {
            for(int i=0;i<Foods.Count;i++)
            {
                if (Foods[i].transform.position.y < this.transform.position.y)
                {
                    Foods[i].transform.parent = null;
                    Foods.Remove(Foods[i]);
                }
                /*Ray ray = new Ray(Foods[i].transform.position, -Foods[i].transform.up);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Plate")))
                {
                    Foods[i].transform.parent = null;
                    Foods.Remove(Foods[i]);
                }*/
            }
        }
    }

    public override void PickObjs()
    {
        //base.PickObjs();
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.transform.DOMove(MC.transform.GetChild(0).position, 0.1f).
                        OnComplete(() => {
                            MC.PickObj = this.transform.gameObject;
                            this.transform.parent = MC.transform;
                        });
        //this.transform.DOLocalRotate(new Vector3(90, -90, 0), 0.1f);
        MC.ChangeState(MouseControl.State.HasPlate);
    }

    public override void UseTools(GameObject Obj)
    {
        Obj.transform.DOMove(this.transform.GetChild(0).position, 0.1f).
            OnComplete(()=> {
                Obj.transform.parent = this.transform;
                Foods.Add(Obj);
            });
                
    }

    public override void PutObjs()
    {
        MC.PickObj.transform.parent = this.transform;
        MC.PickObj.transform.DOMove(this.transform.GetChild(0).position, 0.3f).
            OnComplete(() => {
                MC.PickObj.GetComponent<Rigidbody>().isKinematic = false;
                MC.PickObj.GetComponent<Collider>().enabled = true;
                Foods.Add(MC.PickObj);
                MC.PickObj = null;          
            });
        MC.ChangeState(MouseControl.State.Nothing);
    }
}
