using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
{
	public static Main Instance;
	public Text textButtonLanguage;

	public Text textTakeOffTitle;
	public Text textTakeOffSpeed;
	public Dropdown dropDownTakeOffCondition;
	public Text textTakeOffInitialRotation;
	public Text textTakeOffTilt;
	public Text textTakeOffHorizontalSpeed;
	public Text textTakeOffVerticalSpeed;
	public Text textTakeOffSomersaultSpeed;
	public Text textTakeOffTwistSpeed;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Changement de langue utilisée a été appuyer. </summary>

	public void ButtonLanguage()
	{

		if (textButtonLanguage.text == "Fr")
		{
			MainParameters.Instance.languages.Used = MainParameters.Instance.languages.french;
			textButtonLanguage.text = "En";
		}
		else
		{
			MainParameters.Instance.languages.Used = MainParameters.Instance.languages.english;
			textButtonLanguage.text = "Fr";
		}

		MainParameters.StrucMessageLists languagesUsed =	MainParameters.Instance.languages.Used;

		textTakeOffTitle.text = languagesUsed.takeOffTitle;
		textTakeOffSpeed.text = languagesUsed.takeOffTitleSpeed;
		List<string> dropDownOptions = new List<string>();
		dropDownOptions.Add(languagesUsed.takeOffConditionNoGravity);
		dropDownOptions.Add(languagesUsed.takeOffConditionTrampolining);
		dropDownOptions.Add(languagesUsed.takeOffConditionTumbling);
		dropDownOptions.Add(languagesUsed.takeOffConditionDiving1m);
		dropDownOptions.Add(languagesUsed.takeOffConditionDiving3m);
		dropDownOptions.Add(languagesUsed.takeOffConditionDiving5m);
		dropDownOptions.Add(languagesUsed.takeOffConditionDiving10m);
		dropDownOptions.Add(languagesUsed.takeOffConditionHighBar);
		dropDownOptions.Add(languagesUsed.takeOffConditionUnevenBars);
		dropDownOptions.Add(languagesUsed.takeOffConditionVault);
		dropDownTakeOffCondition.ClearOptions();
		dropDownTakeOffCondition.AddOptions(dropDownOptions);
		textTakeOffInitialRotation.text = languagesUsed.takeOffInitialRotation;
		textTakeOffTilt.text = languagesUsed.takeOffTilt;
		textTakeOffHorizontalSpeed.text = languagesUsed.takeOffHorizontal;
		textTakeOffVerticalSpeed.text = languagesUsed.takeOffVertical;
		textTakeOffSomersaultSpeed.text = languagesUsed.takeOffSomersault;
		textTakeOffTwistSpeed.text = languagesUsed.takeOffTwist;

		dropDownOptions = new List<string>();
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedFast);
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedNormal);
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow);
		AnimationF.Instance.dropDownPlaySpeed.ClearOptions();
		AnimationF.Instance.dropDownPlaySpeed.AddOptions(dropDownOptions);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Quitter a été appuyer. </summary>

	public void ButtonQuit()
	{
		Application.Quit();
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
