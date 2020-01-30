using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    public int secondtillAnimation;
    public bool animate;

    private Stopwatch time = new Stopwatch();
    private Animator anim;

    void Start()
    {
        // Grabs the animator component
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        time.Start();
        // Checks if the timer has elasped
        if (time.Elapsed.Seconds >= secondtillAnimation)
        {
            time.Stop();
            time.Reset();
            // Aniamtes the UI and disables the script
            anim.SetBool("animate", animate);
            GetComponent<UIAnimationController>().enabled = false;
        }
    }
}
