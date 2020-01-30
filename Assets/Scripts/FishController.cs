using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishController : MonoBehaviour
{
    //Rewrite of AI, to make it slightly better.
    #region Variables
    public FishObject fishObject;

    public GameObject GameSystems;

    public GameObject MovementTargetParent;

    public long Age;

    public int MaxAgeMinutes;

    public string SpeciesName;
    public float DecayRate;

    public float HungerLevel;
    public float MaxHunger;
    public float Metabolism;
    public float HungerTolerance;
    //Not used currently, but should be soon enough
    public float Length;

    public bool CreateTargets;
    public string[] PrefferedFoodTypes;
    //Deprecate maybe
    public bool IsAlive = true;

    public float Cost;

    public bool JokeMode = false;

    public float DeathRate = .5f;


    public float LengthOffset = 2f;

    #region State Variables

    public int CurrentState;

    public int HowManyStates = 4;

    public const int IDLE = 0;
    public const int HUNGRY = 1;
    public const int EATING = 2;
    public const int DYING = 3;
    #endregion

    public float SwimSpeed;
    //Global so that I do not need to keep recreating it constantly
    private string TargetName;
    public float RotationModifier = 100f;
    public float Rotation = 45f;

    #region TestVariables
    public GameObject CurrentFoodTarget;
    //mass disable through another script;
    public bool MoveToTargets;

    public GameObject EnvironmentTarget;
    //can use the environment target as a bool
    public bool MoveToEnvironment;
    public float InteractionTimer;
    public int InteractionTimeOut;
    #endregion

    #region Bounds

    public float UpperXBound;
    public float LowerXBound;
    public float UpperYBound;
    public float LowerYBound;
    public float UpperZBound;
    public float LowerZBound;

    #endregion

    #endregion
    //New Variables, Testing?

    void Start()
    {
        GameSystems = GameObject.FindGameObjectWithTag("System");
        ParseFishObject();
        MovementTargetParent = GameObject.FindGameObjectWithTag("MovementTargetParent");
        CurrentState = IDLE;
        //Add in a Unique identifier
        TargetName = $"Target for {name}";
        SetBounds();
    }

    //the bounds are to stop the fish from creating a target that is outside of the tank
    private void SetBounds()
    {
        var Bounds = GameSystems.GetComponent<GameSystems>();

        UpperXBound = Bounds.UpperXBound;
        LowerXBound = Bounds.LowerXBound;
        UpperYBound = Bounds.UpperYBound;
        LowerYBound = Bounds.LowerYBound;
        UpperZBound = Bounds.UpperZBound;
        LowerZBound = Bounds.LowerZBound;
    }
    //grabs all the data from the fish object, they check if the values are not 0 before parsing the fish object values becausae everything starts at 0 unles they have been loaded in from the load function
    private void ParseFishObject()
    {
        //set Temp Color
        GetComponent<MeshRenderer>().material.color = fishObject.color;
        SpeciesName = fishObject.SpeciesName;
        // Unique ID for fish name, somehow
        name = SpeciesName + " ID: " + UnityEngine.Random.Range(0, 100000);
        //name = SpeciesName + " " + GameObject.FindGameObjectsWithTag("Fish").Length.ToString();
        SwimSpeed = fishObject.SwimSpeed;
        MaxHunger = fishObject.MaxHunger;
        //yikes
        if (HungerLevel == 0)
        {
            HungerLevel = MaxHunger;
        }
        DecayRate = fishObject.DecayRate;

        HungerTolerance = MaxHunger / 3;

        Metabolism = fishObject.Metabolism;
        Cost = fishObject.Cost;

        PrefferedFoodTypes = fishObject.PreferredFoodTypes.Select(o => o.ToString()).ToArray();

        MaxAgeMinutes = fishObject.MaxAgeMinutes;

        if (Length == 0)
        {
            Length = fishObject.GiveLength();
        }
    }

    void Update()
    {
        StateLogic();
        HungerLogic();
        SizeLogic();
        AgeLogic();
    }

    //Ages the fish, if it gets too old, it dies
    private void AgeLogic()
    {
        if (Age < MaxAgeMinutes * 60)
        {
            Age += (long)Time.deltaTime;
        }
        else
        {
            CurrentState = DYING;
        }
    }

    //dynamically changes the size depending on how fed the fish is, although growing in size isn't working currently
    private void SizeLogic()
    {
        transform.localScale = new Vector3(1, Length / LengthOffset, Length);

        if (HungerLevel > MaxHunger)
        {
            Length += Time.deltaTime * .001f;
        }
    }
    //uses a finite state machine to contorl the fishes actions
    private void StateLogic()
    {
        switch (CurrentState)
        {
            case IDLE:
                IdleLogic();
                break;
            case HUNGRY:
                FindFood();
                if (CurrentFoodTarget != null)
                {
                    MoveToTarget(CurrentFoodTarget);
                }
                break;
            case DYING:
                DeathLogic();
                break;
        }
    }

    //for non environmental movements, just makes a beeline to a targe that was created before hand
    private void MoveToTarget(GameObject Target)
    {
        //Rotate as now using assets
        transform.LookAt(Target.transform);

        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, SwimSpeed * Time.deltaTime);

        //Implement pathfinding here and hope for the best
        Debug.DrawLine(transform.position, Target.transform.position, Color.red);
    }

    /*
    public void Temp()
    {
        //SIMS!!!
        float Tolerance = 10;
        if (HungerLevel <= HungerTolerance + Tolerance)
        {
            //Create Text bubble
            //Like a bubble, goes up with text in it, then pops
        }
    }
    */

        //checks the game for any food targets, and calculates the distance between them, deletes movement target if it has found food
    private void FindFood()
    {
        GameObject[] FoodInPlay = GameObject.FindGameObjectsWithTag("Food");

        //FoodInPlay = FoodInPlay.Where(p => p.GetComponent<Food>().FoodType == PrefferedFoodTypes.ToString());
        //for now any food
        //Make sure it isnt empty, setting to null doesnt like to work
        if (FoodInPlay.Length >= 1 && CurrentState == HUNGRY)
        {
            CurrentFoodTarget = FoodInPlay.FirstOrDefault();

            foreach (var item in FoodInPlay)
            {
                float Distance = Vector3.Distance(item.transform.position, transform.position);

                if (Distance <= Vector3.Distance(CurrentFoodTarget.transform.position, transform.position))
                {
                    CurrentFoodTarget = item;
                    //Delete the movementTarget
                    Destroy(GameObject.Find(TargetName));
                }
            }
        }
    }

    //hunger logic, if it isnt full but not starving nothing happens other thne its getting hungrier, 
    //however if it begins to starve it starts to shrink in size
    private void HungerLogic()
    {
        HungerLevel -= Metabolism * Time.deltaTime;

        if (HungerLevel <= HungerTolerance && Length > 0)
        {
            Length -= (DecayRate * Time.deltaTime) / 100;
        }
        //40k being average food, need to change to variable, but seriously running out of time
        if (HungerLevel <= 40000 && GameObject.FindGameObjectsWithTag("Food").Length != 0)
        {
            CurrentState = HUNGRY;
        }
        //if no food in the game scene it just goes to idling
        else if (GameObject.FindGameObjectsWithTag("Food").Length == 0)
        {
            CurrentState = IDLE;
        }
        //if it beyond starving it starts to die
        if (HungerLevel <= 0)
        {
            CurrentState = DYING;
        }
    }

    //when the fish isnt looking for food it is idling, it dynamically creates a movement target and starts to move to it,
    //at one point environments were supported but then we pivoted
    private void IdleLogic()
    {
        //When the 'fish' is idle it will spawn in some movement objects and swim to them, if there is an enviromental thing in 
        //the game scene it will swim to that instead
        // Check if already has target
        GameObject MovementObject = GameObject.Find(TargetName);

        if (MovementObject == null && EnvironmentTarget == null)
        {
            MovementObject = CreateMovementTarget();
        }
        else if (MovementObject != null && EnvironmentTarget == null)
        {
            MoveToTarget(MovementObject);
        }
        /*
        if (UnityEngine.Random.Range(0, 1000) == 500 && GameObject.FindGameObjectsWithTag("Environment").Count() > 0)
        {
            GameObject[] EnvironmentObject = GameObject.FindGameObjectsWithTag("Environment");
            int i = 0;

            i = UnityEngine.Random.Range(0, EnvironmentObject.Length);

            EnvironmentTarget = EnvironmentObject[i];
        }
        /* deprecated
        if (EnvironmentTarget != null)
        {
            InteractWithEnvironment(EnvironmentTarget);
        }
        */
    }

    //deprecated
    private void InteractWithEnvironment(GameObject Target)
    {
        if (Vector3.Distance(transform.position, Target.transform.position) >= Length * 2)
        {
            MoveToTarget(Target);
        }
        else
        {
            //Rotate around for 10 seconds or so then idle, also set random direction
            //make kinematic true to make sure that it doesnt move it by accident
            if (InteractionTimer <= InteractionTimeOut)
            {
                InteractionTimer += Time.deltaTime;
                Target.GetComponent<Rigidbody>().isKinematic = true;
                Target.GetComponent<Rigidbody>().useGravity = false;
                // broken, but wonderful, so leaving in
                if (JokeMode == false)
                {
                    transform.RotateAround(Target.transform.position, Vector3.up, (SwimSpeed * Time.deltaTime) * RotationModifier);

                    transform.rotation = new Quaternion(0, 1 * Time.deltaTime, 0, 1);
                }
                else
                {

                    Quaternion TargetDirection = new Quaternion((Target.transform.position.x - transform.position.x), Target.transform.position.y - transform.position.y, 0, 1f);

                    transform.rotation = TargetDirection;
                }
            }
            else
            {
                Target.GetComponent<Rigidbody>().isKinematic = false;
                Target.GetComponent<Rigidbody>().useGravity = true;
                InteractionTimer = 0;
                EnvironmentTarget = null;
            }
        }
    }

    //creates a movement target and spawns it in an appropriate area according to the bounds of the fish tank
    private GameObject CreateMovementTarget()
    {
        Vector3 SpawnPosition;

        float X = UnityEngine.Random.Range(LowerXBound, UpperXBound);
        float Y = UnityEngine.Random.Range(LowerYBound, UpperYBound);
        float Z = UnityEngine.Random.Range(LowerZBound, UpperZBound);

        SpawnPosition = new Vector3(X, Y, Z);

        GameObject MovementTarget = new GameObject()
        {
            name = TargetName,
            tag = "MovementTarget"
        };
        MovementTarget.AddComponent<BoxCollider>().isTrigger = true;
        MovementTarget.AddComponent<Rigidbody>().useGravity = false;

        MovementTarget.transform.position = SpawnPosition;
        MovementTarget.transform.parent = MovementTargetParent.transform;

        return MovementTarget;
    }

    private void DeathLogic()
    {
        //Play animation, sits at bottom on tank till it fades away or is eaten :O
        //GetComponent
        //set rotation to upside down, but slowly turn
        GetComponent<Animator>().enabled = false;
        Quaternion TargetRotation = new Quaternion(transform.rotation.x, transform.rotation.z, -180f, 0);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, SwimSpeed /2 );
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        //shrink baby shrink
        //going negative causes it to grow again, and don't want that

        if (Length <= 0)
        {
            GetComponent<BoxCollider>().enabled = false;
            //lets out 1 final bubble upon death
            //GetComponent<BubbleFX>().enabled = true;
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play(0);
                Handheld.Vibrate();
                GameObject Temp = GameObject.FindGameObjectWithTag("System");
                Temp.GetComponent<GameSystems>().FishDeaths++;
                GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_bruh_you_gonna_let_your_fish_die_like_that);
                GooglePlayGamesScript.IncrementAchievements(GPGSIds.achievement_you_monster,1);
                GooglePlayGamesScript.IncrementAchievements(GPGSIds.achievement_the_reaper_rises,1);
                Destroy(gameObject, GetComponent<AudioSource>().clip.length);
            }
        }
        else
        {
            Length -= DeathRate * Time.deltaTime;
        }

    }

    IEnumerator WaitForMe(float TimeToWait)
    {
        yield return new WaitForSeconds(TimeToWait);
    }

    //collision detection handled by the unity engine
    public void OnTriggerEnter(Collider other)
    {
        //Changing to Switches
        switch (other.transform.tag)
        {
            case "Food":
                //Rewrite to include animation
                var FoodScript = other.transform.GetComponent<Food>();

                HungerLevel += FoodScript.Sustenance;

                Destroy(other.gameObject);
                break;
            case "MovementTarget":
                //Big yikes but it works
                if (other.transform.name == TargetName)
                {
                    Destroy(other.gameObject);
                }
                break;
        }
    }
}
