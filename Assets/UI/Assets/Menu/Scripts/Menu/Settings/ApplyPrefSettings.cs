using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ApplyPrefSettings : MonoBehaviour
{
    public GameObject CameraAnchor;

    public GameObject MusicObject;
    public GameObject SFXObject;
    public GameObject RZDObject;

    public void ApplySettings()
    {
        // Save Music/SFX Volume
        Slider MusicSlider = MusicObject.GetComponentInChildren<Slider>();
        Slider SFXSlider = SFXObject.GetComponentInChildren<Slider>();
        PlayerPrefs.SetInt("MusicVolume", (int)MusicSlider.value);
        PlayerPrefs.SetInt("SFXVolume", (int)SFXSlider.value);

        // Save Rotation/Zoom Sens (and Direction) || Setting values for Rotation/Zoom Sens
        Slider RotationSlider = RZDObject.GetComponentsInChildren<Slider>().Single(t => t.name == "Rotation Slider");
        Slider ZoomSlider = RZDObject.GetComponentsInChildren<Slider>().Single(t => t.name == "Zoom Slider");
        Toggle InvertX = RZDObject.GetComponentsInChildren<Toggle>().Single(t => t.name == "Invert X");
        Toggle InvertY = RZDObject.GetComponentsInChildren<Toggle>().Single(t => t.name == "Invert Y");
        Toggle InvertZoom = RZDObject.GetComponentsInChildren<Toggle>().Single(t => t.name == "Invert Zoom");

        PlayerPrefs.SetFloat("RotationSpeed", RotationSlider.value);
        PlayerPrefs.SetFloat("ZoomSpeed", ZoomSlider.value);
        PlayerPrefs.SetFloat("InvertX", FloatParse(InvertX.isOn));
        PlayerPrefs.SetFloat("InvertY", FloatParse(InvertY.isOn));
        PlayerPrefs.SetFloat("InvertZoom", FloatParse(InvertZoom.isOn));

        // Sets the values into the script
        CameraAnchor.GetComponent<MouseTouchSim>().RotationSpeed = RotationSlider.value;
        CameraAnchor.GetComponent<MouseTouchSim>().ZoomSpeed = ZoomSlider.value;
        CameraAnchor.GetComponent<MouseTouchSim>().InvertX = FloatParse(InvertX.isOn);
        CameraAnchor.GetComponent<MouseTouchSim>().InvertY = FloatParse(InvertY.isOn);
        CameraAnchor.GetComponent<MouseTouchSim>().InvertZoom = FloatParse(InvertZoom.isOn);
    }

    // Converts float into parse based on the value
    private float FloatParse(bool a)
    {
        if (a) { return 1f; } else { return -1f; }
    }
}
