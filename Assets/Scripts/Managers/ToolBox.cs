using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBox : MonoBehaviour {

    private static ToolBox _instance;

    Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

    public static ToolBox GetInstance()
    {
        if (ToolBox._instance == null)
        {
            var go = new GameObject("Toolbox");
            DontDestroyOnLoad(go);
            ToolBox._instance = go.AddComponent<ToolBox>();
        }
        return ToolBox._instance;
    }

    void Awake()
    {
        if (ToolBox._instance != null)
        {
            Destroy(this);
        }

        CreateManager<AniGraphManager>();
        CreateManager<GameManager>();
        CreateManager<StatManager>();
        CreateManager<DrawManager>();
/*        CreateManager<ScoreManager>();
        CreateManager<LoadSceneManager>();
        CreateManager<ProfileManager>();*/
    }

    private void CreateManager<T>() where T : MonoBehaviour
    {
        var go = new GameObject(typeof(T).ToString());
        go.transform.parent = this.gameObject.transform;
        go.AddComponent<T>();
        dict.Add(typeof(T).ToString(), go);
    }

    public T GetManager<T>() where T : MonoBehaviour
    {
        string key = typeof(T).ToString();
        return this.dict[key].GetComponent<T>();
    }
}
