using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
///		OnClick() functions for Tab5 / Settings
/// </summary>

public class Tab5_OnClick : MonoBehaviour
{
	// Variables
	// [Header("SectionTitle")]	[Tooltip("HighlightInfo")]
	[Header("Main Script Reference")]
	public Main mainScript;

	[Header("Language Panel")]
	public List<Button> languageButton;

	[Header("Tooltip Panel")]
	public Button tooltipOn;
	
	public Text tooltipOnText;
	public Button tooltipOff;
	public Text tooltipOffText;
	public bool toolOn = true;

	[Header("Point of View Panel")]
	public Button pOV1;
	
	public Button pOV3;
	public Button pOVOther;

	[Header("Peripherals Panel")]
	public Button peripherals1;
	
	public Button peripherals2;
	public bool peripheralsOn = true;

	[Header("Collapse Panel")]
	public Button collapse1;
	
	public Button collapse2;
	public bool collapseOn = true;

	[Header("Avatar Positioning Panel")]
	public Button avatarPos1;
	
	public Button avatarPos2;

	///===///  Language Panel

	#region		<-- TOP
	//public bool boolcheck = false;
	//enum LanguageCheck { English, French, Other};
	//public int languageCheck = 3;

	//public void LanguageToggle()
	//{
	//	switch (languageCheck)
	//	{
	//		case 1:
	//		{
	//			print("");
	//		}
	//		break;
	//		case 2:
	//		{

	//		}
	//		break;
	//	}
	//}

	#endregion		<-- BOTTOM

	///===///  Tooltips Panel
	#region		<-- TOP

	//public Main toolTipsON;
	//public void ToggleTooltips()
	//{
	//	switch (languageCheck)
	//	{
	//		case 1:
	//		{
	//			print("");
	//		}
	//		break;
	//		case 2:
	//		{

	//		}
	//		break;
	//	}
	//}

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
	/// Functions linked to Tab0_OnClick.cs

	/// Load player settings
	public void LoadSettings_GameManager()
	{
		LoadPlay_GameManager();
	}

	/// Save player settings
	public void SaveSettings_GameManager()
	{
		SavePlay_GameManager();

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