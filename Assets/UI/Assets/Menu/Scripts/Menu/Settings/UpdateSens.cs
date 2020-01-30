using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSens : MonoBehaviour
{
    public Slider RotationSlider;
    public Slider ZoomSlider;
    public Text RotationValue;
    public Text ZoomValue;

    // Set value-to-text to match current value
    void Start()
    {
        RotationValue.text = $"{RotationSlider.value}F";
        ZoomValue.text = $"{ZoomSlider.value}F";
    }

    // Update value-to-text if value changed
    public void UpdateValues()
    {
        RotationValue.text = $"{RotationSlider.value}F";
        ZoomValue.text = $"{ZoomSlider.value}F";
    }
}
