using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		OnClick() functions for Tab2 / Take Off
/// </summary>

public class Tab2_OnClick : MonoBehaviour
{
	// Variables
	// [Header("SectionTitle")]	[Tooltip("HighlightInfo")]
	bool isTakeOff = false;

	///===///  OnClick() functions
	#region		<-- TOP

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

	///===///  Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

}