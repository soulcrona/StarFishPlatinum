using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        // Every frame, it rotates the camera around the object
        transform.Rotate(0, Speed * Time.deltaTime, 0);
    }
}
