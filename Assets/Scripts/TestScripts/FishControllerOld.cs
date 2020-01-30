using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class FishControllerOld : MonoBehaviour
{
    // Trying for a statebased AI, with its own little brain
    public FishObject FishObject;

    public GameObject MovementTargetParent;

    public string SpeciesName;

    public int State;

    public float SwimSpeed;

    public float HungerLevel;
    public float HungerTolerance;
    public float Metabolism;
    public float MaxHunger = 5;

    public float Length;

    public bool CreateTargets;
    public string[] PreferredFoodTypes;

    public bool IsAlive = true;

    public float Cost;
    //if it equals dead, then make it float to bottom then 'disolve'

    #region States
    private const int HowManyStates = 3;
    public const int IDLE = 0;
    public const int HUNGRY = 1;
    public const int EATING = 2;
    #endregion
    //Used to make sure everything has the same name, So I can check later

    // do calculations based off of time.

    private string TargetName;

    #region GameObjects
    public GameObject[] Food;
    public GameObject[] Environment;
    public GameObject[] Fish;
    //can search for fish using fish tag, then a string.contains species name
    #endregion

    #region TestVariables
    public GameObject CurrentFoodTarget;
    public bool MoveToTargets = true;
    #endregion 

    void Start()
    {
        ReadFromCardField();
        MovementTargetParent = GameObject.FindGameObjectWithTag("MovementTargetParent");
        State = IDLE;
        Debug.Log(State.ToString() + "" + this.name.ToString());
        TargetName = "Target For " + name;
        Debug.Log(TargetName.ToString());
        //Check if the scriptable object is attached, it should be, then take all the info from that.
    }

    private void ReadFromCardField()
    {
        //set color, just for demo
        GetComponent<MeshRenderer>().material.color = FishObject.color;
        SpeciesName = FishObject.SpeciesName;
        name = SpeciesName + " " + GameObject.FindGameObjectsWithTag("Fish").Length.ToString();
        SwimSpeed = FishObject.SwimSpeed;
        MaxHunger = FishObject.MaxHunger;
        HungerLevel = MaxHunger;
        Metabolism = FishObject.Metabolism;
        Cost = FishObject.Cost;
        //PreferredFoodTypes = FishObject.PreferredFoodTypes;
        List<string> DummyVariable = new List<string>();

        foreach (var item in FishObject.PreferredFoodTypes)
        {
            DummyVariable.Add(item.ToString());
        }

        PreferredFoodTypes = DummyVariable.ToArray();

        //set size based on min length and maxlength
        Length = FishObject.GiveLength();
    }

    void Update()
    {
        StateLogic();
        HungerLogic();
    }

    void HungerLogic()
    {
        //thinking every fish has its own semi randomized value of how hungry it will be.
        //i.e
        float DecreaseRate = Metabolism * Time.deltaTime;

        HungerLevel -= DecreaseRate;
        //increase the metabolism rate when swimming, reduce it when staying still

    }

    void StateLogic()
    {
        StateCorrections();
        StateChangeLogic();
        StateCheck();
    }

    private void StateCheck()
    {
        switch (State)
        {
            case IDLE:
                CreateMovementTargets();
                break;
            case HUNGRY:
                //CreateThoughtBubble;
                SearchForFood();
                break;
            case EATING:

                break;
        }
    }

    private void CreateMovementTargets()
    {
        //debug time
        TargetName = $"{name}'s Movement Target";

        GameObject[] Targets = GameObject.FindGameObjectsWithTag("MovementTarget");

        Targets = Targets.Where(n => n.name == TargetName).ToArray();

        //if (Targets == null)
        if (Targets.Length == 0)
        {
            GameObject MovementTarget = CreateTarget();
            MovementTarget.transform.parent = MovementTargetParent.transform;
        }
        else if (Targets.Length != 0 && MoveToTargets == true)
        {
            MoveTowardsTarget(Targets.FirstOrDefault());
        }

        //Vector3 MovementTargetX = new Vector3(,,0);
    }
    //Test
    public GameObject CreateTarget()
    {

        GameObject MovementTarget = new GameObject()
        {
            name = TargetName,
            tag = "MovementTarget"
        };
        //create random variable that is not on something else 
        int x = (int)UnityEngine.Random.Range(-5, 5);
        int y = (int)UnityEngine.Random.Range(-5, 5);

        MovementTarget.transform.position = new Vector3(x, y, 0);

        //maybe this will work
        MovementTarget.AddComponent<BoxCollider>();
        MovementTarget.AddComponent<Rigidbody>();
        MovementTarget.GetComponent<Rigidbody>().useGravity = false;


        MovementTarget.GetComponent<BoxCollider>().isTrigger = true;
        return MovementTarget;
    }


    private void SearchForFood()
    {
        Food = GameObject.FindGameObjectsWithTag("Food");
        //works
        Food = Food.Where(p => p.GetComponent<Food>().BeingEaten == false).ToArray();
        //Needs to go to food rather then just automagically find what it likes the most.

        if (Food.Length >= 1)
        {
            CurrentFoodTarget = Food.FirstOrDefault();

            //search for all with the shortest distance
            foreach (GameObject FoodItem in Food)
            {
                float dist = Vector3.Distance(FoodItem.transform.position, this.transform.position);

                if (dist <= Vector3.Distance(CurrentFoodTarget.transform.position, transform.position))
                {
                    CurrentFoodTarget = FoodItem;
                }
            }
            MoveTowardsTarget(CurrentFoodTarget);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered with tag {other.tag}");

        switch (other.tag)
        {
            case "Food":
                State = EATING;
                //EatLogic(other.gameObject);

                //HungerLevel += other.Sustenance;
                break;
            case "Fish":
                State = IDLE;
                break;
            case "MovementTarget":
                Debug.Log("Colliding");
                Destroy(other.gameObject);
                break;
        }
    }

    private void EatLogic(GameObject food)
    {
        //transform.position = new Vector3(0, 0, 0);
        //give it a while true so it runs once it collides, but can move outwards so it can still run.
        HungerLevel += food.GetComponent<Food>().Sustenance;
        Destroy(food);
    }

    void MoveTowardsTarget(GameObject Target)
    {
        //Shouldn't even need this wtf
        string TempName = $"{name}'s alternate target";

        GameObject TestObject = null;

        if (Physics.Linecast(transform.position, Target.transform.position, out RaycastHit hit) && hit.transform.name != Target.name)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("MovementTarget");

            if (objects.FirstOrDefault() != null && objects.FirstOrDefault().name == TempName)
            //if (GameObject.FindGameObjectWithTag("MovementTarget").name == TempName)
            {
                TestObject = CreateTarget();
                TestObject.name = TempName;
                TestObject.transform.parent = MovementTargetParent.transform;
            }
        }
        //Error with food target if they disapear too quickly

        Debug.DrawLine(transform.position, Target.transform.position, Color.red);

        //When blocked, move to new target, then move to the old target
        if (Target != null && TestObject == null)
        {
            if (Target.tag == "Food")
            {
                //More complex stuff for food stuffs
                //create a target that is close to food, and make sure food cannot be eaten by anyone else.
                float Distance = Vector3.Distance(transform.position, Target.transform.position);
                float ArbitraryDistanceFromTarget = 1;
                if (Distance > ArbitraryDistanceFromTarget)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, SwimSpeed * Time.deltaTime);
                }
                else
                {
                    /* No
                    float RotateModifier = 100f;
                    transform.RotateAround(Target.transform.position, Target.transform.forward, (SwimSpeed * RotateModifier) * Time.deltaTime);
                    */
                    var FishScript = Target.GetComponent<Food>();
                    FishScript.BeingEaten = true;

                    FishScript.Fish = this.gameObject;

                    EatLogic(Target);
                }
            }
            else
            {
                //Swim to fish
                transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, SwimSpeed * Time.deltaTime);
                State = IDLE;
            }
        }
        else if (TestObject == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TestObject.transform.position, SwimSpeed * Time.deltaTime);
        }


    }

    private void StateChangeLogic()
    {
        //And food objects in play
        if (HungerLevel <= HungerTolerance && GameObject.FindGameObjectsWithTag("Food").Length != 0)
        {
            State = HUNGRY;
        }
        /*else
        {
            State = IDLE;
        }
        */
    }

    void StateCorrections()
    {
        if (State < 0 || State > HowManyStates)
        {
            Debug.Log($"Current State {State} for {name} does not exist");
        }
    }

    void FishCollision()
    {

    }

    void FishAvoidance()
    {
        GameObject[] Fish = GameObject.FindGameObjectsWithTag("Fish");

        GameObject ClosestFish = null;

        //Search for nearest Fish
        foreach (var item in Fish)
        {
            float Distance = Vector3.Distance(item.transform.position, transform.position);

            if (ClosestFish == null)
            {
                ClosestFish = item;
            }
            else if (Distance <= Vector3.Distance(ClosestFish.transform.position, transform.position))
            {
                ClosestFish = item;
            }
        }
        //Can 9/10 get rid of above code as trying raycast, otherwise can continue to see if the fish will intersect the path and avoid it
        //Ray cast to see if can see the object its aiming for
    }

    void EnvironmentAvoidance()
    {

    }
}