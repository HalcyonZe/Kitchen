using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResultPlane : BasicObj
{
    private List<FoodItem> FoodItems = new List<FoodItem>();


    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CalcResult();
        }
    }

    public void CalcResult()
    {
        if (this.transform.childCount > 0)
        {
            GameObject res = this.transform.GetChild(0).gameObject;
            if (res.GetComponent<Plates>() != null)
            {
                Plates plates = res.GetComponent<Plates>();
                for(int i = 0; i < plates.Foods.Count;i++)
                {
                    FoodItems.Add(plates.Foods[i].GetComponent<Foods>().m_foodItem);
                }
                ScoreController.Instance.DishCalc(FoodItems);
            }
        }
    }

    public override void PutObjs()
    {        
        MC.PickObj.transform.DOMove(this.transform.position + Vector3.up * 0.5f, 0.3f).
            OnComplete(() => {
                MC.PickObj.GetComponent<Rigidbody>().isKinematic = false;
                MC.PickObj.GetComponent<Collider>().enabled = true;
                MC.PickObj.transform.parent = this.transform;
                MC.PickObj = null;
            });
        MC.ChangeState(MouseControl.State.Nothing);
    }
}
