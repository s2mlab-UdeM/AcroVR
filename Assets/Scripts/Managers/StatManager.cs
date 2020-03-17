using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct InputData
{
    public float Velocity;
    public float Distance;
    public float Duration;
}

[System.Serializable]
public struct PlayerInfo
{
    public string Name;
    public string Id;
    public float Score;
    public InputData input;
}

public class StatManager : MonoBehaviour
{
    public PlayerInfo info;

    public void ProfileLoad(string fileName)
    {
        ReadDataFromJSON(fileName);
    }

    public void ProfileSave()
    {
        if (info.Name != null)
        {
            WriteDataToJSON(info.Name);
        }
    }

    private void WriteDataToJSON(string fileName)
    {
        string jsonData = JsonUtility.ToJson(info, true);
        File.WriteAllText(fileName, jsonData);
    }

    private void ReadDataFromJSON(string fileName)
    {
        string dataAsJson = File.ReadAllText(fileName);
        info = JsonUtility.FromJson<PlayerInfo>(dataAsJson);
    }
}
