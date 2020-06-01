using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseProfile : LevelBase
{
    public Dropdown dropDownDDLNames;
    public Text resultText;
    public GameObject anchorSidePOV = null;
    public GameObject anchorThirdPOV = null;
    public Camera AvatarCamera;

    public override void SetPrefab(GameObject _prefab)
    {
    }

    public override void CreateLevel()
    {
    }

    public void BackToMenu()
    {
        ToolBox.GetInstance().GetManager<LevelManager>().GotoScreen("MainMenu");
    }

    public void ToProfile()
    {
        ToolBox.GetInstance().GetManager<LevelManager>().GotoScreen("Profile");
    }

    public void ToTraining()
    {
        ToolBox.GetInstance().GetManager<LevelManager>().GotoScreen("Training");
    }

    public void ToNextLevel()
    {
        ToolBox.GetInstance().GetManager<LevelManager>().NextLevel();
    }

    public void ToQuit()
    {
        Application.Quit();
    }

    public void ShowResultGraph()
    {
        ToolBox.GetInstance().GetManager<AniGraphManager>().ResultGraphOn();
    }

    public void ToBaseLevel1()
    {
        ToolBox.GetInstance().GetManager<LevelManager>().GotoScreen("BaseLevel1");
    }

    public void MissionLoad()
    {
        ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();

        if(ToolBox.GetInstance().GetManager<DrawManager>().setAvatar == DrawManager.AvatarMode.SingleFemale)
            ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleFemale);
        else
            ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleMale);

        TakeOffOn();
        InitDropdownDDLNames(0);

    }

    public void MissionLoad2()
    {
        ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();
        ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.DoubleFemale);

        TakeOffOn();
        InitDropdownDDLNames(0);
    }

    public void SaveFile()
    {
        ToolBox.GetInstance().GetManager<GameManager>().SaveFile();
    }

    public void TakeOffOn()
    {
        ToolBox.GetInstance().GetManager<AniGraphManager>().GraphOn();
    }

    public void TakeOffOff()
    {
        ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
    }

    public void InitDropdownDDLNames(int ddl)
    {
        List<string> dropDownOptions = new List<string>();
        for (int i = 0; i < MainParameters.Instance.joints.nodes.Length; i++)
        {
            if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLHipFlexion.ToLower())
                dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLHipFlexion);
            else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLKneeFlexion.ToLower())
                dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLKneeFlexion);
            else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLLeftArmFlexion.ToLower())
                dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLLeftArmFlexion);
            else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLLeftArmAbduction.ToLower())
                dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLLeftArmAbduction);
            else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLRightArmFlexion.ToLower())
                dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLRightArmFlexion);
            else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLRightArmAbduction.ToLower())
                dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLRightArmAbduction);
            else
                dropDownOptions.Add(MainParameters.Instance.joints.nodes[i].name);
        }
        dropDownDDLNames.ClearOptions();
        dropDownDDLNames.AddOptions(dropDownOptions);
        if (ddl >= 0)
        {
            dropDownDDLNames.value = ddl;
        }
    }

    public void DisplayDDL(int ddl, bool axisRange)
    {
        if (ddl >= 0)
        {
            ToolBox.GetInstance().GetManager<AniGraphManager>().DisplayCurveAndNodes(0, ddl, axisRange);
            if (MainParameters.Instance.joints.nodes[ddl].ddlOppositeSide >= 0)
            {
                ToolBox.GetInstance().GetManager<AniGraphManager>().DisplayCurveAndNodes(1, MainParameters.Instance.joints.nodes[ddl].ddlOppositeSide, axisRange);
            }
        }
    }

    public void DropDownDDLNamesOnValueChanged(int value)
    {
        DisplayDDL(value, true);
    }

    public void ShowResult()
    {
        resultText.text = ToolBox.GetInstance().GetManager<DrawManager>().DisplayMessage();
    }

    public void FirstPOVCamera()
    {
        AvatarCamera.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.position;
        AvatarCamera.transform.rotation = ToolBox.GetInstance().GetManager<DrawManager>().GetFirstViewTransform().transform.rotation;
    }

    public void ThirdPOVCamera()
    {
        AvatarCamera.transform.position = anchorThirdPOV.transform.position;
        AvatarCamera.transform.rotation = anchorThirdPOV.transform.rotation;
    }

    public void SidePOVCamera()
    {
        AvatarCamera.transform.position = anchorSidePOV.transform.position;
        AvatarCamera.transform.rotation = anchorSidePOV.transform.rotation;
    }

    public void PlayAvatar()
    {
        ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();
    }

    public void SetTab(int _num)
    {
        ToolBox.GetInstance().GetManager<UIManager>().SetCurrentTab(_num);
    }

    public void SetFrench()
    {
        MainParameters.Instance.languages.Used = MainParameters.Instance.languages.french;
    }

    public void SetEnglish()
    {
        MainParameters.Instance.languages.Used = MainParameters.Instance.languages.english;
    }

    public void SetTooltip(bool _flag)
    {
        ToolBox.GetInstance().GetManager<UIManager>().SetTooltip(_flag);
    }

    public void SetMaleAvatar()
    {
        ToolBox.GetInstance().GetManager<DrawManager>().setAvatar = DrawManager.AvatarMode.SingleMale;
    }

    public void SetFemaleAvatar()
    {
        ToolBox.GetInstance().GetManager<DrawManager>().setAvatar = DrawManager.AvatarMode.SingleFemale;
    }
}