using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foods : BasicObj
{
    public string foodName;
    public FoodItem m_foodItem = new FoodItem();

    private void Awake()
    {
        m_foodItem.foodName = this.foodName;
    }

    public void foodInit(FoodItem foodItem)
    {
        m_foodItem = foodItem;
        foodName = foodItem.foodName;
    }

    public override void PickObjs()
    {       
        base.PickObjs();
        MC.ChangeState(MouseControl.State.HasFoods);
        this.gameObject.layer = LayerMask.NameToLayer("Foods");
    }

    


}
