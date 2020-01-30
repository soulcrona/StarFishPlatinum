using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="EnvironmentData", menuName ="Environment", order = 2)]
public class EnvironmentObject : ScriptableObject
{
    public GameObject Prefab;

    public bool InPlay;

    public float MaxCondition;

    public float ConditionStatusAsANumber;

    public float DecayRate;

    public bool CanBeFood;

    public float Bought;

   // public Color color = Color.green;

    public FoodType foodType;

    public float Cost;

    public enum FoodType
    {
        Plant
    }
}