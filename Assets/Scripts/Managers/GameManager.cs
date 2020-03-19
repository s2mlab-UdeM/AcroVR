using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Crosstales.FB;

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
public class AnimationInfo
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
    public List<Nodes> nodes = new List<Nodes>();
}

public class GameManager : MonoBehaviour
{
    private MissionInfo mission;

    public void MissionLoad()
    {
        //        InitAnimationInfo();
        //        ReadDataFromJSON(fileName);
        //        missionName.text = mission.Name;

//          ReadAniFromJSON("walk.json");

        ExtensionFilter[] extensions = new[]
        {
            new ExtensionFilter(MainParameters.Instance.languages.Used.movementLoadDataFileTxtFile, "json"),
            new ExtensionFilter(MainParameters.Instance.languages.Used.movementLoadDataFileAllFiles, "*" ),
        };

        string dirSimulationFiles = Environment.ExpandEnvironmentVariables(@"\SimulationJson");

        string fileName = FileBrowser.OpenSingleFile(MainParameters.Instance.languages.Used.movementLoadDataFileTitle, dirSimulationFiles, extensions);
        if (fileName.Length <= 0)
            return;

//         ReadDataFiles_s(fileName);
        ReadAniFromJSON(fileName);
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

        jointsTemp.nodes = new MainParameters.StrucNodes[info.nodes.Count];

        for (int i = 0; i < info.nodes.Count; i++)
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

    private void WriteDataToJSON(string fileName)
    {
        AnimationInfo info = new AnimationInfo();

        info.Objective = "defalut";
        info.Duration = MainParameters.Instance.joints.duration;
        info.Condition = MainParameters.Instance.joints.condition;
        info.VerticalSpeed = MainParameters.Instance.joints.takeOffParam.verticalSpeed;
        info.AnteroposteriorSpeed = MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed;
        info.SomersaultSpeed = MainParameters.Instance.joints.takeOffParam.somersaultSpeed;
        info.TwistSpeed = MainParameters.Instance.joints.takeOffParam.twistSpeed;
        info.Tilt = MainParameters.Instance.joints.takeOffParam.tilt;
        info.Rotation = MainParameters.Instance.joints.takeOffParam.rotation;

        for (int i = 0; i < MainParameters.Instance.joints.nodes.Length; i++)
        {
            Nodes n = new Nodes();
            n.Name = MainParameters.Instance.joints.nodes[i].name;
            n.T = MainParameters.Instance.joints.nodes[i].T;
            n.Q = MainParameters.Instance.joints.nodes[i].Q;

            info.nodes.Add(n);
        }

        string jsonData = JsonUtility.ToJson(info, true);
        File.WriteAllText(fileName, jsonData);
    }

    public void WriteDataFiles_s(string fileName)
    {
        string fileLines = string.Format(
            "Duration: {0}{1}Condition: {2}{3}VerticalSpeed: {4:0.000}{5}AnteroposteriorSpeed: {6:0.000}{7}SomersaultSpeed: {8:0.000}{9}TwistSpeed: {10:0.000}{11}Tilt: {12:0.000}{13}Rotation: {14:0.000}{15}{16}",
            MainParameters.Instance.joints.duration, System.Environment.NewLine,
            MainParameters.Instance.joints.condition, System.Environment.NewLine,
            MainParameters.Instance.joints.takeOffParam.verticalSpeed, System.Environment.NewLine,
            MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed, System.Environment.NewLine,
            MainParameters.Instance.joints.takeOffParam.somersaultSpeed, System.Environment.NewLine,
            MainParameters.Instance.joints.takeOffParam.twistSpeed, System.Environment.NewLine,
            MainParameters.Instance.joints.takeOffParam.tilt, System.Environment.NewLine,
            MainParameters.Instance.joints.takeOffParam.rotation, System.Environment.NewLine, System.Environment.NewLine);

        fileLines = string.Format("{0}Nodes{1}DDL, name, interpolation (type, numIntervals, slopes), T, Q{2}", fileLines, System.Environment.NewLine, System.Environment.NewLine);

        for (int i = 0; i < MainParameters.Instance.joints.nodes.Length; i++)
        {
            fileLines = string.Format("{0}{1}:{2}:{3},{4},{5:0.000000},{6:0.000000}:", fileLines, i + 1, MainParameters.Instance.joints.nodes[i].name, MainParameters.Instance.joints.nodes[i].interpolation.type,
                MainParameters.Instance.joints.nodes[i].interpolation.numIntervals, MainParameters.Instance.joints.nodes[i].interpolation.slope[0], MainParameters.Instance.joints.nodes[i].interpolation.slope[1]);
            for (int j = 0; j < MainParameters.Instance.joints.nodes[i].T.Length; j++)
            {
                if (j < MainParameters.Instance.joints.nodes[i].T.Length - 1)
                    fileLines = string.Format("{0}{1:0.000000},", fileLines, MainParameters.Instance.joints.nodes[i].T[j]);
                else
                    fileLines = string.Format("{0}{1:0.000000}:", fileLines, MainParameters.Instance.joints.nodes[i].T[j]);
            }
            for (int j = 0; j < MainParameters.Instance.joints.nodes[i].Q.Length; j++)
            {
                if (j < MainParameters.Instance.joints.nodes[i].Q.Length - 1)
                    fileLines = string.Format("{0}{1:0.000000},", fileLines, MainParameters.Instance.joints.nodes[i].Q[j]);
                else
                    fileLines = string.Format("{0}{1:0.000000}:{2}", fileLines, MainParameters.Instance.joints.nodes[i].Q[j], System.Environment.NewLine);
            }
        }

        System.IO.File.WriteAllText(fileName, fileLines);
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

    public void SaveFile()
    {
        string dirSimulationFiles = Environment.ExpandEnvironmentVariables(@"\SimulationJson");

        string fileName = FileBrowser.SaveFile(MainParameters.Instance.languages.Used.movementSaveDataFileTitle, dirSimulationFiles, "DefaultFile", "json");
        if (fileName.Length <= 0)
            return;

        WriteDataToJSON(fileName);
        //        WriteDataFiles_s(fileName);
    }

    private float[] ExtractDataTQ(string values)
    {
        string[] subValues = Regex.Split(values, ",");
        float[] data = new float[subValues.Length];
        for (int i = 0; i < subValues.Length; i++)
            data[i] = float.Parse(subValues[i]);
        return data;
    }

    public void InterpolationDDL()
    {
        int n = (int)(MainParameters.Instance.joints.duration / MainParameters.Instance.joints.lagrangianModel.dt) + 1;
        float[] t0 = new float[n];
        float[,] q0 = new float[MainParameters.Instance.joints.lagrangianModel.nDDL, n];

        GenerateQ0 generateQ0 = new GenerateQ0(MainParameters.Instance.joints.lagrangianModel, MainParameters.Instance.joints.duration, 0, out t0, out q0);
        generateQ0.ToString();

        MainParameters.Instance.joints.t0 = MathFunc.MatrixCopy(t0);
        MainParameters.Instance.joints.q0 = MathFunc.MatrixCopy(q0);
    }

    public void DisplayDDL(int ddl, bool axisRange)
    {
        if (ddl >= 0)
        {
            transform.parent.GetComponentInChildren<AniGraphManager>().DisplayCurveAndNodes(0, ddl, true);
            if (MainParameters.Instance.joints.nodes[ddl].ddlOppositeSide >= 0)
            {
                transform.parent.GetComponentInChildren<AniGraphManager>().DisplayCurveAndNodes(1, MainParameters.Instance.joints.nodes[ddl].ddlOppositeSide, true);
            }
        }
    }
}
