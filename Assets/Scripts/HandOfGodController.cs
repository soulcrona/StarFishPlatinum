using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Deprecated sadly, I loved this feature, but wasnt working out
public class HandOfGodController : MonoBehaviour
{
    public List<Transform> ObjectQueue;
    public Transform MovementTarget;
    public float Speed = 1f;
    public float SafeYDistanceUp = 16.5f;
    public float SafeYdistanceDown = 3.6f;

    public AudioSource audioSource;

    public bool IsRetreating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddToObjectQueue(Transform ThisObject)
    {
        ObjectQueue.Add(ThisObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectQueue.Count != 0 && IsRetreating == false)
        {
            SetTargets();
            MoveToTargets();
        }

        if(IsRetreating == true)
        {
            Retreat();
        }

        if(transform.position.y >= SafeYDistanceUp)
        {
            IsRetreating = false;
        }
        //Music();

        if (IsRetreating == false && MovementTarget == null)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void Music()
    {
        if (MovementTarget != null)
        {
            audioSource.Stop();
        }

        if (MovementTarget == null && IsRetreating == false)
        {
            audioSource.Play();
        }
    }

    private void SetTargets()
    {
        MovementTarget = ObjectQueue[0];
    }

    private void MoveToTargets()
    {
        if (transform.position.y > SafeYdistanceDown)
        {
            transform.position = new Vector3(MovementTarget.transform.position.x, transform.position.y,transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, MovementTarget.transform.position, Speed * Time.deltaTime);
        }
    }

    private void Retreat()
    {
        transform.Translate(0, 0, Speed * Time.deltaTime);
    }
    
    private void OnTriggerStay (Collider other)
    {
        if (MovementTarget != null)
        {
        if (MovementTarget.name == other.transform.name)
        {
            //For now
            if(transform.position.y <= 3.6)
            //if (transform.position.y <= other.transform.position.y + (other.transform.localScale.y * 2))
            {
                ObjectQueue.RemoveAt(0);
                switch (other.tag)
                {
                    case "Fish":
                        GameSystems.GlobalMoney += other.GetComponent<FishController>().Cost / 2;
                        break;

                    case "Enviroment":
                        GameSystems.GlobalMoney += other.GetComponent<EnvironmentController>().Cost / 2;
                        break;

                }

                Destroy(other.gameObject);
                IsRetreating = true;
                MovementTarget = null;
            }
        }

        }
    }
}
