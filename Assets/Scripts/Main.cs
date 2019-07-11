using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
{
	public static Main Instance;
	public GameObject textVersionFR;
	public GameObject textVersionEN;
	public Text textButtonLanguage;

	public Text textTakeOffTitle;
	public Text textTakeOffSpeed;
	public Dropdown dropDownTakeOffCondition;
	public Text textTakeOffSomersaultPosition;
	public Text textTakeOffTilt;
	public Text textTakeOffHorizontalSpeed;
	public Text textTakeOffVerticalSpeed;
	public Text textTakeOffSomersaultSpeed;
	public Text textTakeOffTwistSpeed;

	public Text textMessagesTextTitle;

	//public GameObject panelMovement;
	//public GameObject panelTakeoffParameters;
	//public GameObject panelMessages;
	//public GameObject panelAnimator;
	//public GameObject panelTopButtons;
	//public GameObject panelOutOfDate;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;

		// Logiciel sera désactivé automatiquement après la date spécifié ci-dessous
		// Un fichier "bidon" est créé pour gérer les cas où l'utilisateur aurait modifié la date par la suite, alors le logiciel resterait désactivé quand même
		// Une façon simple de réactiver le logiciel est d'effacer le fichier "bidon"

		//string checkFileName = Application.persistentDataPath + @"/AcroVR.dll";
		//DateTime endDate = new DateTime(2019, 5, 1);									// Date = 1 mai 2019
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
			textVersionFR.SetActive(true);
			textVersionEN.SetActive(false);
		}
		else
		{
			MainParameters.Instance.languages.Used = MainParameters.Instance.languages.english;
			textButtonLanguage.text = "Fr";
			textVersionFR.SetActive(false);
			textVersionEN.SetActive(true);
		}

		MainParameters.StrucMessageLists languagesUsed =	MainParameters.Instance.languages.Used;

		// Section graphique du mouvement

		if (MainParameters.Instance.joints.nodes != null)
			MovementF.Instance.DisplayDDL(true, -1, false);
		GraphSettings.Instance.textVerticalAxisTitle.text = languagesUsed.movementGraphSettingsVerticalTitle;
		GraphSettings.Instance.textVerticalAxisLowerBound.text = languagesUsed.movementGraphSettingsLowerBound;
		GraphSettings.Instance.textVerticalAxisUpperBound.text = languagesUsed.movementGraphSettingsUpperBound;
		GraphSettings.Instance.textHorizontalAxisTitle.text = languagesUsed.movementGraphSettingsHorizontalTitle;
		GraphSettings.Instance.textHorizontalAxisLowerBound.text = languagesUsed.movementGraphSettingsLowerBound;
		GraphSettings.Instance.textHorizontalAxisUpperBound.text = languagesUsed.movementGraphSettingsUpperBound;
		GraphSettings.Instance.toggleGraphSettingsUpdateSimulation.GetComponentInChildren<Text>().text = languagesUsed.movementGraphSettingsUpdateSimulation;
		GraphSettings.Instance.buttonGraphSettingsDefaultValues.GetComponentInChildren<Text>().text = languagesUsed.movementGraphSettingsDefaultValuesButton;
		GraphSettings.Instance.buttonGraphSettingsCancel.GetComponentInChildren<Text>().text = languagesUsed.movementGraphSettingsCancelButton;

		// Section paramètres de décollage

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
		textTakeOffSomersaultPosition.text = languagesUsed.takeOffSomersaultPosition;
		textTakeOffTilt.text = languagesUsed.takeOffTilt;
		textTakeOffHorizontalSpeed.text = languagesUsed.takeOffHorizontal;
		textTakeOffVerticalSpeed.text = languagesUsed.takeOffVertical;
		textTakeOffSomersaultSpeed.text = languagesUsed.takeOffSomersaultSpeed;
		textTakeOffTwistSpeed.text = languagesUsed.takeOffTwist;

		// Section Résultats

		textMessagesTextTitle.text = languagesUsed.displayMsgTitle;

		// Section Animation

		dropDownOptions = new List<string>();
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedFast);
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedNormal);
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow1);
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow2);
		dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow3);
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
	/// <summary>
	/// Activer ou désactiver plusieurs contrôles (boutons, listes déroulantes, ...) du panneau principale. </summary>
	/// <param name="status">État des contrôles, activé ou non. </param>
	/// <param name="statusLoad">État du bouton Charger, modifié ou non. </param>

	public void EnableDisableControls(bool status, bool statusLoad)
	{
		Color color;
		if (status)
			color = Color.white;
		else
			color = Color.gray;

		MovementF.Instance.dropDownDDLNames.interactable = status;
		if (statusLoad)
		{
			MovementF.Instance.buttonLoad.interactable = status;
			MovementF.Instance.buttonLoadImage.color = color;
		}
		MovementF.Instance.dropDownInterpolation.interactable = false;				// Non fonctionnelle encore
		MovementF.Instance.buttonSave.interactable = status;
		MovementF.Instance.buttonSaveImage.color = color;
		MovementF.Instance.buttonSymetricLeftRight.interactable = false;            // Non fonctionnelle encore
		MovementF.Instance.buttonSymetricLeftRightImage.color = Color.gray;         // Non fonctionnelle encore
		MovementF.Instance.buttonASymetricLeftRight.interactable = status;
		MovementF.Instance.buttonASymetricLeftRightImage.color = color;
		MovementF.Instance.buttonGraphSettings.interactable = status;
		MovementF.Instance.buttonGraphSettingsImage.color = color;

		MovementF.Instance.dropDownCondition.interactable = status;
		MovementF.Instance.inputFieldSomersaultPosition.interactable = status;
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
