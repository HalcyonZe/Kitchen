using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    MoveMent,
    HasFoods,
    HasPlate,
    HasTools,
}


public abstract class BaseState 
{
    public virtual void OnEnter()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}
