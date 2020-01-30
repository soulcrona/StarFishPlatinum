using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Food : MonoBehaviour
{
    public FoodObject FoodObject;

    public float Sustenance;

    public string FoodType;

    public float Cost;

    #region TestVariables
    public bool BeingEaten = false;
    public GameObject Fish;
    #endregion

    // sets everything up based on the foodobject it has already inside of it
    void Start()
    {

        FoodType = FoodObject.foodType.ToString();
        Debug.Log(FoodObject.foodType.ToString());

        GetComponent<MeshRenderer>().material.color = FoodObject.Color;

        Sustenance = FoodObject.Sustenance;
        Cost = FoodObject.Cost;
        
        GameObject[] PlacementValue = GameObject.FindGameObjectsWithTag("Food");

        //linq to find the ones with the right food type
        PlacementValue = PlacementValue.Where(n => n.GetComponent<Food>().FoodType.ToString() == FoodType).ToArray();
        Debug.Log(PlacementValue[0].GetComponent<Food>().FoodType.ToString());
        //switch with a global variable for ease of use, and to make sure the ids are never the same

        //Working
        name = FoodObject.FoodName + " " + PlacementValue.Length.ToString();
        Debug.Log(FoodObject.FoodName + " " + PlacementValue.Length.ToString());
        //Test
        //name = FoodObject.FoodName + " " + Counter.FishCount.ToString();
    }
    
    void Update()
    {
        
    }
}
