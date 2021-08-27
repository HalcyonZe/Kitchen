using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YiDuXi : Dish
{
    public override void ListInit()
    {
        _dishItem = new DishItem();
        _dishItem.dishname = "YiDuXu";

        FoodItem foodItem1 = new FoodItem();
        foodItem1.foodName= "streaky pork";
        foodItem1.handleScoreDic.Add("blanching", 10);
        foodItem1.handleScoreDic.Add("salt", 5);
        foodItem1.handleScoreDic.Add("pepper", 5);
        _dishItem.foodScoreDic.Add(foodItem1, 5);

        FoodItem foodItem2 = new FoodItem();
        foodItem2.foodName = "bamboo shoots";
        foodItem2.handleScoreDic.Add("cut", 5);
        foodItem2.handleScoreDic.Add("blanching", 5);
        _dishItem.foodScoreDic.Add(foodItem2, 5);

        FoodItem foodItem3 = new FoodItem();
        foodItem3.foodName = "salty streaky";
        foodItem3.handleScoreDic.Add("blanching", 10);
        _dishItem.foodScoreDic.Add(foodItem3, 5);

        FoodItem foodItem4 = new FoodItem();
        foodItem4.foodName = "ginger";
        _dishItem.foodScoreDic.Add(foodItem4, 5);

        FoodItem foodItem5 = new FoodItem();
        foodItem5.foodName = "ham";
        _dishItem.foodScoreDic.Add(foodItem5, 5);

        FoodItem foodItem6 = new FoodItem();
        foodItem6.foodName = "onion";
        _dishItem.foodScoreDic.Add(foodItem6, 5);

    }


    public override void ScoreCalc(List<FoodItem> foodList)
    {
        CheckDish(foodList);
        Debug.Log(Score);
    }

}
