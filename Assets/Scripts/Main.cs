﻿using UnityEngine;
using UnityEngine.UI;
using System;
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

	public GameObject panelMovement;
	public GameObject panelTakeoffParameters;
	public GameObject panelMessages;
	public GameObject panelAnimator;
	public GameObject panelTopButtons;
	public GameObject panelOutOfDate;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;

		// Logiciel sera désactivé automatiquement après la date spécifié ci-dessous
		// Un fichier "bidon" est créé pour gérer les cas où l'utilisateur aurait modifié la date par la suite, alors le logiciel resterait désactivé quand même
		// Une façon simple de réactiver le logiciel est d'effacer le fichier "bidon"

		//string checkFileName = Application.persistentDataPath + @"/AcroVR.dll";
		//DateTime endDate = new DateTime(2019, 5, 1);
		//if (DateTime.Today >= endDate)                                                  // Date spécifié passé
		//{
		//	System.IO.File.WriteAllText(checkFileName, "$$&*&@@@!!");                   // On modifie le fichier pour indiquer que le logiciel est désactivé
		//	panelMovement.SetActive(false);                                             // Désactivé tous les panneaux
		//	panelTakeoffParameters.SetActive(false);
		//	panelMessages.SetActive(false);
		//	panelAnimator.SetActive(false);
		//	panelTopButtons.SetActive(false);
		//	panelOutOfDate.SetActive(true);                                             // Activé le panneau pour indiquer que le logiciel est désactivé
		//}
		//else if (!System.IO.File.Exists(checkFileName))
		//	System.IO.File.WriteAllText(checkFileName, "$%&*&@@@!!");                   // Création du fichier à la première exécution
		//else
		//{
		//	string fileContents = System.IO.File.ReadAllText(checkFileName);
		//	if (fileContents.IndexOf("$$") >= 0)                                        // Date modifié et fichier modifié, alors logiciel est désactivé
		//	{
		//		panelMovement.SetActive(false);                                         // Désactivé tous les panneaux
		//		panelTakeoffParameters.SetActive(false);
		//		panelMessages.SetActive(false);
		//		panelAnimator.SetActive(false);
		//		panelTopButtons.SetActive(false);
		//		panelOutOfDate.SetActive(true);                                         // Activé le panneau pour indiquer que le logiciel est désactivé
		//	}
		//}
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

	// =================================================================================================================================================================
	/// <summary> Activer ou désactiver plusieurs contrôles (boutons, listes déroulantes, ...) du panneau principale. </summary>

	public void EnableDisableControls(bool status, bool mode)
	{
		Color color;
		if (status)
			color = Color.white;
		else
			color = Color.gray;

		if (mode)
		{
			MovementF.Instance.buttonLoad.interactable = status;
			MovementF.Instance.buttonLoadImage.color = color;
		}
		MovementF.Instance.dropDownDDLNames.interactable = status;
		MovementF.Instance.dropDownInterpolation.interactable = false;				// Non fonctionnelle encore
		MovementF.Instance.dropDownNumIntervals.interactable = false;               // Non fonctionnelle encore
		MovementF.Instance.buttonSave.interactable = false;                         // Non fonctionnelle encore
		MovementF.Instance.buttonSaveImage.color = Color.gray;                      // Non fonctionnelle encore

		MovementF.Instance.dropDownCondition.interactable = status;
		MovementF.Instance.inputFieldInitialRotation.interactable = status;
		MovementF.Instance.inputFieldTilt.interactable = status;
		MovementF.Instance.inputFieldHorizontalSpeed.interactable = status;
		MovementF.Instance.inputFieldVerticalSpeed.interactable = status;
		MovementF.Instance.inputFieldSomersaultSpeed.interactable = status;
		MovementF.Instance.inputFieldTwistSpeed.interactable = status;

		AnimationF.Instance.textChrono.text = "";
		AnimationF.Instance.dropDownPlayMode.interactable = status;
		AnimationF.Instance.dropDownPlayView.interactable = status;
		AnimationF.Instance.buttonPlay.interactable = status;
		AnimationF.Instance.buttonPlayImage.color = color;
		AnimationF.Instance.dropDownPlaySpeed.interactable = status;
		AnimationF.Instance.buttonGraph.interactable = false;                       // Non fonctionnelle encore
		AnimationF.Instance.buttonGraphImage.color = Color.gray;					// Non fonctionnelle encore
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
