using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		OnClick() functions for Tab4 / Results
/// </summary>

public class Tab4_OnClick : MonoBehaviour
{
	// Variables
	// [Header("SectionTitle")]	[Tooltip("HighlightInfo")]

	bool isTakeOff = false;
	bool isResultGraphOff = false;

	///===///  OnClick() functions
	#region		<-- TOP

	/// ResultGraphOn
	public void ResultGraphOn_AniGraphManager()
	{
		if (!isResultGraphOff)
		{
			isResultGraphOff = true;
			ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOn();
		}
		else
		{
			isResultGraphOff = false;
			ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
		}

		TakeOffGraphDisabled_AniGraphManager();

	}

	/// Disables Graph window
	public void TakeOffGraphDisabled_AniGraphManager()
	{
		/// Checkout this website to instanciate the graph into the object https://gamedev.stackexchange.com/questions/103760/how-to-instantiate-ui-image-in-unity2d
		if (!isTakeOff)
		{
			isTakeOff = true;
			ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOn();
		}
		else
		{
			isTakeOff = false;
			ToolBox.GetInstance().GetManager<AniGraphManager>().TaskOffGraphOff();
		}
	}

	#endregion		<-- BOTTOM

	///===///  Mission Objective Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Output Data Results Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

}