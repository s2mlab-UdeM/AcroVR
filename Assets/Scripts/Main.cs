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
	public Button buttonToolTips;
	public Image buttonToolTipsImage;
	public GameObject buttonNoToolTips;
	public Image buttonNoToolTipsImage;
	public Button buttonLanguage;
	public Text textButtonLanguage;
	public GameObject buttonQuit;

	public Text textTakeOffTitle;
	public Text textTakeOffSpeed;
	public Dropdown dropDownTakeOffCondition;
	public Text textTakeOffInitialPosture;
	public Text textTakeOffSomersaultPosition;
	public Text textTakeOffTilt;
	public Text textTakeOffHorizontalSpeed;
	public Text textTakeOffVerticalSpeed;
	public Text textTakeOffSomersaultSpeed;
	public Text textTakeOffTwistSpeed;

	public Text textMessagesTextTitle;

	public GameObject panelMovement;				// Utilisées pour désactiver automatiquement le logiciel après une date spécifiée
	public GameObject panelTakeoffParameters;
	public GameObject panelMessages;
	public GameObject panelAnimator;
	public GameObject panelTopButtons;
	public GameObject panelOutOfDate;

	public bool toolTipsON;

	public bool testXSensUsed;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;
		toolTipsON = !testXSensUsed;
		MainParameters.Instance.testXSensUsed = testXSensUsed;

		// S'assurer que le séparateur décimal est bien un point et non une virgule

		System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
		nfi.NumberDecimalSeparator = ".";
		System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
		ci.NumberFormat = nfi;
		System.Threading.Thread.CurrentThread.CurrentCulture = ci;

		// Logiciel sera désactivé automatiquement après la date spécifiée ci - dessous
		// Un fichier "bidon" est créé pour gérer les cas où l'utilisateur aurait modifié la date par la suite, alors le logiciel resterait désactivé quand même
		// Une façon simple de réactiver le logiciel est d'effacer le fichier "bidon"

		//#if UNITY_STANDALONE_OSX
		//		string dirCheckFileName = string.Format("{0}/Documents/AcroVR/Lib", Environment.GetFolderPath(Environment.SpecialFolder.Personal));
		//		string checkFileName = string.Format("{0}/AcroVR.dll", dirCheckFileName);
		//#else
		//		string checkFileName = Application.persistentDataPath + @"/AcroVR.dll";
		//#endif
		//		DateTime endDate = new DateTime(2019, 12, 6);                                   // Date = 6 déc 2019
		//		if (DateTime.Today >= endDate)                                                  // Date spécifié passé
		//		{
		//			System.IO.File.WriteAllText(checkFileName, "$$&*&@@@!!");                   // On modifie le fichier pour indiquer que le logiciel est désactivé
		//			panelMovement.SetActive(false);                                             // Désactivé tous les panneaux
		//			panelTakeoffParameters.SetActive(false);
		//			panelMessages.SetActive(false);
		//			panelAnimator.SetActive(false);
		//			panelTopButtons.SetActive(false);
		//			panelOutOfDate.SetActive(true);                                             // Activé le panneau pour indiquer que le logiciel est désactivé
		//		}
		//		else if (!System.IO.File.Exists(checkFileName))
		//		{
		//#if UNITY_STANDALONE_OSX
		//			System.IO.Directory.CreateDirectory(dirCheckFileName);                      // Création du répertoire où sera le fichier, obligatoire pour Mac
		//#endif
		//			System.IO.File.WriteAllText(checkFileName, "$%&*&@@@!!");                   // Création du fichier à la première exécution
		//		}
		//		else
		//		{
		//			string fileContents = System.IO.File.ReadAllText(checkFileName);
		//			if (fileContents.IndexOf("$$") >= 0)                                        // Date modifié et fichier modifié, alors logiciel est désactivé
		//			{
		//				panelMovement.SetActive(false);                                         // Désactivé tous les panneaux
		//				panelTakeoffParameters.SetActive(false);
		//				panelMessages.SetActive(false);
		//				panelAnimator.SetActive(false);
		//				panelTopButtons.SetActive(false);
		//				panelOutOfDate.SetActive(true);                                         // Activé le panneau pour indiquer que le logiciel est désactivé
		//			}
		//		}
	}

	// =================================================================================================================================================================
	// Quand le logiciel se termine, alors il faut supprimer les libraries DLL chargées en mémoire

	void OnDestroy()
	{
		if (testXSensUsed)
			MainParameters.Instance.FreeLib();
	}

	// =================================================================================================================================================================
	/// <summary> Bouton ToolTips a été appuyer. </summary>

	public void ButtonToolTips()
	{
		toolTipsON = false;
		buttonToolTips.gameObject.SetActive(false);
		buttonNoToolTips.SetActive(true);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton NoToolTips a été appuyer. </summary>

	public void ButtonNoToolTips()
	{
		toolTipsON = true;
		buttonToolTips.gameObject.SetActive(true);
		buttonNoToolTips.SetActive(false);
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
		{
			MovementF.Instance.DisplayDDL(-1, false);
			MovementF.Instance.InitDropdownDDLNames(-1);
			MovementF.Instance.InitDropdownInterpolation(-1);
			if (MainParameters.Instance.joints.nodes[GraphManager.Instance.ddlUsed].ddlOppositeSide >= 0)
			{
				MovementF.Instance.textCurveName1.text = languagesUsed.leftSide;
				MovementF.Instance.textCurveName2.text = languagesUsed.rightSide;
			}
		}
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
		textTakeOffInitialPosture.text = languagesUsed.takeOffInitialPosture;
		textTakeOffSomersaultPosition.text = languagesUsed.takeOffSomersaultPosition;
		textTakeOffTilt.text = languagesUsed.takeOffTilt;
		textTakeOffHorizontalSpeed.text = languagesUsed.takeOffHorizontal;
		textTakeOffVerticalSpeed.text = languagesUsed.takeOffVertical;
		textTakeOffSomersaultSpeed.text = languagesUsed.takeOffSomersaultSpeed;
		textTakeOffTwistSpeed.text = languagesUsed.takeOffTwist;

		// Section Résultats

		textMessagesTextTitle.text = languagesUsed.displayMsgTitle;

		// Section Animation

		Debug.Log(AnimationF.Instance.lineStickFigure);
		if (AnimationF.Instance.lineStickFigure != null)
		{
			AnimationF.Instance.textCurveName1.text = languagesUsed.leftSide;
			AnimationF.Instance.textCurveName2.text = languagesUsed.rightSide;
		}
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
	/// <param name="statusLoad">État des boutons Charger et Changer la langue, modifié ou non. </param>

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
			buttonToolTips.interactable = status;
			buttonToolTipsImage.color = color;
			buttonNoToolTips.GetComponent<Button>().interactable = status;
			buttonNoToolTipsImage.color = color;
			buttonLanguage.interactable = status;
			MovementF.Instance.buttonLoad.interactable = status;
			MovementF.Instance.buttonLoadImage.color = color;
		}
		MovementF.Instance.dropDownInterpolation.interactable = status;
		MovementF.Instance.buttonSave.interactable = status;
		MovementF.Instance.buttonSaveImage.color = color;
		MovementF.Instance.EnableDisableSymetricLeftRight(status, false);
		MovementF.Instance.buttonGraphSettings.interactable = status;
		MovementF.Instance.buttonGraphSettingsImage.color = color;

		MovementF.Instance.dropDownCondition.interactable = status;
		MovementF.Instance.dropDownInitialPosture.interactable = false;					// Non fonctionnelle encore
		MovementF.Instance.inputFieldSomersaultPosition.interactable = status;
		MovementF.Instance.inputFieldTilt.interactable = status;
		MovementF.Instance.inputFieldHorizontalSpeed.interactable = status;
		MovementF.Instance.inputFieldVerticalSpeed.interactable = status;
		MovementF.Instance.inputFieldSomersaultSpeed.interactable = status;
		MovementF.Instance.inputFieldTwistSpeed.interactable = status;

		AnimationF.Instance.textChrono.text = "";
		AnimationF.Instance.dropDownPlayMode.interactable = status;
		AnimationF.Instance.dropDownPlayView.interactable = status;
		AnimationF.Instance.dropDownPlaySpeed.interactable = status;
		AnimationF.Instance.buttonPlay.interactable = status;
		AnimationF.Instance.buttonPlayImage.color = color;
		if (status && MainParameters.Instance.joints.rot != null)
		{
			AnimationF.Instance.buttonGraph.interactable = true;
			AnimationF.Instance.buttonGraphImage.color = Color.white;
		}
		else
		{
			AnimationF.Instance.buttonGraph.interactable = false;
			AnimationF.Instance.buttonGraphImage.color = Color.gray;
		}
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
