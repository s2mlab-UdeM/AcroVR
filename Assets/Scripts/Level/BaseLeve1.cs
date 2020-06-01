using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLeve1 : LevelBase
{
//    private List<GameObject> prefabs = new List<GameObject>();
//    private List<Vector3> initPos = new List<Vector3>();

    private void Awake()
    {
//        initPos.Add(new Vector3(0, 0, 0));
    }

    public override void SetPrefab(GameObject _prefab)
    {
//        prefabs.Add(_prefab);
    }

    public override void CreateLevel()
    {
//        for (int i = 0; i < prefabs.Count; i++)
//        {
//            Instantiate(prefabs[i], initPos[i], Quaternion.identity);
//        }
    }
}
