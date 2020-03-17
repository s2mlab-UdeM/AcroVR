using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text.RegularExpressions;


[System.Serializable]
public struct Goal
{
    public float Distance;
    public float Duration;
}

[System.Serializable]
public struct Solution
{
    public float Velocity;
}

[System.Serializable]
public struct MissionInfo
{
    public string Name;
    public Goal goal;
    public Solution solution;
}

[System.Serializable]
public struct Nodes
{
    public string Name;
    public float[] T;
    public float[] Q;
}

[System.Serializable]
public struct AnimationInfo
{
    public string Objective;
    public float Duration;
    public int Condition;
    public float VerticalSpeed;
    public float AnteroposteriorSpeed;
    public float SomersaultSpeed;
    public float TwistSpeed;
    public float Tilt;
    public float Rotation;
    public Nodes[] nodes;
}

public class GameManager : MonoBehaviour {

    private Text missionName;
    private Button missionButton;
    private InputField missionInput;

    private MissionInfo mission;

    void Awake()
    {
        Transform temp = GameObject.Find("Canvas").transform.GetChild(0);
        missionName = temp.GetComponent<Text>();

        temp = GameObject.Find("Canvas").transform.GetChild(2);
        missionButton = temp.GetComponent<Button>();
        missionButton.onClick.AddListener(MissionOnClick);

        temp = GameObject.Find("Canvas").transform.GetChild(1);
        missionInput = temp.GetComponent<InputField>();
    }

    public void MissionLoad(string fileName)
    {
        InitAnimationInfo();
        ReadDataFromJSON(fileName);
        missionName.text = mission.Name;

//        ReadAniFromJSON("test1.json");
//        ReadDataFiles_s("UnevenBars_BackDoubleFull.txt");
    }

    void MissionOnClick()
    {
        string v = missionInput.text;
        print(v);
    }

    private void ReadDataFromJSON(string fileName)
    {
        string dataAsJson = File.ReadAllText(fileName);
        mission = JsonUtility.FromJson<MissionInfo>(dataAsJson);
    }

    private void InitAnimationInfo()
    {
        MainParameters.StrucJoints jointsTemp = new MainParameters.StrucJoints();
        jointsTemp.fileName = null;
        jointsTemp.nodes = null;
        jointsTemp.duration = 0;
        jointsTemp.condition = 0;
        jointsTemp.takeOffParam.verticalSpeed = 0;
        jointsTemp.takeOffParam.anteroposteriorSpeed = 0;
        jointsTemp.takeOffParam.somersaultSpeed = 0;
        jointsTemp.takeOffParam.twistSpeed = 0;
        jointsTemp.takeOffParam.tilt = 0;
        jointsTemp.takeOffParam.rotation = 0;

        jointsTemp.nodes = new MainParameters.StrucNodes[6];

        for (int i = 0; i < 6; i++)
        {
            jointsTemp.nodes[i].ddl = i + 1;
            jointsTemp.nodes[i].name = null;
            jointsTemp.nodes[i].interpolation = MainParameters.Instance.interpolationDefault;
            jointsTemp.nodes[i].T = new float[] { 0, 0.0001f};
            jointsTemp.nodes[i].Q = new float[] { 0, 0.0f};
            jointsTemp.nodes[i].ddlOppositeSide = -1;
        }

        MainParameters.Instance.joints = jointsTemp;

        LagrangianModelSimple lagrangianModelSimple = new LagrangianModelSimple();
        MainParameters.Instance.joints.lagrangianModel = lagrangianModelSimple.GetParameters;
    }

    private void ReadAniFromJSON(string fileName)
    {
        string dataAsJson = File.ReadAllText(fileName);
        AnimationInfo info = JsonUtility.FromJson<AnimationInfo>(dataAsJson);

        MainParameters.StrucJoints jointsTemp = new MainParameters.StrucJoints();
        jointsTemp.fileName = fileName;
        jointsTemp.nodes = null;
        jointsTemp.duration = info.Duration;
        jointsTemp.condition = info.Condition;
        jointsTemp.takeOffParam.verticalSpeed = info.VerticalSpeed;
        jointsTemp.takeOffParam.anteroposteriorSpeed = info.AnteroposteriorSpeed;
        jointsTemp.takeOffParam.somersaultSpeed = info.SomersaultSpeed;
        jointsTemp.takeOffParam.twistSpeed = info.TwistSpeed;
        jointsTemp.takeOffParam.tilt = info.Tilt;
        jointsTemp.takeOffParam.rotation = info.Rotation;

        jointsTemp.nodes = new MainParameters.StrucNodes[info.nodes.Length];

        for (int i = 0; i < info.nodes.Length; i++)
        {
            jointsTemp.nodes[i].ddl = i + 1;
            jointsTemp.nodes[i].name = info.nodes[i].Name;
            jointsTemp.nodes[i].interpolation = MainParameters.Instance.interpolationDefault;
            jointsTemp.nodes[i].T = info.nodes[i].T;
            jointsTemp.nodes[i].Q = info.nodes[i].Q;
            jointsTemp.nodes[i].ddlOppositeSide = -1;
        }

        MainParameters.Instance.joints = jointsTemp;

        LagrangianModelSimple lagrangianModelSimple = new LagrangianModelSimple();
        MainParameters.Instance.joints.lagrangianModel = lagrangianModelSimple.GetParameters;
    }

