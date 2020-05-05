using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level2 : MonoBehaviour
{
    private Button missionButton;
    private Button resultButton;
    private Button load2Button;
    private Button saveButton;
    private Button playButton;

    void Start () {
        ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();
        ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleFemale);
        ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();

        ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");

        missionButton = GameObject.Find("LoadButton").gameObject.GetComponent<Button>();
        load2Button = GameObject.Find("Load2Button").gameObject.GetComponent<Button>();
        resultButton = GameObject.Find("ResultButton").gameObject.GetComponent<Button>();
        saveButton = GameObject.Find("SaveButton").gameObject.GetComponent<Button>();
        playButton = GameObject.Find("PlayButton").gameObject.GetComponent<Button>();
    }
}
