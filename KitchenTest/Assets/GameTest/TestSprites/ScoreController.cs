using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public Dish current_Dish;
    //public List<Dish> dishList;

    public static ScoreController Instance;

    private void Awake()
    {
        Instance = this;
        DishInit();
    }


    public void DishInit()
    {
        this.gameObject.AddComponent<YiDuXi>();

        current_Dish = this.GetComponent<YiDuXi>();
        current_Dish.ListInit();
    }


    public void DishCalc(List<FoodItem> foodList)
    {
        current_Dish.ScoreCalc(foodList);
    }

}