    private void ReadDataFiles_s(string fileName)
    {
        string[] fileLines = System.IO.File.ReadAllLines(fileName);

        MainParameters.StrucJoints jointsTemp = new MainParameters.StrucJoints();
        jointsTemp.fileName = fileName;
        jointsTemp.nodes = null;
        jointsTemp.duration = 0;
        jointsTemp.condition = 0;
        jointsTemp.takeOffParam.verticalSpeed = 0;
        jointsTemp.takeOffParam.anteroposteriorSpeed = 0;
        jointsTemp.takeOffParam.somersaultSpeed = 0;
        jointsTemp.takeOffParam.twistSpeed = 0;
        jointsTemp.takeOffParam.tilt = 0;
        jointsTemp.takeOffParam.rotation = 0;

        string[] values;
        int ddlNum = -1;

        for (int i = 0; i < fileLines.Length; i++)
        {
            values = Regex.Split(fileLines[i], ":");
            if (values[0].Contains("Duration"))
            {
                jointsTemp.duration = float.Parse(values[1]);
                if (jointsTemp.duration == -999)
                    jointsTemp.duration = MainParameters.Instance.durationDefault;
            }
            else if (values[0].Contains("Condition"))
            {
                jointsTemp.condition = int.Parse(values[1]);
                if (jointsTemp.condition == -999)
                    jointsTemp.condition = MainParameters.Instance.conditionDefault;
            }
            else if (values[0].Contains("VerticalSpeed"))
            {
                jointsTemp.takeOffParam.verticalSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.verticalSpeed == -999)
                    jointsTemp.takeOffParam.verticalSpeed = MainParameters.Instance.takeOffParamDefault.verticalSpeed;
            }
            else if (values[0].Contains("AnteroposteriorSpeed"))
            {
                jointsTemp.takeOffParam.anteroposteriorSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.anteroposteriorSpeed == -999)
                    jointsTemp.takeOffParam.anteroposteriorSpeed = MainParameters.Instance.takeOffParamDefault.anteroposteriorSpeed;
            }
            else if (values[0].Contains("SomersaultSpeed"))
            {
                jointsTemp.takeOffParam.somersaultSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.somersaultSpeed == -999)
                    jointsTemp.takeOffParam.somersaultSpeed = MainParameters.Instance.takeOffParamDefault.somersaultSpeed;
            }
            else if (values[0].Contains("TwistSpeed"))
            {
                jointsTemp.takeOffParam.twistSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.twistSpeed == -999)
                    jointsTemp.takeOffParam.twistSpeed = MainParameters.Instance.takeOffParamDefault.twistSpeed;
            }
            else if (values[0].Contains("Tilt"))
            {
                jointsTemp.takeOffParam.tilt = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.tilt == -999)
                    jointsTemp.takeOffParam.tilt = MainParameters.Instance.takeOffParamDefault.tilt;
            }
            else if (values[0].Contains("Rotation"))
            {
                jointsTemp.takeOffParam.rotation = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.rotation == -999)
                    jointsTemp.takeOffParam.rotation = MainParameters.Instance.takeOffParamDefault.rotation;
            }
            else if (values[0].Contains("DDL"))
            {
                jointsTemp.nodes = new MainParameters.StrucNodes[fileLines.Length - i - 1];
                ddlNum = 0;
            }
            else if (ddlNum >= 0)
            {
                jointsTemp.nodes[ddlNum].ddl = int.Parse(values[0]);
                jointsTemp.nodes[ddlNum].name = values[1];
                jointsTemp.nodes[ddlNum].interpolation = MainParameters.Instance.interpolationDefault;
                int indexTQ = 2;
                if (values.Length > 5)
                {
                    string[] subValues;
                    subValues = Regex.Split(values[2], ",");
                    if (subValues[0].Contains(MainParameters.InterpolationType.CubicSpline.ToString()))
                        jointsTemp.nodes[ddlNum].interpolation.type = MainParameters.InterpolationType.CubicSpline;
                    else
                        jointsTemp.nodes[ddlNum].interpolation.type = MainParameters.InterpolationType.Quintic;
                    jointsTemp.nodes[ddlNum].interpolation.numIntervals = int.Parse(subValues[1]);
                    jointsTemp.nodes[ddlNum].interpolation.slope[0] = float.Parse(subValues[2]);
                    jointsTemp.nodes[ddlNum].interpolation.slope[1] = float.Parse(subValues[3]);
                    indexTQ++;
                }
                jointsTemp.nodes[ddlNum].T = ExtractDataTQ(values[indexTQ]);
                jointsTemp.nodes[ddlNum].Q = ExtractDataTQ(values[indexTQ + 1]);
                jointsTemp.nodes[ddlNum].ddlOppositeSide = -1;
                ddlNum++;
            }
        }

        MainParameters.Instance.joints = jointsTemp;

        LagrangianModelSimple lagrangianModelSimple = new LagrangianModelSimple();
        MainParameters.Instance.joints.lagrangianModel = lagrangianModelSimple.GetParameters;
    }

    private float[] ExtractDataTQ(string values)
    {
        string[] subValues = Regex.Split(values, ",");
        float[] data = new float[subValues.Length];
        for (int i = 0; i < subValues.Length; i++)
            data[i] = float.Parse(subValues[i]);
        return data;
    }
}
