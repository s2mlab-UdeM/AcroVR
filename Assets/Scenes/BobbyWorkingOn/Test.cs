using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	/// <summary>
	///	 
	/// </summary>

public class Test : MonoBehaviour
{
	// Variables
	///===/// SummarySection
	[Header("ToolTipSection")]
	[Tooltip("HighlightToolTip")]
	public float myFloat;
	
	// Variables
	///===/// SummarySection
	

	void Start()
	{
        ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");
	}

	void Update()
	{
		
	}


	///===/// On frame initializations
	#region		<-- TOP

	///--- SummarySubSection
	/// Comment
	void Empty1()
	{
		
	}

	#endregion		<-- BOTTOM


	///===/// SummarySection
	#region		<-- TOP

	/// Comment
	void Empty3()
	{
		
	}

	#endregion		<-- BOTTOM


	
}