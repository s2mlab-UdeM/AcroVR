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
	public void LoadProfile_GameManager()
	{
		ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");

	}

	public void SaveProfile_GameManager()
	{
		ToolBox.GetInstance().GetManager<GameManager>().SaveFile();

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

		/// Fetching 3D avatar Spawnpoint Vector3
		Vector3 avatarVector3 = ToolBox.GetInstance().GetManager<DrawManager>().avatarVector3;
		/// Fetching 3D avatar reference
		GameObject avatar3D = ToolBox.GetInstance().GetManager<DrawManager>().girl1;

		/// Place 3D avatar to spawnpoint && Active 3D avatar
		if (avatar3D != null && avatar3D.activeSelf == false)
		{
			avatar3D.transform.position = avatarVector3;
			avatar3D.SetActive(true);
		}

	}

	/// Save play value
	public void SavePlay_GameManager()
	{
		ToolBox.GetInstance().GetManager<GameManager>().SaveFile();

	}

	#endregion		<-- BOTTOM

}