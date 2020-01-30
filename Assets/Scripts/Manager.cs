using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//deprecated
public class Manager : MonoBehaviour
{
    //make singelton if useing multiple scenes

        public static Manager Instance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
