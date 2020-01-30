using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInitSetup : MonoBehaviour
{
    // Fish
    public GameObject[] Fishes;
    public GameObject GuppyPriceLabel;
    public GameObject BlueTailPriceLabel;

    public GameObject GuppyPFT;
    public GameObject BlueTailPFT;

    // Market/Food
    public GameObject[] Foods;
    public GameObject PelletPriceLabel;
    public GameObject FlakePriceLabel;

    public GameObject PelletSustenance;
    public GameObject FlakeSustenance;

    void Awake()
    {
        // Sets the default values for the price and food display for the fishes
        // Fish Init
        GuppyPriceLabel.GetComponent<Text>().text = $"Price: ${Fishes[0].GetComponent<FishController>().fishObject.Cost}";
        BlueTailPriceLabel.GetComponent<Text>().text = $"Price: ${Fishes[1].GetComponent<FishController>().fishObject.Cost}";
        GuppyPFT.GetComponent<Text>().text = $"Preferred Food: {Foods[0].GetComponent<Food>().FoodType}";
        BlueTailPFT.GetComponent<Text>().text = $"Preferred Food: {Foods[1].GetComponent<Food>().FoodType}";

        // Market/Food Init
        PelletPriceLabel.GetComponent<Text>().text = $"Price: ${Foods[0].GetComponent<Food>().Cost}";
        FlakePriceLabel.GetComponent<Text>().text = $"Price: ${Foods[1].GetComponent<Food>().Cost}";
        PelletSustenance.GetComponent<Text>().text = $"Sustenance: {Foods[0].GetComponent<Food>().Sustenance}";
        FlakeSustenance.GetComponent<Text>().text = $"Sustenance: {Foods[1].GetComponent<Food>().Sustenance}";
    }
}
