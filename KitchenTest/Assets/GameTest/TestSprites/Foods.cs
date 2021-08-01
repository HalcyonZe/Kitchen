using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foods : BasicObj
{

    public override void PickObjs()
    {
        
        base.PickObjs();
        //Debug.Log("hhh");
        MC.ChangeState(MouseControl.State.HasFoods);
    }

}
