using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefSettings : MonoBehaviour
{
    // Music/SFX GameObjects
    public GameObject MusicVolume;
    public GameObject BGM;
    public GameObject SFXVolume;
    public GameObject SFX;

    // Rotation/Zoom/Direction GameObject
    public GameObject RZDObject;

    void Awake()
    {
        // ** Initializes default settings if first time launching the application **
        if (!PlayerPrefs.HasKey("FirstStartup"))
        {
            // Music/SFX Volume
            PlayerPrefs.SetInt("MusicVolume", 100);
            PlayerPrefs.SetInt("SFXVolume", 100);
            // Zoom and Rotation Sens (and Direction)
            PlayerPrefs.SetFloat("ZoomSpeed", 5f);
            PlayerPrefs.SetFloat("RotationSpeed", 10f);
            PlayerPrefs.SetFloat("InvertX", -1);
            PlayerPrefs.SetFloat("InvertY", -1);
            PlayerPrefs.SetFloat("InvertZoom", -1);

            PlayerPrefs.SetInt("Environment", 0);

            PlayerPrefs.SetInt("Environment0", 1);
            PlayerPrefs.SetInt("Environment1", 0);
            PlayerPrefs.SetInt("Environment2", 0);
            PlayerPrefs.SetInt("Environment3", 0);
            PlayerPrefs.SetInt("Environment4", 0);
            PlayerPrefs.SetInt("Environment5", 0);
            PlayerPrefs.SetInt("Environment6", 0);

            // And then creates key
            PlayerPrefs.SetInt("FirstStartup", 1);
            PlayerPrefs.Save();
        }

        // ** Init **
        // AudioSource (Music and SFX)
        AudioSource MusicController = BGM.GetComponentInChildren<AudioSource>();
        AudioSource SFXController = SFX.GetComponentInChildren<AudioSource>();
        // Slider (Music and SFX)
        Slider MusicSlider = MusicVolume.GetComponentInChildren<Slider>();
        Slider SFXSlider = SFXVolume.GetComponentInChildren<Slider>();
        // Slider (Rotation and Zoom)
        Slider RotationSlider = RZDObject.GetComponentsInChildren<Slider>().Single(t => t.name == "Rotation Slider");
        Slider ZoomSlider = RZDObject.GetComponentsInChildren<Slider>().Single(t => t.name == "Zoom Slider");
        // Toggle (Invert X, Y and Zoom)
        Toggle InvertX = RZDObject.GetComponentsInChildren<Toggle>().Single(t => t.name == "Invert X");
        Toggle InvertY = RZDObject.GetComponentsInChildren<Toggle>().Single(t => t.name == "Invert Y");
        Toggle InvertZoom = RZDObject.GetComponentsInChildren<Toggle>().Single(t => t.name == "Invert Zoom");


        // ** Set appropriate values based on PlayerPref Settings **
        // Music/SFX Volume
        MusicController.volume = PlayerPrefs.GetInt("MusicVolume") / 100;
        SFXController.volume = PlayerPrefs.GetInt("SFXVolume") / 100;
        MusicSlider.value = PlayerPrefs.GetInt("MusicVolume");
        SFXSlider.value = PlayerPrefs.GetInt("SFXVolume");
        // Rotation/Zoom Sens and Direction
        RotationSlider.value = PlayerPrefs.GetFloat("RotationSpeed");
        ZoomSlider.value = PlayerPrefs.GetFloat("ZoomSpeed");
        InvertX.isOn = BoolParse(PlayerPrefs.GetFloat("InvertX"));
        InvertY.isOn = BoolParse(PlayerPrefs.GetFloat("InvertY"));
        InvertZoom.isOn = BoolParse(PlayerPrefs.GetFloat("InvertZoom"));
    }

    private bool BoolParse(float a)
    {
        if (a == 1f) { return true; } else { return false; }
    }
}
