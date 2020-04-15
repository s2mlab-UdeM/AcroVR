using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		UI In-Game menu logic
/// </summary>

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiGO;


    void Start()
    {
        FindGameObjectsWithTag();


    }

    public void FindGameObjectsWithTag()
    {
        if (uiGO == null)
        {
            uiGO = GameObject.FindGameObjectWithTag("UI");

        }

    }
}