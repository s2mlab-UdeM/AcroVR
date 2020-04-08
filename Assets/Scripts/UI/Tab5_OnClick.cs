using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		OnClick() functions for Tab5 / Settings
/// </summary>

public class Tab5_OnClick : MonoBehaviour
{
	// Variables



	///===///  Language Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Tooltips Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Point of View Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Peripherals Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Collapse Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Avatar Positioning Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Settings Panel
	#region		<-- TOP

	/// Load player settings
	public void LoadSettings_GameManager()
	{
		ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();
	}

	/// Save player settings
	public void SaveSettings_GameManager()
	{
		ToolBox.GetInstance().GetManager<GameManager>().SaveFile();

	}

	/// Load play value
	public void LoadPlay_GameManager()
	{
		ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();
	}

	/// Save play value
	public void SavePlay_GameManager()
	{
		ToolBox.GetInstance().GetManager<GameManager>().SaveFile();

	}

	#endregion		<-- BOTTOM

}