using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
{
	public static Main Instance;
	public Button animatorButtonPlay;
	public Image animatorButtonPlayImage;
	public Text textButtonLanguage;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;
		EnableDisableUserControls(false);
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
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Quitter a été appuyer. </summary>

	public void ButtonQuit()
	{
		Application.Quit();
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Quitter a été appuyer. </summary>

	public void	EnableDisableUserControls(bool enable)
	{
		animatorButtonPlay.interactable = enable;
		if (enable)
			animatorButtonPlayImage.color = Color.white;
		else
			animatorButtonPlayImage.color = Color.gray;
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
