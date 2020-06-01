using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelBase : MonoBehaviour
{
    public abstract void SetPrefab(GameObject prefab);
    public abstract void CreateLevel();
}
