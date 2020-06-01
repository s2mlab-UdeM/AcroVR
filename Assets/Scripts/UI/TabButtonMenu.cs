using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		In-Game menu logic
/// </summary>

public class TabButtonMenu : MonoBehaviour
{
	///===/// Variables
	/// Developer tool to bypass Start behavior
	[Header("UI Developer Tool Bypass")]
	[Tooltip("Activate to bypass UI behavior")]
	public bool startBehaviour = false;
	[Tooltip("Profile check: not linked to outside function")]
	public bool playerProfile = false;
	[Tooltip("Settings check: not linked to outside function")]
	public bool playerSettings = false;


	///===/// Variables
	/// UI GameObjects
	[Header("Tab GameObjects")]
	[Tooltip("Drag and Drop GameObject with Button object and Panel object")]
	public GameObject tab0;
	public GameObject tab1;
	public GameObject tab2;
	public GameObject tab3;
	public GameObject tab4;
	public GameObject tab5;

	private List<GameObject> listTab = new List<GameObject>();

//	public GameObject verticalNumber;


	// Start
	void Start()
	{
		CreateTabList();
		//		DeveloperTool();
		//		ProfileCheckOnStart();

		Tab2();

	}

	// Update
	//void Update()
	//{

	//}

	public void GetManagerFunctions()
	{
		ToolBox.GetInstance().GetManager<StatManager>().ProfileLoad("Student1");
	}


	public void CreateTabList()
	{
		listTab.Add(tab0);
		listTab.Add(tab1);
		listTab.Add(tab2);
		listTab.Add(tab3);
		listTab.Add(tab4);
		listTab.Add(tab5);
	}


	///--- Player profile check logic
	public void ProfileCheckOnStart()
	{

		///--- Developer Tool function to bypass player profile check
		if (startBehaviour == false)
		{
			///--- Activate GameObject children "ButtonTab", Disable GameObject "Panel" in parent in GameObject Tab, found in List<> named listTab[]
			SetActiveTab();

			if (playerProfile == false && playerSettings == false)
			{
				Tab0();
			}

			/// Player profile and settings is true, activate LaunchWithProfile function. If preferred, replace with Tab function
			else
			{
				/// Origine: line 96
				LaunchWithProfile();
			}
		}

	}

	///--- Profile function
	/// If not useful, errase function and change line 87 to desired Tab
	public void LaunchWithProfile()
	{
		Tab1();
	}

	///--- Activate/Disable children in List listTab (GameObject Tab)
	public void SetActiveTab()
	{
		/// listTab Start behaviour
		foreach (GameObject tab in listTab)
		{
			/// Activate parent on start
			if (tab.transform != null)
			{
				tab.gameObject.SetActive(true);
			}

			/// Disable child's "Panels" on start
			if (tab.transform.Find("Panels") != null)
			{
				tab.transform.Find("Panels").gameObject.SetActive(false);

			}

			/// Activate child's "ButtonTab" on start
			if (tab.transform.Find("ButtonTab") != null)
			{
				tab.transform.Find("ButtonTab").gameObject.SetActive(true);

			}

			///	ARCHIVE: Optimized version
			///	if (tab.transform.GetChild(0).gameObject != null)
			///	{
			///		tab.transform.GetChild(0).gameObject.SetActive(false);
			///	}
		}
	}


	///===/// Tab switching function
	#region		<== TOP

	///--- Welcome Tab
	public void Tab0()
	{
		/// All comments apply to remaining Tab functions

		/// Activate "ButtonTab" & Disable "Panel" children in List listTab (GameObject Tab)
		SetActiveTab();

		/// Activate Element0 (parent)
		listTab[0].gameObject.SetActive(true);

		/// Activate Element0's Panel (child)
		listTab[0].transform.Find("Panels").gameObject.SetActive(true);

		///	ARCHIVE: Optimized version
		///	listTab[0].transform.GetComponentInChildren<GameObject>(true);
	}

	///--- Movement Tab
	public void Tab1()
	{
//		verticalNumber.SetActive(false);

		SetActiveTab();

		///--- Activate Element0 (parent)
		listTab[1].gameObject.SetActive(true);

		///--- Activate Element0's Panel (child)
		listTab[1].transform.Find("Panels").gameObject.SetActive(true);

	}

	///--- Take Off Tab
	public void Tab2()
	{
//		verticalNumber.SetActive(true);

		SetActiveTab();

		///--- Activate Element0 (parent)
		listTab[2].gameObject.SetActive(true);

		///--- Activate Element0's Panel (child)
		listTab[2].transform.Find("Panels").gameObject.SetActive(true);

	}

	///--- Replay Tab
	public void Tab3()
	{
//		verticalNumber.SetActive(false);

		SetActiveTab();

		///--- Activate Element0 (parent)
		listTab[3].gameObject.SetActive(true);

		///--- Activate Element0's Panel (child)
		listTab[3].transform.Find("Panels").gameObject.SetActive(true);

	}

	///--- Results Tab
	public void Tab4()
	{
//		verticalNumber.SetActive(false);

		SetActiveTab();

		///--- Activate Element0 (parent)
		listTab[4].gameObject.SetActive(true);

		///--- Activate Element0's Panel (child)
		listTab[4].transform.Find("Panels").gameObject.SetActive(true);

	}

	///--- Settings Tab
	public void Tab5()
	{
//		verticalNumber.SetActive(false);

		SetActiveTab();

		///--- Activate Element0 (parent)
		listTab[5].gameObject.SetActive(true);

		///--- Activate Element0's Panel (child)
		listTab[5].transform.Find("Panels").gameObject.SetActive(true);

	}

	#endregion		<== BOTTOM


	///===/// Developer Tool function
	#region		<== TOP

	///--- Launches tools on startup
	/// If required to remove Developer Tool function, Search/Find all "Developer Tool" references
	void DeveloperTool()
	{
		/// Found in ProfileCheckOnStart()
		if (startBehaviour == true)
		{
			playerProfile = true;
			playerSettings = true;
		}

		/// Print function
		DevBypassPrint();
	}

	///--- Print active Developer Tool options
	void DevBypassPrint()
	{
		/// If no DEVELOPER TOOL is active, don't print 
		if (startBehaviour || playerProfile || playerSettings)
		{
			print("UI DEVELOPER TOOL Active" + "\n" +
						/// Found in ProfileCheckOnStart()
						"BYPASS Start Behaviour: " + startBehaviour.ToString() +
						/// Found in SetActiveTab()
					"    BYPASS Player Profile: " + playerProfile.ToString() +
						/// Found in SetActiveTab()
					"    BYPASS Player Settings: " + playerSettings.ToString());
		}
	}

	#endregion		<== BOTTOM


}