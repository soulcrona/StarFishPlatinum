using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Food", order = 3)]
public class FoodObject : ScriptableObject
{
    public string FoodName;
    public GameObject Prefab;
    public float Sustenance;
    public Color Color;
    public float Cost;

    public FoodType foodType;

    public enum FoodType
    {
        Pellets, Meat
    }
}
