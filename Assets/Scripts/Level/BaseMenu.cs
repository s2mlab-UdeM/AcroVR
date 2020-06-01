using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMenu : LevelBase
{
    /*    private List<GameObject> prefabs = new List<GameObject>();
        private List<Vector3> initPos = new List<Vector3>();

        private void Awake()
        {
            initPos.Add(new Vector3(-4.89f, -1.18f, -3.3f));
        }*/

    private GameObject trampolin;

    private void Start()
    {
        trampolin = GameObject.Find("trampoline");
    }

    public override void SetPrefab(GameObject _prefab)
    {
//        prefabs.Add(_prefab);
    }

    public override void CreateLevel()
    {
//        trampolin = Instantiate(prefabs[0], initPos[0], Quaternion.identity);
    }

    private void Update()
    {
        if(trampolin != null)
            trampolin.transform.Rotate(Vector3.up * 20f * Time.deltaTime);
    }
}
