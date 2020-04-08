using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///		OnClick() functions for Tab0 / Welcome
/// </summary>

public class Tab0_OnClick : MonoBehaviour
{
	// Variables


	///===///  Language Panel
	#region		<-- TOP


	#endregion		<-- BOTTOM

	///===///  Settings Panel
	#region		<-- TOP

	/// Load player settings
	public void LoadSettings_GameManager()
	{
		ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");

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