using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Plate : MonoBehaviour
{
    List<GameObject> Objs =null;
    // Start is called before the first frame update
    void Start()
    {
        //Objs = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Objs.Count > 0)
        {
            foreach (GameObject o in Objs)
            {
                float dist = Vector3.Distance(o.transform.position, this.transform.position);
                if (dist >= 1)
                {
                    Objs.Remove(o);
                    o.transform.parent = null;
                }
            }
        }*/
    }

    public void UseTools(GameObject Obj)
    {
        Obj.transform.DOMove(this.transform.GetChild(0).position, 0.1f);
        Obj.transform.parent = this.transform.parent;
        //Objs.Add(Obj);
    }

}
