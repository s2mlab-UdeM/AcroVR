using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		UI In-Game menu logic
/// </summary>

public class TabGameObjects : MonoBehaviour
{
	// Variables
	///===/// Dev Tools
	[Header("UI Developer Tool Bypass")]
	[Tooltip("Activate to bypass UI restrictiuons or logic")]
	public bool startBehaviour = false;
	[Tooltip("Profile check: not linked to outside function")]
	public bool playerProfile = false;
	[Tooltip("Settings check: not linked to outside function")]
	public bool playerSettings = false;


	public GameObject tabGameObjects;

	// Start
	void Start()
	{
		tabGameObjects = GameObject.Find("TabGameObjects");

		//DeveloperTool();
		//ProfileCheckOnStart();	

	}

	//// Update
	//void Update()
	//{

	//}


	/////===/// Developer Tool function
	//#region		<== TOP

	/////--- Launches tools on startup
	///// If required to remove Developer Tool function, Search/Find all "Developer Tool" references
	//void DeveloperTool()
	//{

	//	if (startBehaviour == true)
	//	{
	//		playerProfile = true;
	//		playerSettings = true;
	//	}

	//	/// Print function, line 60
	//	DevBypassPrint();
	//}

	/////--- Print active Developer Tool options
	//void DevBypassPrint()
	//{
	//	/// If no DEVELOPER TOOL is active, don't print 
	//	if (startBehaviour || playerProfile || playerSettings)
	//	{
	//		print("UI DEVELOPER TOOL Active" + "\n" +
	//					/// line 84
	//					"BYPASS Start Behaviour: " + startBehaviour.ToString() +
	//				/// line 89
	//				"    BYPASS Player Profile: " + playerProfile.ToString() +
	//				/// line 89
	//				"    BYPASS Player Settings: " + playerSettings.ToString());
	//	}
	//}

	//#endregion		<== BOTTOM


	/////--- Player profile check logic
	///// line 30
	//void ProfileCheckOnStart()
	//{

	//	///--- Developer Tool function to bypass player profile check
	//	if (startBehaviour == false)
	//	{
	//		///--- Activate GameObject children "Button", Disable GameObject "Panel" in parent in GameObject Tab, found in List<> named listTab[]. Origine: line 104
	//		SetActiveTab();

	//		if (playerProfile == false && playerSettings == false)
	//		{
	//			Tab0();
	//		}

	//		/// Player profile and settings is true, activate LaunchWithProfile function. If preferred, replace with Tab function, line 151  
	//		else
	//		{
	//			/// Origine: line 96
	//			LaunchWithProfile();
	//		}
	//	}

	//}

	/////--- Profile function
	///// If not useful, errase function and change line 87 to desired Tab
	//public void LaunchWithProfile()
	//{
	//	Tab1();

	//}

	/////--- Activate/Disable children in List listTab (GameObject Tab)
	//void SetActiveTab()
	//{

	//}


	/////===/// Tab switching function
	//#region		<== TOP

	/////--- Welcome Tab
	//public void Tab0()
	//{
	//	/// All comments apply to remaining Tab functions

	//	/// Activate "Button" & Disable "Panel" children in List listTab (GameObject Tab), line 113
	//	SetActiveTab();

	//	/// Activate Element0 (parent), line 116
	//	listTab[0].transform.gameObject.SetActive(true);

	//	/// Activate Element0's Panel (child), line 125
	//	listTab[0].transform.Find("Panels").transform.gameObject.SetActive(true);

	//	///	ARCHIVE: Optimized version
	//	///	listTab[0].transform.GetComponentInChildren<GameObject>(true);
	//}

	/////--- Movement Tab
	//public void Tab1()
	//{
	//	SetActiveTab();

	//	/// Activate Element0 (parent)
	//	listTab[1].transform.gameObject.SetActive(true);

	//	/// Activate Element0's Panel (child)
	//	listTab[1].transform.Find("Panels").transform.gameObject.SetActive(true);

	//}

	/////--- Take Off Tab
	//public void Tab2()
	//{
	//	SetActiveTab();

	//	/// Activate Element0 (parent)
	//	listTab[2].transform.gameObject.SetActive(true);

	//	/// Activate Element0's Panel (child)
	//	listTab[2].transform.Find("Panels").transform.gameObject.SetActive(true);

	//}

	/////--- Replay Tab
	//public void Tab3()
	//{
	//	SetActiveTab();

	//	/// Activate Element0 (parent)
	//	listTab[3].transform.gameObject.SetActive(true);

	//	/// Activate Element0's Panel (child)
	//	listTab[3].transform.Find("Panels").transform.gameObject.SetActive(true);

	//}

	/////--- Results Tab
	//public void Tab4()
	//{
	//	SetActiveTab();

	//	///--- Activate Element0 (parent)
	//	listTab[4].transform.gameObject.SetActive(true);

	//	///--- Activate Element0's Panel (child)
	//	listTab[4].transform.Find("Panels").transform.gameObject.SetActive(true);

	//}

	/////--- Settings Tab
	//public void Tab5()
	//{
	//	SetActiveTab();

	//	/// Activate Element0 (parent)
	//	listTab[5].transform.gameObject.SetActive(true);

	//	/// Activate Element0's Panel (child)
	//	listTab[5].transform.Find("Panels").transform.gameObject.SetActive(true);

	//}

	//#endregion		<== BOTTOM


	/////===/// Buttons for Tab0
	//#region		<== TOP

	/////--- OnClick() load mission file
	///// <summary>
	/////		UI In-Game menu logic
	///// </summary>
	//public void ButtonLoadMission()
	//{
	//	ToolBox.GetInstance().GetManager<GameManager>().MissionLoad();

	//	ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar(1);
	//}


	//#endregion		<== BOTTOM


}