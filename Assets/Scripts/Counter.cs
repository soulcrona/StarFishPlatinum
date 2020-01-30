using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//basically deprecated, it was usable for the editor to see how much fish and what fish were in the editor at the time
public class Counter : MonoBehaviour
{
    public List<string> fishNames = new List<string>();

    public int FishCount;

    //test2
    public List<ExtraClasses> FishArray = new List<ExtraClasses>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLists();
        Count();

        FishCount = GameObject.FindGameObjectsWithTag("Fish").Length;
    }



    void UpdateLists()
    {
        var fishTypes = GameObject.FindGameObjectsWithTag("Fish").ToList();
        int i = 0;
        foreach (var fish in fishTypes)
        {
            if (!fishNames.Contains(fish.GetComponent<FishController>().SpeciesName))
            {
                fishNames.Add(fish.GetComponent<FishController>().SpeciesName);

                ExtraClasses Extra = new ExtraClasses
                {
                    SpeciesName = fish.GetComponent<FishController>().SpeciesName
                };

                FishArray.Add(Extra);
            }

            i++;
        }
        i = 0;
    }
    private void Count()
    {

        FishCount = GameObject.FindGameObjectsWithTag("Fish").Length;

        for (int i = 0; i < FishArray.Count; i++)
        {
            GameObject[] TempObject = GameObject.FindGameObjectsWithTag("Fish");

            TempObject = TempObject.Where(n => n.GetComponent<FishController>().SpeciesName == FishArray[i].SpeciesName).ToArray();

            FishArray[i].Count = TempObject.Length;
        }
    }
}
