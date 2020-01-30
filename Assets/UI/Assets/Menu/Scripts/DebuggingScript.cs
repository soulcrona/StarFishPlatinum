using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class DebuggingScript : MonoBehaviour
{
    public GameObject Anchor;
    public Camera MainCamera;

    void Update()
    {
        ClearLog();
        Debug.Log($"MAIN CAMERA || X: {MainCamera.transform.position.x} || Y: {MainCamera.transform.position.y} || Z: {MainCamera.transform.position.z}");
        Debug.Log($"ANCHOR || X: {Anchor.transform.position.x} || Y: {Anchor.transform.position.y} || Z: {Anchor.transform.position.z}");
        Debug.Log($"LOCAL MAIN CAMERA || X: {MainCamera.transform.localPosition.x} || Y: {MainCamera.transform.localPosition.y} || Z: {MainCamera.transform.localPosition.z}");
        Debug.Log($"LOCAL ANCHOR || X: {Anchor.transform.localPosition.x} || Y: {Anchor.transform.localPosition.y} || Z: {Anchor.transform.localPosition.z}");
    }

    static void ClearLog()
    {
        Type.GetType("UnityEditor.LogEntries,UnityEditor.dll")
            .GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)
            .Invoke(null, null);
    }
}
