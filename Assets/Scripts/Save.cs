using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Save
{
    public int AmountOfFish;
    public int AmountOfEnvironments;
    public float Money;
    public int TotalFishBred;
    public int FishDeaths;

    //EachType of environment
    public int PatricksHouseAmount;

    public List<float> PatricksHouseX = new List<float>();
    public List<float> PatricksHouseY = new List<float>();
    public List<float> PatricksHouseZ = new List<float>();
    public List<float> PatricksConditionStatusNumber = new List<float>();

    public double SaveTime;
    //Every new thing I have to add 

    public int GuppyCount;
    
    //Guppy
    public List<float> GuppyX = new List<float>();
    public List<float> GuppyY = new List<float>();
    public List<float> GuppyZ = new List<float>();

    public List<long> GuppyAGE = new List<long>();

    public List<float> GuppyHungerLevel = new List<float>();
    public List<float> GuppyLength = new List<float>();

    //Orange Tail

    public List<float> BlueTailX = new List<float>();
    public List<float> BlueTailY = new List<float>();
    public List<float> BlueTailZ = new List<float>();

    public List<long> BlueTailAGE = new List<long>();

    public List<float> BlueTailHungerLevel = new List<float>();
    public List<float> BlueTailLength = new List<float>();


    public float[] PositionToFloat(Vector3 vector)
    {
        float[] Vector3Position = new float[3];

        Vector3Position[0] = vector.x;
        Vector3Position[1] = vector.y;
        Vector3Position[2] = vector.z;

        return Vector3Position;
     }   

    public Vector3 ReturnToVector3(float[] FloatToConvert)
    {
        Vector3 v3;

        v3 = new Vector3(FloatToConvert[0], FloatToConvert[1],FloatToConvert[2]);

        return v3;
    }
}
