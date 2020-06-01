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

	public Text number;
	public CameraMovement camera;

	public Dropdown dropDownPlaySpeed;
	public Dropdown dropDownPlayMode;

	private void Start()
	{
		ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(3);
	}

	///===///  OnClick() play control buttons
	#region		<-- TOP

	/// Play avatar #1 sequence
	public void PlayAvatar1_DrawManager()
	{
		/*		if (number.text != "")
				{
					ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(1);

					float num = float.Parse(number.text);
					ToolBox.GetInstance().GetManager<GameManager>().InitAnimationInfo(num);
					ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleFemale);

					ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();

		//			camera.player = ToolBox.GetInstance().GetManager<DrawManager>().girl1Hip;
				}
				else
				{
					ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(3);
					ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();
				}*/

		if (dropDownPlayMode.captionText.text == MainParameters.Instance.languages.Used.animatorPlayModeSimulation)
			ToolBox.GetInstance().GetManager<DrawManager>().SimulationMode();
		else
			ToolBox.GetInstance().GetManager<DrawManager>().GestureMode();

		string playSpeed = dropDownPlaySpeed.captionText.text;
		if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow3)
			ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(10);
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow2)
			ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(3);
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow1)
			ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(1.5f);
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedNormal)
			ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(1);
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedFast)
			ToolBox.GetInstance().GetManager<DrawManager>().SetAnimationSpeed(0.8f);


		ToolBox.GetInstance().GetManager<DrawManager>().LoadAvatar(DrawManager.AvatarMode.SingleFemale);
		ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();
		ToolBox.GetInstance().GetManager<DrawManager>().PlayAvatar();
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
		//		ToolBox.GetInstance().GetManager<DrawManager>().ShowAvatar();		
		ToolBox.GetInstance().GetManager<DrawManager>().PlayAvatar();
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