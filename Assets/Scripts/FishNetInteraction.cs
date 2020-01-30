using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNetInteraction : MonoBehaviour
{
    public float Force = 20;
    Rigidbody rb = new Rigidbody();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if touch input on the net, push it away, give achievement
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                rb.constraints = RigidbodyConstraints.None;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit HitInfo;
                if (Physics.Raycast(ray, out HitInfo))
                {
                    if (HitInfo.transform.name == "NET")
                    {
                        Rigidbody rig = HitInfo.collider.GetComponent<Rigidbody>();
                        if (rig != null)
                        {
                            rig.AddForceAtPosition(ray.direction * Force, HitInfo.point, ForceMode.VelocityChange);
                            GooglePlayGamesScript.UnlockAchievements(GPGSIds.achievement_getting_wood);
                        }
                    }
                }
            }
        }
    }
}
