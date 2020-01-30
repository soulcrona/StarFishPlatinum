using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//was used for testing all the fish in the game scene at once by removing all thier movement targets so I could do minut changes
public class MassFishController : MonoBehaviour
{
    public bool FishMetabolismActive = false;
    public bool MoveToTargets = true;

    public static bool ThingsCostMoney = true;
    public bool ThingsCostMoneyNonStatic = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ThingsCostMoney = ThingsCostMoneyNonStatic;

        var Fish = GameObject.FindGameObjectsWithTag("Fish");


        foreach (var item in Fish)
        {
            if (MoveToTargets == true)
            {
                item.GetComponent<FishController>().MoveToTargets = true;
            }
            else
            {
                item.GetComponent<FishController>().MoveToTargets = false;
            }

        }

        int tempValue1 = 0;
        int tempValue2 = -100;

        if (FishMetabolismActive == true)
        {
            tempValue1 = 1;
            tempValue2 = 0;
        }
        else
        {
            tempValue1 = 0;
            tempValue2 = -100;

        }

        foreach (var item in Fish)
        {
            item.GetComponent<FishController>().Metabolism = tempValue1;
            item.GetComponent<FishController>().HungerTolerance = tempValue2;
        }

        //set values to something else so I can reuse the code
    }
}
