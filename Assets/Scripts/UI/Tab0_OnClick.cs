using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		OnClick() functions for Tab0 / Welcome
/// </summary>

public class Tab0_OnClick : MonoBehaviour
{
	// Variables
	// [Header("SectionTitle")]	[Tooltip("HighlightInfo")]
	private Tab5_OnClick tab5Function;

	///===///  Language Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Settings Panel
	#region		<-- TOP

	/// Load player settings
	public void LoadSettings_GameManager()
	{
		tab5Function.LoadSettings_GameManager();
	}

	public void SaveSettings_GameManager()
	{
		tab5Function.SaveSettings_GameManager();
	}

	/// Load play value
	public void LoadPlay_GameManager()
	{
		tab5Function.LoadPlay_GameManager();

	}

	/// Save play value
	public void SavePlay_GameManager()
	{
		tab5Function.SavePlay_GameManager();

	}

	#endregion		<-- BOTTOM

}