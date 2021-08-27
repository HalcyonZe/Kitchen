using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : BasicObj
{
    public override void PutObjs()
    {
        base.PutObjs();
        MC.PickObj.layer = LayerMask.NameToLayer("CutFoods");
        MC.ChangeState(MouseControl.State.Nothing);
    }
}
