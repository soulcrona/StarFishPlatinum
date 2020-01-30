using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseTouchSim : MonoBehaviour
{
    public GameObject CameraAnchor;
    public Camera MainCamera;

    private float rotatX = 0f;
    private float rotatY = 0f;
    private float posZ = 0f;
    private Vector3 orgRotat;
    private Vector3 orgPos;

    public float RotationSpeed = 1f;
    public float ZoomSpeed = 1f;
    public float InvertX;
    public float InvertY;
    public float InvertZoom;

    void Start()
    {
        // Grabs the values of the sensitivity and applies to user for rotation
        RotationSpeed = PlayerPrefs.GetFloat("RotationSpeed");
        ZoomSpeed = PlayerPrefs.GetFloat("ZoomSpeed");
        InvertX = PlayerPrefs.GetFloat("InvertX");
        InvertY = PlayerPrefs.GetFloat("InvertY");
        InvertZoom = PlayerPrefs.GetFloat("InvertZoom");

        orgRotat = CameraAnchor.transform.eulerAngles;
        orgPos = MainCamera.transform.position;
        posZ = orgPos.z;

        rotatX = orgRotat.x;
        rotatY = orgRotat.y;
        posZ = MainCamera.transform.position.z;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    switch (Input.touchCount)
                    {
                        case 1:
                            // Move the camera around the object based on touch movement
                            rotatX += Input.GetTouch(0).deltaPosition.y * Time.deltaTime * RotationSpeed * InvertX;
                            rotatY -= Input.GetTouch(0).deltaPosition.x * Time.deltaTime * RotationSpeed * InvertY;
                            // Clamp x to prevent swiping out of bounds
                            rotatX = Mathf.Clamp(rotatX, 0, 90);
                            // Transform Camera's current eulerAngles to new vector3 pos
                            CameraAnchor.transform.eulerAngles = new Vector3(rotatX, rotatY, 0f);
                            break;
                        case 2:
                            // Grab the touch of two fingers
                            Touch fingerOne = Input.GetTouch(0);
                            Touch fingerTwo = Input.GetTouch(1);
                            // Grabs the difference between the two finger's position and delta position
                            Vector2 prevPos1 = fingerOne.position - fingerOne.deltaPosition;
                            Vector2 prevPos2 = fingerTwo.position - fingerTwo.deltaPosition;
                            float prevMag = (prevPos1 - prevPos2).magnitude;
                            // Grabs the difference between each positioning of the finger
                            float currMag = (fingerOne.position - fingerTwo.position).magnitude;
                            // Grabs the overall difference
                            float diff = currMag - prevMag;
                            // Change FOV based on the overall difference (acting as an incremental value)
                            // and clamps the value to prevent going over or under bounds
                            MainCamera.fieldOfView += (diff * ZoomSpeed) * (0.01f * InvertZoom);
                            MainCamera.fieldOfView = Mathf.Clamp(MainCamera.fieldOfView, 5f, 30f);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
