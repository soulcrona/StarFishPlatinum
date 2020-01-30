using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "FishData", menuName = "Fish", order = 1)]
public class FishObject : ScriptableObject
{
    public GameObject Prefab;

    public string SpeciesName = "Guppy";
    public bool IsAlive = true;
    public Color color = Color.blue;
    public float SwimSpeed = 5;
    public float DecayRate;

    public int State = 0;
    //maybe replace hunger level because might just set that to max
    public float MaxHunger = 0;
    public float HungerTolerance;
    public float Metabolism;

    public bool CreateTargets = false;

    public FoodTypes[] PreferredFoodTypes;

    //public bool TargetName;

    public float MaxLength;
    public float MinLength;

    public float Length;

    public bool GiveBirth;

    public float Cost;
    //instead of storing a massive value, can tweak easier without having to know what is 67865 / 60
    public int MaxAgeMinutes;

    //gives a length based on the minimum and maximum length
    public float GiveLength()
    {
        Length = Random.Range(MinLength,MaxLength);

        return Length;
    }

    public enum FoodTypes{
        Pellets,
        Meat
    }
}
