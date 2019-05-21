using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées par les contrôles de la section Paramètres du graphique. </summary>

public class GraphSettings : MonoBehaviour
{
	public static GraphSettings Instance;

	public GameObject panelGraphSettings;
	public GameObject panelGraphSettingsErrorMsg;

	public Text textVerticalAxisTitle;
	public Text textVerticalAxisLowerBound;
	public Text textVerticalAxisUpperBound;
	public GameObject verticalAxisSlider;
	public Transform transformHandleVerticalAxisLowerBound;
	public Transform transformHandleVerticalAxisUpperBound;
	public InputField inputFieldVerticalAxiesLowerBound;
	public InputField inputFieldVerticalAxiesUpperBound;

	public Text textHorizontalAxisTitle;
	public Text textHorizontalAxisLowerBound;
	public Text textHorizontalAxisUpperBound;
	public GameObject horizontalAxisSlider;
	public InputField inputFieldHorizontalAxiesUpperBound;

	bool sliderMode;											// Mode de modification des barres de défilement, false = via un script, true = via un utilisateur (OnValueChange)
	Vector3 localPosSliderMin, localPosSliderMax;
	int verticalAxisLowerBound;									// Borne inférieur de l'axe vertical
	int verticalAxisLowerBoundPrev;
	int verticalAxisUpperBound;									// Borne supérieur de l'axe vertical
	int verticalAxisUpperBoundPrev;
	int horizontalAxisUpperBound;                               // Borne supérieur de l'axe horizontal
	int horizontalAxisUpperBoundPrev;
	int errorCode = 0;

	int minValueHipFlexion = -60;
	int maxValueHipFlexion = 130;
	int minValueKneeFlexion = -30;
	int maxValueKneeFlexion = 180;
	int minValueArmFlexion = -80;
	int maxValueArmFlexion = 180;
	int minValueArmAbduction = -80;
	int maxValueArmAbduction = 180;
	int minValueDefault = -300;
	int maxValueDefault = 300;

	void Start()
	{
		Instance = this;
	}

	// =================================================================================================================================================================
	// Initialisation de la barre de défilement double et de ses paramètres, utilisée pour l'axe vertical

	public void Init()
	{
		// Indiquez que les paramètres de la barre de défilement pour l'axe vertical est modifié via un script

		sliderMode = false;

		// Initialisation des valeurs minimum et maximum de la barre de défilement pour l'axe vertical, selon l'articulation sélectionnée

		errorCode = 0;
		int minValue, maxValue;
		string ddlName = MovementF.Instance.dropDownDDLNames.captionText.text;
		if (ddlName.Contains(MainParameters.Instance.languages.Used.movementDDLHipFlexion))
		{
			minValue = minValueHipFlexion;
			maxValue = maxValueHipFlexion;
		}
		else if (ddlName.Contains(MainParameters.Instance.languages.Used.movementDDLKneeFlexion))
		{
			minValue = minValueKneeFlexion;
			maxValue = maxValueKneeFlexion;
		}
		else if (ddlName.Contains(MainParameters.Instance.languages.Used.movementDDLLeftArmFlexion) || ddlName.Contains(MainParameters.Instance.languages.Used.movementDDLRightArmFlexion))
		{
			minValue = minValueArmFlexion;
			maxValue = maxValueArmFlexion;
		}
		else if (ddlName.Contains(MainParameters.Instance.languages.Used.movementDDLLeftArmAbduction) || ddlName.Contains(MainParameters.Instance.languages.Used.movementDDLRightArmAbduction))
		{
			minValue = minValueArmAbduction;
			maxValue = maxValueArmAbduction;
		}
		else
		{
			minValue = minValueDefault;
			maxValue = maxValueDefault;
		}
		verticalAxisSlider.GetComponent<Slider>().minValue = minValue;
		verticalAxisSlider.GetComponent<Slider>().maxValue = maxValue;

		// Trouver la longueur de la barre de défilement de l'axe vertical en pixels, utilisé pour positionner le bouton de la borne supérieur de façon manuel

		verticalAxisSlider.GetComponent<Slider>().value = minValue;
		localPosSliderMin = transformHandleVerticalAxisLowerBound.GetComponentInChildren<RectTransform>().localPosition;    // Position de la valeur minimum
		verticalAxisSlider.GetComponent<Slider>().value = maxValue;
		localPosSliderMax = transformHandleVerticalAxisLowerBound.GetComponentInChildren<RectTransform>().localPosition;    // Position de la valeur maximum

		// Placer les boutons, aux bornes inférieur et supérieur aux valeurs actuellement utilisées, de la barre de défilement et des boîtes d'entrée de texte de l'axe vertical

		verticalAxisLowerBound = (int)GraphManager.Instance.axisYmin;
		verticalAxisLowerBoundPrev = verticalAxisLowerBound;
		verticalAxisUpperBound = (int)GraphManager.Instance.axisYmax;
		verticalAxisUpperBoundPrev = verticalAxisUpperBound;

		if (verticalAxisUpperBound <= maxValue)
			setHandleVerticalAxisUpperBound(verticalAxisUpperBound);
		else
			setHandleVerticalAxisUpperBound(maxValue);
		verticalAxisSlider.GetComponent<Slider>().value = verticalAxisLowerBound;
		inputFieldVerticalAxiesLowerBound.text = string.Format("{0}°", verticalAxisLowerBound);
		inputFieldVerticalAxiesUpperBound.text = string.Format("{0}°", verticalAxisUpperBound);

		// Placer le bouton de la borne supérieur à la valeur actuellement utilisée (borne inférieur toujours à zéro), de la barre de défilement et des boîtes d'entrée de texte de l'axe horizontal

		horizontalAxisUpperBound = (int)GraphManager.Instance.axisXmax;
		horizontalAxisUpperBoundPrev = horizontalAxisUpperBound;

		horizontalAxisSlider.GetComponent<Slider>().minValue = 1;
		horizontalAxisSlider.GetComponent<Slider>().maxValue = horizontalAxisUpperBound * 3;
		horizontalAxisSlider.GetComponent<Slider>().value = horizontalAxisUpperBound;
		inputFieldHorizontalAxiesUpperBound.text = string.Format("{0} s", horizontalAxisUpperBound);

		sliderMode = true;
	}

