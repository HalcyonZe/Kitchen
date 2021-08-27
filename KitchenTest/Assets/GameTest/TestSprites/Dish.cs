using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    public DishItem _dishItem;
    public int Score = 50;
    public virtual void ListInit()
    {

    }

    public virtual void FoodInit()
    {

    }

    public virtual void ScoreCalc(List<FoodItem> foodList)
    {

    }

    public void CheckDish(List<FoodItem> foodList)
    {
        foreach(var food in _dishItem.foodScoreDic.Keys)
        {
            List<FoodItem> _foodList = new List<FoodItem>();
            _foodList = FindFood(food, foodList);
            if (_foodList.Count != 0)
            {
                FindHandle(_foodList, food);
                Score += _dishItem.foodScoreDic[food];
                //_dishItem.foodScoreDic.Remove(food);
            }
        }
    }

    public List<FoodItem> FindFood(FoodItem food, List<FoodItem> foodList)
    {
        List<FoodItem> _foodList = new List<FoodItem>();
        /*_foodItem = foodList.Find(delegate (FoodItem foodItem) {
            return foodItem.foodName == food.foodName;
        });*/
        for(int i = 0; i < foodList.Count; i++)
        {
            if (foodList[i].foodName == food.foodName)
            {
                _foodList.Add(foodList[i]);
            }
        }
        return _foodList;
    }

    public void FindHandle(List<FoodItem> foodList, FoodItem _foodItem)
    {
        if(_foodItem.handleScoreDic.Count>0)
        {
            foreach(var handle in _foodItem.handleScoreDic.Keys)
            {
                for(int i = 0; i < foodList.Count; i++)
                {
                    if (foodList[i].handleScoreDic.ContainsKey(handle))
                    {
                        Score += _foodItem.handleScoreDic[handle];
                        i = foodList.Count;
                    }
                }
                /*if(foodItem.handleScoreDic.ContainsKey(handle))
                {
                    Score += foodItem.handleScoreDic[handle];
                    _foodItem.handleScoreDic.Remove(handle);
                }*/
            }
        }
    }

    /*public void RemoveFood(string foodName)
    {
        FoodItem _foodItem = _dishItem._foodList.Find(delegate (FoodItem foodItem) {
            return foodItem.name == foodName;
        });
        _dishItem._foodList.Remove(_foodItem);
    }*/

}
