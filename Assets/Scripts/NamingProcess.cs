using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NamingProcess : MonoBehaviour
{
    public string naming;
    void Awake()
    {
        name = naming;
    }
}