	// =================================================================================================================================================================
	/// <summary> La barre déroulante de l'axe vertical a été déplacer. </summary>

	public void SetVerticalAxisSliderOnValueChanged(float sliderPos)
	{
		if (!sliderMode) return;

		// Vérifier lequel des 2 boutons est le plus près de la nouvelle position de la barre de défilement

		sliderMode = false;
		float diffLowerBound;
		float diffUpperBound;
		if (verticalAxisLowerBound >= verticalAxisSlider.GetComponent<Slider>().minValue)
			diffLowerBound = Mathf.Abs(sliderPos - verticalAxisLowerBound);
		else
			diffLowerBound = Mathf.Abs(sliderPos - verticalAxisSlider.GetComponent<Slider>().minValue);
		if (verticalAxisUpperBound <= verticalAxisSlider.GetComponent<Slider>().maxValue)
			diffUpperBound = Mathf.Abs(sliderPos - verticalAxisUpperBound);
		else
			diffUpperBound = Mathf.Abs(sliderPos - verticalAxisSlider.GetComponent<Slider>().maxValue);
		if (diffLowerBound < diffUpperBound || (diffLowerBound == diffUpperBound && sliderPos <= verticalAxisLowerBound))   // Borne inférieur est plus près
		{
			if (sliderPos <= verticalAxisUpperBound)
			{
				verticalAxisLowerBound = (int)sliderPos;
				inputFieldVerticalAxiesLowerBound.text = string.Format("{0}°", verticalAxisLowerBound);
				verticalAxisLowerBoundPrev = verticalAxisLowerBound;
			}
		}
		else                                                                                                                // Borne supérieur est plus près
		{
			if (sliderPos >= verticalAxisLowerBound)
			{
				verticalAxisUpperBound = (int)sliderPos;
				setHandleVerticalAxisUpperBound(verticalAxisUpperBound);
				inputFieldVerticalAxiesUpperBound.text = string.Format("{0}°", verticalAxisUpperBound);
				verticalAxisSlider.GetComponent<Slider>().value = verticalAxisLowerBound;       // Valeur de la borne inférieur
				verticalAxisUpperBoundPrev = verticalAxisUpperBound;
			}
		}
		sliderMode = true;
	}

	// =================================================================================================================================================================
	// La borne inférieur de l'axe vertical est spécifié en modifiant la valeur de la boîte d'entrée ("Input Field")

