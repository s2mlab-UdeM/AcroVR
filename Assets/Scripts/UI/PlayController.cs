using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///	 
/// </summary>

public class PlayController : MonoBehaviour
{
	// Variables
	[Range(1,5)]
	public int playSpeed = 3; /// 3 is the average / normal speed
	public bool isStopFixed = true;
	public GameObject avatar3D;

	bool isPaused = false;
	bool isTakeOff = false;


	///===///  OnClick() play control buttons
	#region		<-- TOP

	/// Play avatar #1 sequence
	public void PlayAvatar1_DrawManager()
	{
		ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(playSpeed);
        ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleFemale);
        ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();
	}

	/// Pause / un-pause avatar play sequence
	public void PauseAvatar_DrawManager()
	{
		isPaused = !isPaused;
		ToolBox.GetInstance().GetManager<DrawManager>().PauseAvatar(isPaused);
		
	}

	/// Stop avatar play sequence
	public void StopAvatar_DrawManager()
	{
		print("StopAvatar Not Done");

	}

	/// Replay avatar play sequence
	public void ReplayAvatar_DrawManager()
	{
        ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();
	}

	/// Play avatar #2 sequence
	public void RewindAvatar_DrawManager()
	{
		print("RewindAvatar Not Done");

	}

	/// Play avatar #2 sequence
	public void RewindSlowelyAvatar_DrawManager()
	{
		print("RewindSlowelyAvatar Not Done");

	}

	/// Play avatar #2 sequence
	public void ForwardAvatar_DrawManager()
	{
		print("ForwardAvatar Not Done");

	}

	/// Play avatar #2 sequence
	public void ForwardSlowelyAvatar_DrawManager()
	{
		print("ForwardSlowelyAvatar Not Done");

	}

	#endregion		<-- BOTTOM

	///===///  Scrollbar functions
	#region		<-- TOP


	#endregion		<-- BOTTOM
}