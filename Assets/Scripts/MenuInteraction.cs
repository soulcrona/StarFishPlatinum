using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    public GameObject CameraAnchor;
    public GameObject Bounds;
    public float Timer = 60;
    public float TimeOutTime = 60;

    public Camera MainCamera;
    private GameObject CurrentSelection;

    public GameObject MenuPanel;

    // UI Panels
    public GameObject FishSelectionPanel;
    public GameObject SellPanel;
    public GameObject MarketPanel;
    public GameObject EnvironmentPanel;
    public GameObject TopPanel;

    // Selling Fish System
    public GameObject[] SellFilterSelections;
    public FishObject[] SellFilters;
    private FishObject CurrentFilter;

    // Spawn Fish System
    public GameObject[] FishPanelSelections;
    public GameObject[] FishSelections;
    public GameObject FishObjectParent;
    private GameObject CurrentFishSelection;

    // Market/Food System
    public GameObject[] FoodSelections;
    public GameObject[] FoodPanelSelections;
    public GameObject FoodObjectParent;
    private GameObject CurrentFoodSelection;

    // Environment Systems
    public GameObject[] EnvironmentSelections;
    public Text LBLButton;
    public Text LBLEnvironmentName;
    public Text LBLEnvironmentPrice;
    public Text LBLEnvironmentStreet;
    public RawImage EnvironmentImage;
    public Texture[] EnvironmentImageSelections;
    private int EnvironmentSelectedInt;
    public float[] EnvironmentPrice;
    private bool[] HasPurchased = new bool[7];

    // UI
    public GameObject PauseGamePanel;
    public GameObject MenuMainPanel;
    public GameObject GameCanvas;

    // The current state of the Menu UI when a user clicks a button
    private int CurrentState = 0;

    public int DistanceFromCamera;

    //Adding in static constants to make it look nicer
    
    //touch pish to push away if not in menu
    public float PushForceMultiplier = 5;

    private bool IntParse(int val)
    {
        if (val == 1) { return true; } else { return false; }
    }

    private void Awake()
    {
        // Before game initializes, grabs the environment that it was last used by
        // the user before the game closed and sets it as the current environment
        EnvironmentSelections[PlayerPrefs.GetInt("Environment")].SetActive(true);
        // Iterates through a list containing all the environments and disables the one that is
        // not equal to the current used environment
        foreach (var environment in EnvironmentSelections.Where(t => t != EnvironmentSelections[PlayerPrefs.GetInt("Environment")]))
        {
            environment.SetActive(false);
        }
        // Iterates through a list of booleans which implies that the user has purchased or not
        // purchased an environment and initalizses it based on player data
        for (int i = 0; i < HasPurchased.Length; i++)
        {
            HasPurchased[i] = IntParse(PlayerPrefs.GetInt($"Environment{i}"));
        }
    }

    private void Start()
    {
        // Sets the current filter to nothing at the moment
        CurrentFilter = null;

        Bounds = GameObject.FindGameObjectWithTag("System");

        // Setup for the environment
        EnvironmentSetupInit();
    }

    void Update()
    {
        //CameraRotationTimeOut();
        switch (CurrentState)
        {
            // State - Sell Fish
            case 1:
                SellFish();
                break;
            // State - Buy/Spawn Fish
            case 2:
                SpawnFish();
                break;
            // State - Buy/Spawn Food
            case 3:
                SpawnFood();
                break;
            // State - Environment Change
            case 4:
                break;
            default:
                break;
        }

        if (CurrentState == 0 || CurrentState >= 4)
        {
            //broken
           // PushFish();
        }
    }

    private void PushFish()
    {
       if(Input.GetMouseButtonDown(0))
       // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit HitInfo;

            if (Physics.Raycast(ray, out HitInfo))
            {

                if (HitInfo.transform.tag == "Fish")
                {
                    Rigidbody rig = HitInfo.collider.GetComponent<Rigidbody>();

                    if (rig != null)
                    {
                        Debug.Log(rig.transform.name);
                        rig.isKinematic = false;
                        rig.useGravity = true;
                        Debug.Log(rig.useGravity.ToString());
                        rig.AddForceAtPosition(ray.direction * PushForceMultiplier, HitInfo.point, ForceMode.VelocityChange);

                        StartCoroutine(Wait(1f));
                        rig.isKinematic = true;
                        rig.useGravity = false;
                        Debug.Log(rig.useGravity.ToString());
                        //GooglePlayGamesScript.IncrementAchievements(GPGSIds.achievement_Bully);

                    }
                }
            }
        }
    }

    IEnumerator Wait(float WaitTimeSeconds)
    {
        yield return new WaitForSeconds(WaitTimeSeconds);
    }

    //private void CameraRotationTimeOut()
    //{
    //    Timer += Time.deltaTime;
    //    if (Timer >= TimeOutTime)
    //    {
    //        CameraAnchor.GetComponent<CameraRotator>().enabled = true;
    //    }
    //    else
    //    {
    //        CameraAnchor.GetComponent<CameraRotator>().enabled = false;
    //    }
    //    //reset timer for rotation
    //    if (Input.touchCount > 0)
    //    {
    //        Timer = 0;
    //    }
    //}

    #region Sell Fish
    // Current setup for the UI animation of "Sell"
    public void SFSetup()
    {
        var animSellPanel = SellPanel.GetComponent<Animator>();
        var animMenuPanel = MenuPanel.GetComponent<Animator>();

        if (animSellPanel.GetBool("animate"))
        {
            CurrentState = 0;
            animSellPanel.SetBool("animate", false);
            animMenuPanel.SetBool("animate", true);
        }
        else
        {
            CurrentState = 1;
            animSellPanel.SetBool("animate", true);
            animMenuPanel.SetBool("animate", false);
            ResetSelectedFilter();
        }
    }
    // Grabs the filter that has been selected by the user
    public void SelectedFilter()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Guppy BF":
                SellFilterSelections[0].SetActive(true);
                SellFilterSelections[1].SetActive(false);
                SellFilterSelections[2].SetActive(false);
                CurrentFilter = SellFilters[0];
                break;
            case "Blue Tail BF":
                SellFilterSelections[0].SetActive(false);
                SellFilterSelections[1].SetActive(true);
                SellFilterSelections[2].SetActive(false);
                CurrentFilter = SellFilters[1];
                break;
            case "No Filter B":
                SellFilterSelections[0].SetActive(false);
                SellFilterSelections[1].SetActive(false);
                SellFilterSelections[2].SetActive(true);
                CurrentFilter = null;
                break;
            default:
                break;
        }
    }
    // Resets the filter if the user leaves the UI
    public void ResetSelectedFilter()
    {
        SellFilterSelections[0].SetActive(false);
        SellFilterSelections[1].SetActive(false);
        SellFilterSelections[2].SetActive(true);
    }
    // Sells the fish by touch input, also applies the filter if been given.
    public void SellFish()
    {
        RaycastHit ObjHit = new RaycastHit();
        bool TouchIfHit = new bool();
        bool MouseIfHit = new bool();
        // Touch Input
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TouchIfHit = Physics.Raycast(MainCamera.ScreenPointToRay(Input.GetTouch(0).position), out ObjHit);
            }
        }
        // Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            MouseIfHit = Physics.Raycast(MainCamera.ScreenPointToRay(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), out ObjHit);
        }
        if (TouchIfHit || MouseIfHit)
        {
            switch (ObjHit.transform.tag)
            {
                case "Fish":
                    //Debug.Log("Fish found!");
                    FishController FishObj = ObjHit.transform.GetComponent<FishController>();
                    // Fish Filter
                    if (CurrentFilter != null)
                    {
                        if (CurrentFilter.SpeciesName != FishObj.SpeciesName)
                        {
                            Debug.Log("Filter does not match, will not sell!");
                            return;
                        }
                    }
                    float Price = (FishObj.Cost * FishObj.Length) / 2;
                    GameSystems.GlobalMoney += Mathf.Floor(Price);
                    Destroy(ObjHit.transform.gameObject);
                    break;
                case "Environment":
                    break;
                default:
                    break;
            }
        }
        else
        {
            //Debug.Log("Fish not found!");
        }
    }
    #endregion

    #region Buy/Spawn Fish
    // Current setup for the UI animation of "Buy"
    public void BSFSetup()
    {
        var animFishPanel = FishSelectionPanel.GetComponent<Animator>();
        var animMenuPanel = MenuPanel.GetComponent<Animator>();

        if (animFishPanel.GetBool("animate"))
        {
            CurrentState = 0;
            animFishPanel.SetBool("animate", false);
            animMenuPanel.SetBool("animate", true);
        }
        else
        {
            CurrentState = 2;
            animFishPanel.SetBool("animate", true);
            animMenuPanel.SetBool("animate", false);
        }
    }
    // Grabs the fish that has been selected by the user
    public void SelectedFish()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Guppy":
                FishPanelSelections[0].SetActive(true);
                FishPanelSelections[1].SetActive(false);
                CurrentSelection = FishSelections[0];
                break;
            case "Blue Tail":
                FishPanelSelections[0].SetActive(false);
                FishPanelSelections[1].SetActive(true);
                CurrentSelection = FishSelections[1];
                break;
            default:
                break;
        }
    }
    // Checks if the user in gameplay
    public bool IsInGameScene()
    {
        var BoundsScript = Bounds.GetComponent<GameSystems>();

        Vector3 InputPosition = new Vector3();

       // if (Input.touchCount > 0 || Input.GetMouseButton(0))
       // {

           // if (Input.GetTouch(0).phase == TouchPhase.Began)
           // {
             //   InputPosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, DistanceFromCamera));
           //     Debug.Log(InputPosition + "Mouse");
           // }

           // if(Input.GetMouseButtonDown(0))
           // {
                InputPosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceFromCamera));
                Debug.Log(InputPosition);
            //}

            if (InputPosition.x >= BoundsScript.LowerXBound && InputPosition.x <= BoundsScript.UpperXBound &&
                InputPosition.y >= BoundsScript.LowerYBound && InputPosition.y <= BoundsScript.UpperYBound 
                //&& InputPosition.z >= BoundsScript.LowerZBound && InputPosition.z <= BoundsScript.UpperZBound
                )
            {
                return true;
            }
       // }

        return false;
    }

    // Attempts to spawn the fish in the location of the touch input/mouse position
    // Deducts the total amount by the price of the fish if successful
    public void SpawnFish()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0) && IsInGameScene() == true)
        {
            if (GameSystems.GlobalMoney < CurrentSelection.GetComponent<FishController>().fishObject.Cost) { return; }
            GameSystems.GlobalMoney -= CurrentSelection.GetComponent<FishController>().fishObject.Cost;
            var InputPosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceFromCamera));
            GameObject objInstance = Instantiate(CurrentSelection, InputPosition, Quaternion.identity, FishObjectParent.transform.parent);
            objInstance.transform.SetParent(FishObjectParent.transform);
            // Particle Effect
            objInstance.AddComponent<ParticleSystem>();
            objInstance.AddComponent<BubbleFX>();
            GooglePlayGamesScript.IncrementAchievements(GPGSIds.achievement_the_fish_mogul, 1);
        }
        // Touch Input
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && IsInGameScene() == true && GameSystems.GlobalMoney >= CurrentSelection.GetComponent<FishController>().fishObject.Cost)
            {
                if (GameSystems.GlobalMoney < CurrentSelection.GetComponent<FishController>().fishObject.Cost) { return; }
                GameSystems.GlobalMoney -= CurrentSelection.GetComponent<FishController>().fishObject.Cost;
                Vector3 InputPosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, DistanceFromCamera));
                GameObject objInstance = Instantiate(CurrentSelection, InputPosition, Quaternion.identity, FishObjectParent.transform.parent);
                objInstance.transform.SetParent(FishObjectParent.transform);
                // Particle Effect
                objInstance.AddComponent<ParticleSystem>();
                objInstance.AddComponent<BubbleFX>();
                GooglePlayGamesScript.IncrementAchievements(GPGSIds.achievement_the_fish_mogul, 1);
            }
            else if (GameSystems.GlobalMoney < CurrentSelection.GetComponent<FishController>().fishObject.Cost)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Play(0);
                }
            }
        }
    }
    // Resets the selected fish if user leaves UI
    public void ResetSelectedFish()
    {
        foreach (var panel in FishPanelSelections) { panel.SetActive(false); }
        CurrentSelection = null;
    }
    #endregion

    #region Food Market
    // Current setup for the UI animation of "Food"
    public void MSetup()
    {
        var animMarketPanel = MarketPanel.GetComponent<Animator>();
        var animMenuPanel = MenuPanel.GetComponent<Animator>();

        if (animMarketPanel.GetBool("animate"))
        {
            CurrentState = 0;
            animMarketPanel.SetBool("animate", false);
            animMenuPanel.SetBool("animate", true);
        }
        else
        {
            CurrentState = 3;
            animMarketPanel.SetBool("animate", true);
            animMenuPanel.SetBool("animate", false);
        }
    }
    // Grabs the food that has been selected by the user
    public void SelectFood()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Fish Pellets":
                FoodPanelSelections[0].SetActive(true);
                FoodPanelSelections[1].SetActive(false);
                CurrentFoodSelection = FoodSelections[0];
                break;
            case "Fish Flakes":
                FoodPanelSelections[0].SetActive(false);
                FoodPanelSelections[1].SetActive(true);
                CurrentFoodSelection = FoodSelections[1];
                break;
            default:
                break;
        }
    }
    // Attempts to spawn the food in the location of the touch input/mouse position
    // Deducts the total amount by the price of the food if successful
    public void SpawnFood()
    {
        // Mouse Input
        if (Input.GetMouseButtonDown(0) && IsInGameScene() == true)
        {
            if (GameSystems.GlobalMoney < CurrentFoodSelection.GetComponent<Food>().Cost) { return; }
            GameSystems.GlobalMoney -= CurrentFoodSelection.GetComponent<Food>().Cost;
            var InputPosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, DistanceFromCamera));
            GameObject objInstance = Instantiate(CurrentFoodSelection, InputPosition, Quaternion.identity);
            objInstance.transform.SetParent(FoodObjectParent.transform);
        }
        // Touch Input
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && IsInGameScene() == true)
            {
                if (GameSystems.GlobalMoney < CurrentFoodSelection.GetComponent<Food>().Cost) { return; }
                GameSystems.GlobalMoney -= CurrentFoodSelection.GetComponent<Food>().Cost;
                var InputPosition = MainCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, DistanceFromCamera));
                GameObject objInstance = Instantiate(CurrentFoodSelection, InputPosition, Quaternion.identity, FoodObjectParent.transform.parent);
                objInstance.transform.SetParent(FoodObjectParent.transform);
            }
        }
    }
    // Resets the selected food if user leaves UI
    public void ResetSelectedFood()
    {
        foreach (var panel in FoodPanelSelections) { panel.SetActive(false); }
        CurrentFoodSelection = null;
    }
    #endregion

    #region Environment
    // Environment inital setup for animation UI
    public void ESetup()
    {
        var animEnvironmentPanel = EnvironmentPanel.GetComponent<Animator>();
        var animMenuPanel = MenuPanel.GetComponent<Animator>();

        if (animEnvironmentPanel.GetBool("animate"))
        {
            CurrentState = 0;
            animEnvironmentPanel.SetBool("animate", false);
            animMenuPanel.SetBool("animate", true);
        }
        else
        {
            CurrentState = 4;
            animEnvironmentPanel.SetBool("animate", true);
            animMenuPanel.SetBool("animate", false);
        }
    }
    // Returns street name based on the object of the name being parsed in
    private string GrabStreet(string name)
    {
        switch (name)
        {
            case "Empty":
                return "Street: Default";
            case "Kelp":
                return "Street: Kelp Lands";
            case "Patrick's House":
                return "Street: 120 Conch Street";
            case "Sandy's Dome":
                return "Street: 126 Conch Street";
            case "Squidward's House":
                return "Street: 122 Conch Street";
            case "Spongebob's House":
                return "Street: 124 Conch Street";
            case "Conch Street":
                return "Street: Conch Street";
            default:
                return "";
        }
    }
    // Initalizes the setup for the environment UI
    private void EnvironmentSetupInit()
    {
        GameSystems.EnvironmentName = EnvironmentSelections.First(t => t.activeSelf == true).name;
        EnvironmentSelectedInt = PlayerPrefs.GetInt("Environment");
        LBLEnvironmentName.text = $"Environment Name: {EnvironmentSelections[EnvironmentSelectedInt].name}";
        LBLEnvironmentPrice.text = $"Price: ${EnvironmentPrice[EnvironmentSelectedInt]}";
        LBLEnvironmentStreet.text = GrabStreet(EnvironmentSelections[EnvironmentSelectedInt].name);
        EnvironmentImage.texture = EnvironmentImageSelections[EnvironmentSelectedInt];
        if (IntParse(PlayerPrefs.GetInt($"Environment{EnvironmentSelectedInt}")))
        {
            LBLButton.text = "APPLY";
        }
        else
        {
            LBLButton.text = "BUY";
        }
    }
    // Navigation through the environment list
    public void NavigateLeft()
    {
        try
        {
            EnvironmentSelectedInt--;
            LBLEnvironmentName.text = $"Environment Name: {EnvironmentSelections[EnvironmentSelectedInt].name}";
            LBLEnvironmentPrice.text = $"Price: ${EnvironmentPrice[EnvironmentSelectedInt]}";
            LBLEnvironmentStreet.text = GrabStreet(EnvironmentSelections[EnvironmentSelectedInt].name);
            EnvironmentImage.texture = EnvironmentImageSelections[EnvironmentSelectedInt];
            if (IntParse(PlayerPrefs.GetInt($"Environment{EnvironmentSelectedInt}")))
            {
                LBLButton.text = "APPLY";
            }
            else
            {
                LBLButton.text = "BUY";
            }
        }
        catch (Exception)
        {
            EnvironmentSelectedInt++;
            return;
        }
    }
    public void NavigateRight()
    {
        try
        {
            EnvironmentSelectedInt++;
            LBLEnvironmentName.text = $"Environment Name: {EnvironmentSelections[EnvironmentSelectedInt].name}";
            LBLEnvironmentPrice.text = $"Price: ${EnvironmentPrice[EnvironmentSelectedInt]}";
            LBLEnvironmentStreet.text = GrabStreet(EnvironmentSelections[EnvironmentSelectedInt].name);
            EnvironmentImage.texture = EnvironmentImageSelections[EnvironmentSelectedInt];
            if (IntParse(PlayerPrefs.GetInt($"Environment{EnvironmentSelectedInt}")))
            {
                LBLButton.text = "APPLY";
            }
            else
            {
                LBLButton.text = "BUY";
            }
        }
        catch (Exception)
        {
            EnvironmentSelectedInt--;
            return;
        }
    }
    // Applies the environment, checks whether the user has already purchased or not
    public void ApplyEnvironment()
    {
        if (HasPurchased[EnvironmentSelectedInt])
        {
            EnvironmentSelections[EnvironmentSelectedInt].SetActive(true);
            foreach (var environment in EnvironmentSelections.Where(t => t != EnvironmentSelections[EnvironmentSelectedInt]))
            {
                environment.SetActive(false);
            }
            PlayerPrefs.SetInt("Environment", EnvironmentSelectedInt);
            PlayerPrefs.Save();
            GameSystems.EnvironmentName = EnvironmentSelections.First(t => t.activeSelf == true).name;
            return;
        }

        if (GameSystems.GlobalMoney < EnvironmentPrice[EnvironmentSelectedInt]) { return; }

        GameSystems.GlobalMoney -= EnvironmentPrice[EnvironmentSelectedInt];
        HasPurchased[EnvironmentSelectedInt] = true;
        LBLButton.text = "APPLY";
        PlayerPrefs.SetInt($"Environment{EnvironmentSelectedInt}", 1);
        EnvironmentSelections[EnvironmentSelectedInt].SetActive(true);
        foreach (var environment in EnvironmentSelections.Where(t => t != EnvironmentSelections[EnvironmentSelectedInt]))
        {
            environment.SetActive(false);
        }
        PlayerPrefs.SetInt("Environment", EnvironmentSelectedInt);
        PlayerPrefs.Save();
        GameSystems.EnvironmentName = EnvironmentSelections.First(t => t.activeSelf == true).name;
    }
    #endregion

    #region UI
    // Enum for menu state when in game or menu
    private enum UIMenuState
    {
        Game, Menu
    }
    private UIMenuState State;
    // Sets the state whenever a user is in the game or main settings
    public void SetState()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "Settings":
                State = UIMenuState.Game;
                break;
            case "Settings - Button":
                State = UIMenuState.Menu;
                break;
            default:
                break;
        }
    }
    // Returns the user to a certain state in the game depending on the last saved state
    public void ReturnToState()
    {
        switch (State)
        {
            case UIMenuState.Game:
                RenderChange();
                PauseGamePanel.SetActive(true);
                break;
            case UIMenuState.Menu:
                MenuMainPanel.SetActive(true);
                break;
            default:
                break;
        }
        State = UIMenuState.Menu;
    }
    // Returns the user back to main menu
    public void BackToMenu()
    {
        MenuPanel.GetComponent<Animator>().SetBool("animate", false);
        EnvironmentPanel.GetComponent<Animator>().SetBool("animate", false);
        MarketPanel.GetComponent<Animator>().SetBool("animate", false);
        FishSelectionPanel.GetComponent<Animator>().SetBool("animate", false);
        SellPanel.GetComponent<Animator>().SetBool("animate", false);
        TopPanel.GetComponent<Animator>().SetBool("animate", false);

        CameraAnchor.GetComponent<TransitionToMenu>().enabled = true;
        CameraAnchor.GetComponent<TransitionToGame>().enabled = false;
    }
    // Initializes the state of the UI when transitioning into game
    public void StateInit()
    {
        TopPanel.GetComponent<Animator>().SetBool("animate", true);
        MenuPanel.GetComponent<UIAnimationController>().enabled = true;
    }
    // Changes the rendering of the gameplay
    public void RenderChange()
    {
        if (GameCanvas.GetComponent<Canvas>().enabled)
        {
            GameCanvas.GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GameCanvas.GetComponent<Canvas>().enabled = true;
        }
    }
    // Shows leaderboard
    public void ShowLeaderboards()
    {
        GooglePlayGamesScript.ShowLeaderBoardUI();
    }
    #endregion
}