	public void SetVerticalAxisLowerBoundOnValueChanged()
	{
		if (!sliderMode) return;

		sliderMode = false;
		verticalAxisLowerBound = ReadAxisBoundValue("°", true, inputFieldVerticalAxiesLowerBound);
		if (errorCode >= 0 && verticalAxisLowerBound < verticalAxisUpperBound)
			verticalAxisSlider.GetComponent<Slider>().value = verticalAxisLowerBound;
		sliderMode = true;
	}

	// =================================================================================================================================================================
	// La borne inférieur de l'axe vertical est spécifié par la fin de l'édition de la boîte d'entrée ("Input Field")

	public void SetVerticalAxisLowerBoundOnEndEdit()
	{
		if (!sliderMode) return;

		sliderMode = false;
		if (errorCode <= -2)
		{
			verticalAxisLowerBound = verticalAxisLowerBoundPrev;
			verticalAxisSlider.GetComponent<Slider>().value = verticalAxisLowerBound;
			panelGraphSettingsErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgLowerBoundOverflow;
			panelGraphSettingsErrorMsg.SetActive(true);
		}
		else if (errorCode == -1)
		{
			verticalAxisLowerBound = verticalAxisLowerBoundPrev;
			verticalAxisSlider.GetComponent<Slider>().value = verticalAxisLowerBound;
			panelGraphSettingsErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgLowerBoundInvalid;
			panelGraphSettingsErrorMsg.SetActive(true);
		}
		inputFieldVerticalAxiesLowerBound.text = string.Format("{0}°", verticalAxisLowerBound);
		verticalAxisLowerBoundPrev = verticalAxisLowerBound;
		sliderMode = true;
	}

	// =================================================================================================================================================================
	// La borne supérieur de l'axe vertical est spécifié en modifiant la valeur de la boîte d'entrée ("Input Field")

	public void SetVerticalAxisUpperBoundOnValueChanged()
	{
		if (!sliderMode) return;

		sliderMode = false;
		verticalAxisUpperBound = ReadAxisBoundValue("°", false, inputFieldVerticalAxiesUpperBound);
		if (errorCode >= 0 && verticalAxisUpperBound > verticalAxisSlider.GetComponent<Slider>().maxValue)
			setHandleVerticalAxisUpperBound(verticalAxisSlider.GetComponent<Slider>().maxValue);
		else if (errorCode >= 0 && verticalAxisUpperBound > verticalAxisLowerBound)
			setHandleVerticalAxisUpperBound(verticalAxisUpperBound);
		sliderMode = true;
	}

	// =================================================================================================================================================================
	// La borne supérieur de l'axe vertical est spécifié par la fin de l'édition de la boîte d'entrée ("Input Field")

	public void SetVerticalAxisUpperBoundOnEndEdit()
	{
		if (!sliderMode) return;

		sliderMode = false;
		if (errorCode <= -2)
		{
			verticalAxisUpperBound = verticalAxisUpperBoundPrev;
			if (verticalAxisUpperBound > verticalAxisSlider.GetComponent<Slider>().maxValue)
				setHandleVerticalAxisUpperBound(verticalAxisSlider.GetComponent<Slider>().maxValue);
			else
				setHandleVerticalAxisUpperBound(verticalAxisUpperBound);
			panelGraphSettingsErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgUpperBoundOverflow;
			panelGraphSettingsErrorMsg.SetActive(true);
		}
		else if (errorCode == -1)
		{
			verticalAxisUpperBound = verticalAxisUpperBoundPrev;
			if (verticalAxisUpperBound > verticalAxisSlider.GetComponent<Slider>().maxValue)
				setHandleVerticalAxisUpperBound(verticalAxisSlider.GetComponent<Slider>().maxValue);
			else
				setHandleVerticalAxisUpperBound(verticalAxisUpperBound);
			panelGraphSettingsErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgUpperBoundInvalid;
			panelGraphSettingsErrorMsg.SetActive(true);
		}
		inputFieldVerticalAxiesUpperBound.text = string.Format("{0}°", verticalAxisUpperBound);
		verticalAxisUpperBoundPrev = verticalAxisUpperBound;
		sliderMode = true;
	}

	// =================================================================================================================================================================
	/// <summary> La barre déroulante de l'axe horizontal a été déplacer. </summary>

	public void SetHorizontalAxisSliderOnValueChanged(float sliderPos)
	{
		if (!sliderMode) return;
		sliderMode = false;
		horizontalAxisUpperBound = (int)sliderPos;
		inputFieldHorizontalAxiesUpperBound.text = string.Format("{0} s", horizontalAxisUpperBound);
		horizontalAxisUpperBoundPrev = horizontalAxisUpperBound;
		sliderMode = true;
	}

