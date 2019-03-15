using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour accéder et vérifier les paramètres de décollage. </summary>

public class TakeOffParamF : MonoBehaviour
{
	public GameObject panelMessage;

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
		if (panel.name == "PanelInitialRotation")
		{
			panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
			MainParameters.Instance.joints.takeOffParam.rotation = value;
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
				panelMessage.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgVerticalSpeed;
				panelMessage.SetActive(true);
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
	}
}
