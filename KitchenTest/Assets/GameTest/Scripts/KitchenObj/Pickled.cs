using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Pickled : BasicObj
{
    private Dictionary<GameObject, float> _foodDic = new Dictionary<GameObject, float>();

    private void FixedUpdate()
    {
        PickledFood();
    }

    private void PickledFood()
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
                else
                {
                    if (_foodDic[food] < 3.0f)
                    {
                        _foodDic[food] += Time.fixedDeltaTime;
                    }
                    else
                    {
                        FoodItem newFood = new FoodItem();
                        newFood.foodName = "salty streaky";
                        food.GetComponent<Foods>().foodInit(newFood);
                        food.transform.parent = null;
                        _foodDic.Remove(food);
                    }
                }
            }
        }
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

        /*FoodItem food = new FoodItem();
        food.foodName = "salty streaky";
        obj.GetComponent<Foods>().foodInit(food);*/
    }
}
