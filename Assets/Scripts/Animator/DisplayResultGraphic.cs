using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;

// =================================================================================================================================================================
/// <summary> Permet d'afficher un graphique des résultats. </summary>

public class DisplayResultGraphic : MonoBehaviour
{
	public Dropdown dropDownGraphicName;
	public GraphChart graph;
	public int panelGraphicNumber;
	public Text textLabelAxisX;
	public Text textLabelAxisY;
	
	public GameObject panelLegend;
	public Text textLegendCurveName1;
	public Text textLegendCurveName2;
	public Text textLegendCurveName3;

	bool calledFromScript;      // Mode de modification d'un contrôle de la scène, false = via l'utilisateur (OnValueChange) ou true = via un script

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		calledFromScript = false;
	}

	// =================================================================================================================================================================
	/// <summary> Fonction exécuté quand le script est activé. </summary>

	void OnEnable()
	{
		calledFromScript = true;
		List<string> dropDownOptions = new List<string>();
		dropDownOptions.Add(MainParameters.Instance.languages.Used.resultsGraphicsSelectionRotationsVsTime);
		dropDownOptions.Add(MainParameters.Instance.languages.Used.resultsGraphicsSelectionTiltVsTime);
		dropDownOptions.Add(MainParameters.Instance.languages.Used.resultsGraphicsSelectionTiltVsSomersault);
		dropDownOptions.Add(MainParameters.Instance.languages.Used.resultsGraphicsSelectionTiltVsTwist);
		dropDownOptions.Add(MainParameters.Instance.languages.Used.resultsGraphicsSelectionTwistVsSomersault);
		dropDownOptions.Add(MainParameters.Instance.languages.Used.resultsGraphicsSelectionAngularSpeedVsTime);
		dropDownGraphicName.ClearOptions();
		dropDownGraphicName.AddOptions(dropDownOptions);
		dropDownGraphicName.value = MainParameters.Instance.resultsGraphicsUsed[panelGraphicNumber - 1];

		calledFromScript = false;
		DropDownGraphicNameOnValueChanged(MainParameters.Instance.resultsGraphicsUsed[panelGraphicNumber - 1]);
	}

	// =================================================================================================================================================================
	/// <summary> Liste déroulante Nom de l'articulation a été modifié. </summary>

	public void DropDownGraphicNameOnValueChanged(int value)
	{
		if (calledFromScript) return;

		MainParameters.Instance.resultsGraphicsUsed[panelGraphicNumber - 1] = value;
		switch (value)
		{
			case 0:                                                                                                                                             // Rotations vs temps
				GraphManager.Instance.DisplayCurves(graph, MainParameters.Instance.joints.t, MainParameters.Instance.joints.rot);
				textLabelAxisX.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTime;
				textLabelAxisY.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisRotation;
				panelLegend.SetActive(true);
				textLegendCurveName1.text = MainParameters.Instance.languages.Used.resultsGraphicsLegendCurveNameSomersault;
				textLegendCurveName2.text = MainParameters.Instance.languages.Used.resultsGraphicsLegendCurveNameTilt;
				textLegendCurveName3.text = MainParameters.Instance.languages.Used.resultsGraphicsLegendCurveNameTwist;
				break;
			case 1:                                                                                                                                             // Inclinaison vs temps
				GraphManager.Instance.DisplayCurves(graph, MainParameters.Instance.joints.t, MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
				textLabelAxisX.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTime;
				textLabelAxisY.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTilt;
				panelLegend.SetActive(false);
				break;
			case 2:                                                                                                                                             // Inclinaison vs salto
				GraphManager.Instance.DisplayCurves(graph, MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 0), MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
				textLabelAxisX.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisSomersault;
				textLabelAxisY.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTilt;
				panelLegend.SetActive(false);
				break;
			case 3:                                                                                                                                             // Inclinaison vs vrille
				GraphManager.Instance.DisplayCurves(graph, MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 2), MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 1));
				textLabelAxisX.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTwist;
				textLabelAxisY.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTilt;
				panelLegend.SetActive(false);
				break;
			case 4:                                                                                                                                             // Vrille vs salto
				GraphManager.Instance.DisplayCurves(graph, MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 0), MathFunc.MatrixGetColumn(MainParameters.Instance.joints.rot, 2));
				textLabelAxisX.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisSomersault;
				textLabelAxisY.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTwist;
				panelLegend.SetActive(false);
				break;
			case 5:                                                                                                                                             // Vitesse angulaire vs temps
				GraphManager.Instance.DisplayCurves(graph, MainParameters.Instance.joints.t, MainParameters.Instance.joints.rotdot);
				textLabelAxisX.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisTime;
				textLabelAxisY.text = MainParameters.Instance.languages.Used.resultsGraphicsLabelAxisAngularSpeed;
				panelLegend.SetActive(true);
				textLegendCurveName1.text = MainParameters.Instance.languages.Used.resultsGraphicsLegendCurveNameSomersault;
				textLegendCurveName2.text = MainParameters.Instance.languages.Used.resultsGraphicsLegendCurveNameTilt;
				textLegendCurveName3.text = MainParameters.Instance.languages.Used.resultsGraphicsLegendCurveNameTwist;
				break;
		}
	}
}
