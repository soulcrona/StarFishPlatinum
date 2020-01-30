using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public EnvironmentObject Object;
    //maybe remove
    public bool InPlay;
    public float MaxCondition;
    public float ConditionStatusAsNumber;
    public float DecayRate;
    public bool CanBeFood;
   // public Color color;
    public string FoodType;
    public float Sustenance;
    public float Cost;

    // Start is called before the first frame update
    void Start()
    {
        ReadValues();
    }

    //read the value from the attached object, however... this script has been deprecated
    void ReadValues()
    {
        InPlay = true;
        MaxCondition = Object.MaxCondition;
        ConditionStatusAsNumber = MaxCondition;
        DecayRate = Object.DecayRate;
        CanBeFood = Object.CanBeFood;
        Cost = Object.Cost;
        //Quick Switcharoo soon//color = Environment.color;
        //GetComponent<MeshRenderer>().material.color = Object.color;
        FoodType = Object.foodType.ToString();
    }
}
