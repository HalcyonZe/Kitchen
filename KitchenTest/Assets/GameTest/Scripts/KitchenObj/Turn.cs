using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : BasicObj
{
    private int turnNum = 1;

    public override void PickObjs()
    {
        GameObject go = this.transform.GetChild(0).gameObject;

        turnNum = ( turnNum + 1 ) % 3;
        
        switch (turnNum)
        {
            case 0:
                go.SetActive(false);
                break;
            case 1:
                go.SetActive(true);
                go.transform.localScale = new Vector3(13, 15, 13);
                break;
            case 2:
                go.SetActive(true);
                go.transform.localScale = new Vector3(15, 20, 15);
                break;
        }
        


    }
}
