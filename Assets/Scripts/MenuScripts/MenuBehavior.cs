using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    ///     Quits Game from Build.
    /// </summary>

public class MenuBehavior : MonoBehaviour
{
	public GameObject buttonQuitApplication;
    
    public void QuitApplication()
    {
        Application.Quit();
    }
}