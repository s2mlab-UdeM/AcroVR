using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTraining : LevelBase
{
    public override void SetPrefab(GameObject _prefab)
    {
    }

    public override void CreateLevel()
    {
    }

    bool isPaused = false;
    bool isTakeOff = false;

    bool bFirstView = false;

    void Start()
    {
        ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");
        ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(3);  // 1(fast) ~ 5(slow)
    }
}
