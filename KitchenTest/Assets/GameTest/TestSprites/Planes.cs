using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planes : BasicObj
{
    public override void PutObjs()
    {               
        base.PutObjs();
        MC.ChangeState(MouseControl.State.Nothing);                
    }
}
