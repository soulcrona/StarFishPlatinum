using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;
using UnityEngine.UI;

public class GameSystems : MonoBehaviour
{
    public static float GlobalMoney;
    public float StartMoney = 100;
    public int FishDeaths;
    public static string EnvironmentName;
    public static int TotalFishBred;
    public int MinutesBetweenSaves = 60;

    public GameObject ValueObject;
    public GameObject FishOfFishObject;
    public GameObject EnvironmentNumberObject;

    public GameObject Guppy;
    public GameObject BlueTail;

    public GameObject PatricksHouse;

    public Transform FishParent;
    public Transform EnvironmentParent;

    public float ElegableBreedingSize;

    public float UpperXBound;
    public float LowerXBound;
    public float UpperYBound;
    public float LowerYBound;
    public float UpperZBound;
    public float LowerZBound;

    public static int FishAtStart;

    // Start is called before the first frame update, deprecated for on application focus
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        GlobalMoney = StartMoney;
        //LoadTheGame();
        FishAtStart = GameObject.FindGameObjectsWithTag("Fish").Count();
    }

    //The whole methodology of android is that an app never fully closes, so it broke all my on quit functions
    // however, I could replicate that with an on application focus script
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            SaveTheGame();
        }
        else
        {

            GooglePlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_money_money_money_moneeyyy_monaayyy, (long)GlobalMoney);
            //3 == dying
            int FishInTank = GameObject.FindGameObjectsWithTag("Fish").Where(p => p.GetComponent<FishController>().CurrentState != 3).Count();
            GooglePlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_fish_in_tank, FishInTank);
            DeleteEverything();
            LoadTheGame();
            FishAtStart = GameObject.FindGameObjectsWithTag("Fish").Count();
        }
    }

    //finds and deletes everything that is a fish or food
    public void DeleteEverything()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Fish"))
        {
            Destroy(item.gameObject);
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("Food"))
        {
            Destroy(item.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] FishCount;
        GameObject[] EnvironmentCount;

        FishCount = GameObject.FindGameObjectsWithTag("Fish");
        EnvironmentCount = GameObject.FindGameObjectsWithTag("Environment");

        ValueObject.GetComponent<Text>().text = $"${GlobalMoney}";
        FishOfFishObject.GetComponent<Text>().text = $"{FishCount.Length}";
        EnvironmentNumberObject.GetComponent<Text>().text = EnvironmentName;
        AchievementHandling();
    }


    //loads the game using a binary formatter, the save will always be saved in the persistent datapath, so no matter what happens with the device saving and loading will happen in the same place as each other
    //
    private void LoadTheGame()
    {
        if (File.Exists(Application.persistentDataPath + "/savedata.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedata.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            //achievement Tracking
            FishDeaths = save.FishDeaths;
            TotalFishBred = save.TotalFishBred;

            //checks the time between save files
            double CurrentTime = (DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
            double TimeSinceLastLogin = CurrentTime - save.SaveTime;
            //creates a fish based on the data from the size files
            CreateFish(Guppy, save.GuppyX, save.GuppyY, save.GuppyZ, save.GuppyLength, save.GuppyHungerLevel, TimeSinceLastLogin, save.GuppyAGE);
            CreateFish(BlueTail, save.BlueTailX, save.BlueTailY, save.BlueTailZ, save.BlueTailLength, save.BlueTailHungerLevel, TimeSinceLastLogin, save.BlueTailAGE);
            //divisble by 60 

            //makes sure that you cannot close and open the fish a hundred times to give yourselves 100 chances
            double DecentTimeBetweenSaves = MinutesBetweenSaves * 60;

            //breed chance for every 60 minutes you have logged off
            for (double i = TimeSinceLastLogin; i > DecentTimeBetweenSaves; i-= DecentTimeBetweenSaves)
            {
                BreedLikeFish(ElegableFishCount(save.GuppyLength, Guppy, save.GuppyHungerLevel), Guppy);
                BreedLikeFish(ElegableFishCount(save.BlueTailLength, BlueTail, save.BlueTailHungerLevel), BlueTail);
            }

            //environments completely changed, is deprecated
            /*
            for (int b = 0; b < save.PatricksHouseX.Count; b++)
            {
                Vector3 position = new Vector3(save.PatricksHouseX[b], save.PatricksHouseY[b], save.PatricksHouseZ[b]);
                var NotBob = Instantiate(PatricksHouse, position, Quaternion.identity);
                NotBob.transform.parent = EnvironmentParent;
                NotBob.GetComponent<EnvironmentController>().ConditionStatusAsNumber = save.PatricksConditionStatusNumber[b];
            }
            */

            GlobalMoney = save.Money;
            GooglePlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_fish_bred, TotalFishBred);
            Debug.Log("Game Loaded");
        }
    }
    //creates fish based on values
    public void CreateFish(GameObject FishToSpawn, List<float> X, List<float> Y, List<float> Z, List<float> Length,List<float> HungerLevel, double TimeSinceLastLogin, List<long> Age)
    {
        // save. ba, would be x, y. So we could do X instead
        for (int i = 0; i < X.Count; i++)
        {
            Vector3 position = new Vector3(X[i], Y[i], Z[i]);
            var bob = Instantiate(FishToSpawn, position, Quaternion.identity);
            var BobScript = bob.GetComponent<FishController>();
            bob.transform.parent = FishParent;
            BobScript.HungerLevel = (float)(HungerLevel[i] - (BobScript.fishObject.Metabolism * TimeSinceLastLogin));
            BobScript.Length = Length[i];
            BobScript.Age = Age[i];
        }
    }

    //computes how many fish can breed, this saves from having baby fish breeding
    public int ElegableFishCount(List<float> FishLength, GameObject FishType, List<float> FishHunger)
    {
        int ElegableFishCount = 0;

        for (int i = 0; i < FishLength.Count; i++)
        {
            //elegableBreedingSize would be different for each fish, so lets go /3 and then check hunger
            ElegableBreedingSize = FishType.transform.GetComponent<FishController>().fishObject.GiveLength();
            
            if (FishLength[i] >= ElegableBreedingSize && FishHunger[i] >= FishType.GetComponent<FishController>().MaxHunger / 2)
            {
                ElegableFishCount++;
            }
        }
        return ElegableFishCount;
    }
    //calculates how many fish can breed and where the fish would be when you log in the save file
    private void BreedLikeFish(int FishCount, GameObject Type)
    {
        //divide by 2
        int TriesToBreed = (int)FishCount / 2;
        Debug.Log($"Tries to breed: {TriesToBreed}");
        //give more validation because if the user just rapidly quits the game its just a 50/50 to get more money quicker.
        for (int i = 0; i < TriesToBreed; i++)
        {
            if (DateTime.Now.Millisecond % 8 == 0)
            {
                //Replace with bounds
                
                Vector3 position = new Vector3(UnityEngine.Random.Range(LowerXBound,UpperXBound), UnityEngine.Random.Range(LowerYBound, UpperYBound), UnityEngine.Random.Range(LowerZBound, UpperZBound));
                var bob = Instantiate(Type, position, Quaternion.identity);
                var BobScript = bob.GetComponent<FishController>();
                bob.transform.parent = FishParent;
                BobScript.Length = BobScript.fishObject.GiveLength() - BobScript.fishObject.MinLength;
                Debug.Log("Its a baby");
                //add a fish to total fish ever
                TotalFishBred++;
                GooglePlayGamesScript.IncrementAchievements(GPGSIds.achievement_its_brock_from_pewter_city, 1);
            }
            else
            {
                Debug.Log("No Baby");
            }
        }
    }
    
    //creates a save game
    private Save CreateSaveGame()
    {
        Save SaveGame = new Save();

        GameObject[] Environemts = GameObject.FindGameObjectsWithTag("Environment");

        GameObject[] Fish = GameObject.FindGameObjectsWithTag("Fish");
        Fish = Fish.Where(p => p.transform.GetComponent<FishController>().HungerLevel > 0).ToArray();

        GameObject[] PatrickEnvironments = Environemts.Where(p => p.name.Contains("Patrick")).ToArray();

        GameObject[] GuppyFishs = Fish.Where(p => p.name.Contains("Guppy")).ToArray();

        GameObject[] BlueTailFish = Fish.Where(p => p.name.Contains("Blue Tail")).ToArray();

        SaveGame.Money = GlobalMoney;
        SaveGame.SaveTime = (DateTime.Now - new DateTime(1970,1,1)).TotalSeconds;
        
        /* Deprecated
        foreach (var item in PatrickEnvironments)
        {
            SaveGame.PatricksHouseX.Add(item.transform.position.x);
            SaveGame.PatricksHouseY.Add(item.transform.position.y);
            SaveGame.PatricksHouseZ.Add(item.transform.position.z);
            SaveGame.PatricksConditionStatusNumber.Add(item.GetComponent<EnvironmentController>().ConditionStatusAsNumber);
        }
        */
        //grabs every fish in the game scene, the reason why I can just grab every fish is because they all spawn the same thing and the user would not be the wiser
        foreach (var item in GuppyFishs)
        {
            SaveGame.GuppyX.Add(item.transform.position.x);
            SaveGame.GuppyY.Add(item.transform.position.y);
            SaveGame.GuppyZ.Add(item.transform.position.z);

            SaveGame.GuppyHungerLevel.Add(item.GetComponent<FishController>().HungerLevel);
            SaveGame.GuppyLength.Add(item.GetComponent<FishController>().Length);
            SaveGame.GuppyAGE.Add(item.GetComponent<FishController>().Age);
        }

        foreach (var item in BlueTailFish)
        {
            SaveGame.BlueTailX.Add(item.transform.position.x);
            SaveGame.BlueTailY.Add(item.transform.position.y);
            SaveGame.BlueTailZ.Add(item.transform.position.z);

            SaveGame.BlueTailHungerLevel.Add(item.GetComponent<FishController>().HungerLevel);
            SaveGame.BlueTailLength.Add(item.GetComponent<FishController>().Length);
            SaveGame.BlueTailAGE.Add(item.GetComponent<FishController>().Age);
        }
        //achievement tracking
        SaveGame.FishDeaths = FishDeaths;
        SaveGame.TotalFishBred = TotalFishBred;
        return SaveGame;
    }

    //saves the game to the persistent data path
    public void SaveTheGame()
    {
        Save save = CreateSaveGame();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedata.save");

        bf.Serialize(file,save);
        Debug.Log("Game Saved");
        file.Close();
    }

    //handles achievements, could've written better but I think it works well enough as it is
    public void AchievementHandling()
    {
        int FishCount = GameObject.FindGameObjectsWithTag("Fish").Count();

        if (FishCount >= 10)
        {
            GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_own_10_fish_at_once);
            
            if (FishCount >= 20)
            {
                GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_own_20_fish_at_once);
            }
                if (FishCount >= 30)
                {
                    GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_cluttered);
                }
        }

        if (GameObject.FindGameObjectsWithTag("Food").Count() >= GameObject.FindGameObjectsWithTag("Fish").Count() * 2 && GameObject.FindGameObjectsWithTag("Food").Count() >= 100)
        {
            GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_no_need_to_overfeed);
        }

        //2 parter
        if (FishCount == 0 && FishCount < FishAtStart)
        {
            GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_sans_is_waiting_offshore);
        }
    }
}