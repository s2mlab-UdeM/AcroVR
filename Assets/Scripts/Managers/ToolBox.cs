using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     Access point for all Managers. Initiation of Static ToolBox and Manager logic
/// </summary>

public class ToolBox : MonoBehaviour {

    /// Must be static
    private static ToolBox _instance;

    Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

    /// Manager called and create logic. Must be static
    public static ToolBox GetInstance()
    {
        /// ToolBox duplicate check logic
        if (ToolBox._instance == null)
        {
            var go = new GameObject("Toolbox");
            DontDestroyOnLoad(go);
            ToolBox._instance = go.AddComponent<ToolBox>();
        }
        return ToolBox._instance;
    }

    /// Manager creation must be first call
    void Awake()
    {
        CreateAllManagers();
    }

    /// Creates all Managers
    private void CreateAllManagers()
    {
        /// ToolBox duplicate check logic
        if (ToolBox._instance != null)
        {
            Destroy(this);
        }

        /// Manager Listing. Must have a .cs script with name
        CreateManager<AniGraphManager>();
        CreateManager<GameManager>();
        CreateManager<StatManager>();
        CreateManager<DrawManager>();
        CreateManager<UIManager>();
        CreateManager<LoadSceneManager>();
    }

    /// Create GameObject. Add new Managers in CreateAllManagers()
    private void CreateManager<T>() where T : MonoBehaviour
    {
        var go = new GameObject(typeof(T).ToString());
        go.transform.parent = this.gameObject.transform;
        go.AddComponent<T>();
        dict.Add(typeof(T).ToString(), go);
    }

    /// When calling a Manager logic
    public T GetManager<T>() where T : MonoBehaviour
    {
        string key = typeof(T).ToString();
        return this.dict[key].GetComponent<T>();
    }
}