	// =================================================================================================================================================================
	// La borne supérieur de l'axe horizontal est spécifié en modifiant la valeur de la boîte d'entrée ("Input Field")

	public void SetHorizontalAxisUpperBoundOnValueChanged()
	{
		if (!sliderMode) return;

		sliderMode = false;
		horizontalAxisUpperBound = ReadAxisBoundValue("s", false, inputFieldHorizontalAxiesUpperBound);
		if (errorCode >= 0)
			horizontalAxisSlider.GetComponent<Slider>().value = horizontalAxisUpperBound;
		sliderMode = true;
	}

	// =================================================================================================================================================================
	// La borne supérieur de l'axe horizontal est spécifié par la fin de l'édition de la boîte d'entrée ("Input Field")

	public void SetHorizontalAxisUpperBoundOnEndEdit()
	{
		if (!sliderMode) return;

		sliderMode = false;
		if (errorCode <= -2)
		{
			horizontalAxisUpperBound = horizontalAxisUpperBoundPrev;
			horizontalAxisSlider.GetComponent<Slider>().value = horizontalAxisUpperBound;
			panelGraphSettingsErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgUpperBoundOverflow;
			panelGraphSettingsErrorMsg.SetActive(true);
		}
		else if (errorCode == -1)
		{
			horizontalAxisUpperBound = horizontalAxisUpperBoundPrev;
			horizontalAxisSlider.GetComponent<Slider>().value = horizontalAxisUpperBound;
			panelGraphSettingsErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgUpperBoundInvalid;
			panelGraphSettingsErrorMsg.SetActive(true);
		}
		inputFieldHorizontalAxiesUpperBound.text = string.Format("{0} s", horizontalAxisUpperBound);
		horizontalAxisUpperBoundPrev = horizontalAxisUpperBound;
		sliderMode = true;
	}

	// =================================================================================================================================================================
	/// <summary> Bouton OK a été appuyer. </summary>

	public void ButtonOK()
	{
		// Afficher le graphique de nouveau, avec les nouvelles échelles

		GraphManager.Instance.axisXmax = horizontalAxisUpperBound;
		GraphManager.Instance.axisYmin = verticalAxisLowerBound;
		GraphManager.Instance.axisYmax = verticalAxisUpperBound;
		MovementF.Instance.DisplayDDL(false, GraphManager.Instance.ddlUsed, false);

		// Quitter le panneau Paramètres du graphique

		panelGraphSettings.SetActive(false);
		GraphManager.Instance.mouseTracking = true;
		GraphManager.Instance.mouseDisableLastButton = true;
	}

	// =================================================================================================================================================================
	// Placer le bouton (handle) de la borne supérieur, sur la barre de défilement double axe vertical

	void setHandleVerticalAxisUpperBound(float value)
	{
		Vector3 localPosVerticalAxisMax = transformHandleVerticalAxisUpperBound.GetComponentInChildren<RectTransform>().localPosition;
		localPosVerticalAxisMax.x = (value - verticalAxisSlider.GetComponent<Slider>().minValue) * (localPosSliderMax.x - localPosSliderMin.x) /
								(verticalAxisSlider.GetComponent<Slider>().maxValue - verticalAxisSlider.GetComponent<Slider>().minValue) + localPosSliderMin.x;
		transformHandleVerticalAxisUpperBound.GetComponentInChildren<RectTransform>().localPosition = localPosVerticalAxisMax;
	}

	// =================================================================================================================================================================
	// Lecture de la valeur modifié dans une boîte d'entrée ("Input field")

	int ReadAxisBoundValue(string symbol, bool lowerBound, InputField inputField)
	{
		errorCode = 0;
		int value;
		bool verticalAxis = symbol.Contains("°");
		string stringValue = inputField.text;
		int symbolPos = stringValue.IndexOf(symbol);
		if (symbolPos >= 0)
			stringValue = stringValue.Substring(0, symbolPos);
		try
		{
			value = int.Parse(stringValue);
		}
		catch
		{
			value = 0;
			errorCode = -1;
			return value;
		}
		if ((verticalAxis && ((lowerBound && value >= verticalAxisUpperBound) || (!lowerBound && value <= verticalAxisLowerBound))) || (!verticalAxis && value <= 0))
		{
			value = 0;
			errorCode = -2;
		}
		return value;
	}
}
