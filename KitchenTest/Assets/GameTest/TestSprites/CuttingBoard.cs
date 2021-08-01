using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : BasicObj
{
    public override void PutObjs()
    {
        base.PutObjs();
        MC.PickObj.layer = 13;
        MC.ChangeState(MouseControl.State.Nothing);
    }
}
