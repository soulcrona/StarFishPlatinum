using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class UpdateVolume : MonoBehaviour
{
    public GameObject objAudioSource;
    public Text strVal;
    public Slider slider;

    private float orgVol;

    // Set value-to-text
    public void Start()
    {
        strVal.text = $"{slider.value}%";
    }

    // Update value-to-text if value changed
    public void UpdateValues()
    {
        objAudioSource.GetComponent<AudioSource>().volume = Volume(slider.value);
        strVal.text = $"{slider.value}%";
    }

    // Reverts the volume back if the user has not saved the changes for the volume
    public void RevertVolume()
    {
        objAudioSource.GetComponent<AudioSource>().volume = Volume(orgVol);
        slider.value = orgVol;
    }

    // Grabs the volume of the control and sets the slider value to it
    public void SetOrgVolume()
    {
        orgVol = slider.value;
    }

    // Converts the original value of the slider into a more appropriate value for the volume control
    private float Volume(float a)
    {
        return a / 100;
    }
}
