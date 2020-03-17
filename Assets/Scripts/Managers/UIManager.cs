using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    /// <summary>
    ///     All In-Game Menu
    /// </summary>

public class UIManager : MonoBehaviour {
	//****** Variables
	[Header("Top UI Tab Text")]
	[Tooltip("HI ARIANE... CCCANNN YOU SEE ME!!!!!")]
	public Text TabTitleActive;
	[Tooltip("Tip Text Here")]
	public Text TabTitleInactive;

	[Header("Movement Panel Text")]
	public Text movementText01;
	public Text movementText02;

	[Header("Take-Off Panel Text")]
	public Text takeOff01;
	[Tooltip("HI ARIANE... CCCANNN YOU SEE ME!!!!!")]
	public Text takeOff02;

	// Start
	void Start ()
	{
		
	}
	
	// Update
	void Update ()
	{
		
	}
}