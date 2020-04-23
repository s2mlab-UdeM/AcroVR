using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///		OnClick() functions for Tab5 / Settings
/// </summary>

public class Tab5_OnClick : MonoBehaviour
{
    /// Handler reference
    private Main mainClassHandler = null;


    [Header("Language Panel")]
    public LanguageSwitch.LanguageAvailable currentLanguageEnum;
    [Space]
    public bool isLanguageActive = true;
    /// Add new languages in Inspector
    public List<Button> languageButton;


    [Header("Point of View")]
    public PointOfView currentPOV;
    public enum PointOfView
    {
        POV1,
        POV3,
        POVSideView,
        None
    };
    [Space]
    public bool activatePOV1 = false;
    public Toggle togglePOV1 = null;
    public Text textPOV1 = null;
    public GameObject anchorFirstPOV = null;
    [Space]
    public bool activatePOV3 = false;
    public Toggle togglePOV3 = null;
    public Text textPOV3 = null;
    public GameObject anchorPOV3 = null;
    [Space]
    public bool activatePOVSideView = false;
    public Toggle togglePOVSideView = null;
    public Text textPOVSideView = null;
    public GameObject anchorPOVSideView = null;


    [Header("Tooltip Panel")]
    public bool toolOn = false;
    public Toggle tooltipOn = null;
    public Text tooltipOnText = null;
    public Toggle tooltipOff = null;
    public Text tooltipOffText = null;


    [Header("Peripherals Panel")]
    public Toggle togglePeripherals1;
    public Text textPeripherals1 = null;
    public Toggle togglePeripherals2;
    public Text textPeripherals2 = null;
    public bool peripheralsOn = true;


    [Header("Collapse Panel")]
    public Toggle toggleCollapse1;
    public Text textCollapse1;
    public Toggle toggleCollapse2;
    public Text textCollapse2;
    public bool collapseOn = true;


    [Header("Avatar Positioning Panel")]
    public Toggle toggleAvatarPos1;
    public Text textAvatarPos1;
    public Toggle toggleAvatarPos2;
    public Text textAvatarPos2;


    /// Decrarations of Handlers
    void Awake()
    {
        Main mainClass = GetComponent<Main>();
        //currentLanguageEnum = GetComponent<LanguageSwitch>().currentLanguage;


    }

    void Start()
    {

    }

    void Update()
    {
        POVFollow();
        //POVCamera1();
        //POVCamera3();
        //POVCameraSideView();
    }


    ///***  Language Panel
    #region		<-- TOP
    // Switch + enum created in LanguageSwitch.cs
    // no logic/function yet

    #endregion		<-- BOTTOM



    ///***  Tooltips Panel
    #region		<-- TOP
    public void ToggleTooltips()
    {
        if (toolOn == false && tooltipOn.isOn == true)
        {
            //mainClassHandler.ButtonToolTips();
            print("It is on");
            toolOn = true;
        }

        if (toolOn == true && tooltipOff.isOn == true)
        {
            //mainClassHandler.ButtonNoToolTips();
            print("It is off");
            toolOn = false;

        }

    }

    //public void ToggleTooltipsOff(Toggle toggle)
    //{
    //    toolOn = false;

    //    if (toolOn == false && toggle.isOn == true)
    //    {
    //        //mainClass.ButtonNoToolTips();
    //        print("It is off");

    //    }


    //}
    #endregion		<-- BOTTOM



    ///***  Point of View Panel
    #region		<-- TOP

    /// Linked to enum PointOfView
    public void POVCamera1()
    {

        print("before 1");

        if (currentPOV != PointOfView.POV1)
        {
            currentPOV = PointOfView.POV1;
            activatePOV1 = true;

        }
    }

    public void POVFollow()
    {
        if(activatePOV1 == true)
        {
            Camera.main.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            Camera.main.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;
        }
    }

    public void POVCamera3()
    {
        if (currentPOV != PointOfView.POV3)
        {
            currentPOV = PointOfView.POV3;
            activatePOV1 = false;

            Camera.main.transform.position = anchorPOV3.transform.position;
            Camera.main.transform.rotation = anchorPOV3.transform.rotation;
            print("3");

        }
    }

    public void POVCameraSideView()
    {
        if (currentPOV != PointOfView.POVSideView)
        {
            currentPOV = PointOfView.POVSideView;
            activatePOV1 = false;

            Camera.main.transform.position = anchorPOVSideView.transform.position;
            Camera.main.transform.rotation = anchorPOVSideView.transform.rotation;
            print("Side");

        }
    }


    #endregion		<-- BOTTOM



    ///***  Peripherals Panel
    #region		<-- TOP


    #endregion		<-- BOTTOM



    ///***  Collapse Panel
    #region		<-- TOP


    #endregion		<-- BOTTOM



    ///***  Avatar Positioning Panel
    #region		<-- TOP


    #endregion		<-- BOTTOM



    ///***  Settings Panel
    #region		<-- TOP
    /// Functions linked to Tab0_OnClick.cs

    /// Load player settings
    public void LoadSettings_GameManager()
    {
        LoadPlay_GameManager();
    }

    /// Save player settings
    public void SaveSettings_GameManager()
    {
        SavePlay_GameManager();

    }

    /// Load play value
    public void LoadPlay_GameManager()
    {
        ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();

        /// Fetching 3D avatar Spawnpoint Vector3
        Vector3 avatarVector3 = ToolBox.GetInstance().GetManager<DrawManager>().avatarVector3;
        /// Fetching 3D avatar reference
        GameObject avatar3D = ToolBox.GetInstance().GetManager<DrawManager>().girl1;

        /// Place 3D avatar to spawnpoint && Active 3D avatar
        if (avatar3D != null && avatar3D.activeSelf == false)
        {
            avatar3D.transform.position = avatarVector3;
            avatar3D.SetActive(true);
        }

    }

    /// Save play value
    public void SavePlay_GameManager()
    {
        ToolBox.GetInstance().GetManager<GameManager>().SaveFile();

    }

    #endregion		<-- BOTTOM

}