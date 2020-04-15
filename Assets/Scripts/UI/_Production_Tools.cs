using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	 Functions for production
/// </summary>

public class _Production_Tools : MonoBehaviour
{
	// Variables
	public bool disableOnStart = false;


	void Start()
	{
	}

	void Update()
	{
		DisableOnStart();

	}


	///===///  Simple Functions
	#region		<-- TOP

	/// CommentEmpty
	void DisableOnStart()
	{
		if (disableOnStart == true)
		{
			transform.gameObject.SetActive(false);
		}
	}

	/// CommentEmpty
	void Empty2()
	{
		
	}

	/// CommentEmpty
	void Empty3()
	{
		
	}

	#endregion		<-- BOTTOM


	
}