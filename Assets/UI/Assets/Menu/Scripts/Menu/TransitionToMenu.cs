using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToMenu : MonoBehaviour
{
    public GameObject Anchor;
    public Camera MainCamera;

    public float MovementSpeed;
    public float RotationSpeed;

    private float mSpeed;
    private float rSpeed;
    private Quaternion GoalRotation = Quaternion.Euler(30, 0, 0);
    private Quaternion GoalRotationA = Quaternion.Euler(0, 0, 0);
    private Vector3 GoalLocation = new Vector3(0, 30, -50);
    private Transform transformCam;
    private Transform transformAnchor;

    void Start()
    {
        mSpeed = MovementSpeed * Time.deltaTime;
        rSpeed = RotationSpeed * Time.deltaTime;
        transformCam = MainCamera.transform;
        transformAnchor = Anchor.transform;
    }

    void Update()
    {
        // Checks if the position and the rotation of the camera is the
        // same as the goal rotation and location
        if (Anchor.transform.localRotation == GoalRotationA &&
            MainCamera.transform.localPosition == GoalLocation &&
            MainCamera.transform.localRotation == GoalRotation)
        {
            // Disables the components and then transitions into the game
            GetComponent<TransitionToMenu>().enabled = false;
            GetComponent<CameraRotator>().enabled = true;
            GetComponent<MouseTouchSim>().enabled = false;
        }
        // Every frame, this is updating the rotation and the location of the camera and its anchor
        Anchor.transform.localRotation = Quaternion.RotateTowards(transformAnchor.localRotation, Quaternion.Euler(0, 0, 0), rSpeed);
        MainCamera.transform.localRotation = Quaternion.RotateTowards(MainCamera.transform.localRotation, GoalRotation, rSpeed);
        MainCamera.transform.localPosition = Vector3.MoveTowards(transformCam.localPosition, GoalLocation, mSpeed);
    }
}
