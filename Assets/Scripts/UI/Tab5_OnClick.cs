using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		OnClick() functions for Tab5 / Settings
/// </summary>

public class Tab5_OnClick : MonoBehaviour
{
    //[Header("Language Panel")]
    public List<Button> languageButton;

    [Header("Point of View")]
    public bool activateFirstPOV = false;
    public Button buttonFirstPOV = null;
    public GameObject anchorFirstPOV = null;

    [Space]
    public bool activateThirdPOV = true;
    public Button buttonThirdPOV = null;
    public GameObject anchorThirdPOV = null;
    
    [Space]
    public bool activateSidePOV = false;
    public Button buttonSidePOV = null;
    public GameObject anchorSidePOV = null;

    [Header("Tooltip Panel")]
    public bool toolOn = true;
    public Button tooltipOn = null;
    public Text tooltipOnText = null;
    public Button tooltipOff = null;
    public Text tooltipOffText = null;

    [Header("Peripherals Panel")]
    public Button peripherals1;
    public Button peripherals2;
    public bool peripheralsOn = true;

    [Header("Collapse Panel")]
    public Button collapse1;
    public Button collapse2;
    public bool collapseOn = true;

    [Header("Avatar Positioning Panel")]
    public Button avatarPos1;
    public Button avatarPos2;


    void Start()
    {
        //pOVAnchor2.transform.position = 
    }

    void Update()
    {
        FirstPOVCamera();
        ThirdPOVCamera();
        SidePOVCamera();

    }


    ///===///  Language Panel

    #region		<-- TOP
    //public bool boolcheck = false;
    //enum LanguageCheck { English, French, Other};
    //public int languageCheck = 3;

    //public void LanguageToggle()
    //{
    //	switch (languageCheck)
    //	{
    //		case 1:
    //		{
    //			print("");
    //		}
    //		break;
    //		case 2:
    //		{

    //		}
    //		break;
    //	}
    //}

    #endregion		<-- BOTTOM

    ///===///  Tooltips Panel
    #region		<-- TOP

    //public Main toolTipsON;
    //public void ToggleTooltips()
    //{
    //	switch (languageCheck)
    //	{
    //		case 1:
    //		{
    //			print("");
    //		}
    //		break;
    //		case 2:
    //		{

    //		}
    //		break;
    //	}
    //}

    #endregion		<-- BOTTOM

    ///===///  Point of View Panel
    #region		<-- TOP

    public void FirstPOVActicate()
    {
        activateFirstPOV = !activateFirstPOV;
        activateThirdPOV = false;
        activateSidePOV = false;

    }

    public void FirstPOVCamera()
    {
        if (activateFirstPOV == true)
        {
            Camera.main.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            Camera.main.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;

            //anchorFirstPOV.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            //anchorFirstPOV.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;

            //Camera.main.transform.position = anchorFirstPOV.transform.position;
            //Camera.main.transform.rotation = anchorFirstPOV.transform.rotation;
        }
    }

    public void ThirdPOVActicate()
    {
        activateThirdPOV = !activateThirdPOV;
        activateFirstPOV = false;
        activateSidePOV = false;

    }

    public void ThirdPOVCamera()
    {
        if (activateThirdPOV == true)
        {
            Camera.main.transform.position = anchorThirdPOV.transform.position;
            Camera.main.transform.rotation = anchorThirdPOV.transform.rotation;

            //anchorFirstPOV.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            //anchorFirstPOV.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;
        }
    }
    public void SidePOVActicate()
    {
        activateSidePOV = !activateSidePOV;
        activateFirstPOV = false;
        activateThirdPOV = false;

    }

    public void SidePOVCamera()
    {
        if (activateSidePOV == true)
        {

            Camera.main.transform.position = anchorSidePOV.transform.position;
            Camera.main.transform.rotation = anchorSidePOV.transform.rotation;

            //anchorFirstPOV.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
            //anchorFirstPOV.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;
        }
    }

    #endregion		<-- BOTTOM

    ///===///  Peripherals Panel
    #region		<-- TOP


    #endregion		<-- BOTTOM

    ///===///  Collapse Panel
    #region		<-- TOP


    #endregion		<-- BOTTOM

    ///===///  Avatar Positioning Panel
    #region		<-- TOP


    #endregion		<-- BOTTOM

    ///===///  Settings Panel
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
        ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleFemale);

        //        ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();

        /// Fetching 3D avatar Spawnpoint Vector3
        /*        Vector3 avatarVector3 = ToolBox.GetInstance().GetManager<DrawManager>().avatarVector3;
                /// Fetching 3D avatar reference
                GameObject avatar3D = ToolBox.GetInstance().GetManager<DrawManager>().girl1;

                /// Place 3D avatar to spawnpoint && Active 3D avatar
                if (avatar3D != null && avatar3D.activeSelf == false)
                {
                    avatar3D.transform.position = avatarVector3;
                    avatar3D.SetActive(true);
                }*/

    }

    /// Save play value
    public void SavePlay_GameManager()
    {
        ToolBox.GetInstance().GetManager<GameManager>().SaveFile();

    }

    #endregion		<-- BOTTOM

}