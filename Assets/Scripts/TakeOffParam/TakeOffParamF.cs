using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour accéder et vérifier les paramètres de décollage. </summary>

public class TakeOffParamF : MonoBehaviour
{
	public GameObject panelTakeOffErrorMsg;

	// =================================================================================================================================================================
	/// <summary> Vérification d'un des paramètres de décollage suivants: Rotation initiale, Tilt, Vitesse horizontale, Saut périlleux et Rotation. </summary>

	public void CheckTakeOffParamCondition(Dropdown dropDown)
	{
		MainParameters.Instance.joints.condition = dropDown.value;
	}

	// =================================================================================================================================================================
	/// <summary> Vérification d'un des paramètres de décollage suivants: Rotation initiale, Tilt, Vitesse horizontale, Saut périlleux et Rotation. </summary>

	public void CheckTakeOffParam(GameObject panel)
	{
		float value = float.Parse(panel.GetComponentInChildren<InputField>().text);
		if (panel.name == "PanelSomersaultPosition")
		{
			if (value < -180 || value > 180)
			{
				panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", MainParameters.Instance.joints.takeOffParam.rotation);
				Main.Instance.EnableDisableControls(false, true);
				panelTakeOffErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgSomersaultPosition;
				panelTakeOffErrorMsg.SetActive(true);
			}
			else
			{
				panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
				MainParameters.Instance.joints.takeOffParam.rotation = value;
			}
		}
		else if (panel.name == "PanelTilt")
		{
			panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
			MainParameters.Instance.joints.takeOffParam.tilt = value;
		}
		else if (panel.name == "PanelHorizontalSpeed")
		{
			panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
			MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed = value;
		}
		else if (panel.name == "PanelVerticalSpeed")
		{
			if (value < 0)
			{
				panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", MainParameters.Instance.joints.takeOffParam.verticalSpeed);
				Main.Instance.EnableDisableControls(false, true);
				panelTakeOffErrorMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgVerticalSpeed;
				panelTakeOffErrorMsg.SetActive(true);
			}
			else
			{
				panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
				MainParameters.Instance.joints.takeOffParam.verticalSpeed = value;
			}
		}
		else if (panel.name == "PanelSomersaultSpeed")
		{
			panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.000}", value);
			MainParameters.Instance.joints.takeOffParam.somersaultSpeed = value;
		}
		else if (panel.name == "PanelTwistSpeed")
		{
			panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.000}", value);
			MainParameters.Instance.joints.takeOffParam.twistSpeed = value;
		}
		else
			Debug.Log("ERREUR - Nom de panneau inconnu");
	}

	// =================================================================================================================================================================
	/// <summary> Fonction exécuté quand le bouton OK du panneau PanelTakeOffErrorMsg est appuyé. </summary>

	public void ErrorMsgButtonOK()
	{
		Main.Instance.EnableDisableControls(true, true);
		panelTakeOffErrorMsg.SetActive(false);
	}
}
