using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishItem 
{
    public string dishname;
    //public List<FoodItem> _foodList = new List<FoodItem>();
    public Dictionary<FoodItem, int> foodScoreDic = new Dictionary<FoodItem, int>();
}
