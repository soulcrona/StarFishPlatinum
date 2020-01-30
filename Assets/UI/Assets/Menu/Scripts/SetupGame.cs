using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupGame : MonoBehaviour
{
    public Canvas MenuCanvas;
    public Canvas GameCanvas;
    public GameObject MenuCameraAnchor;
    public GameObject Gameplay;
    public GameObject Settings;
    public GameObject MenuMain;

    public GameObject MenuPanel;
    public GameObject FishSelectionPanel;
    public GameObject SellPanel;
    public GameObject MarketPanel;
    public GameObject EnvironmentPanel;

    // Initialize's the setup of the game
    void Start()
    {
        MenuCanvas.gameObject.SetActive(true);
        GameCanvas.gameObject.SetActive(false);
        MenuCameraAnchor.SetActive(true);
        Gameplay.SetActive(true);
        Settings.SetActive(false);
        MenuMain.SetActive(true);
        MenuPanel.SetActive(true);
        FishSelectionPanel.SetActive(true);
        SellPanel.SetActive(true);
        MarketPanel.SetActive(true);
        EnvironmentPanel.SetActive(true);
    }
}
