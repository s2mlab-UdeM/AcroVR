using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QCurvesFunc : MonoBehaviour
{

	void Start ()
	{
	}

	public void DropDownDDLNamesOnValueChanged(int value)
	{
		// Afficher la courbe des positions des angles pour l'articulation sélectionné par défaut

		GraphManager.Instance.DisplayCurveAndNodes(0, value, MainParameters.Instance.joints.t0, MainParameters.Instance.joints.q0, MainParameters.Instance.joints.nodes);
		if (MainParameters.Instance.joints.nodes[value].ddlOppositeSide >= 0)
			GraphManager.Instance.DisplayCurveAndNodes(1, MainParameters.Instance.joints.nodes[value].ddlOppositeSide, MainParameters.Instance.joints.t0, MainParameters.Instance.joints.q0,
				MainParameters.Instance.joints.nodes);
	}
}
