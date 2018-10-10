using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;

// =================================================================================================================================================================
/// <summary> Accès au graphique du panneau PanelMovement, qui est utilisé pour afficher les angles interpolés et les noeuds d'une articulation. </summary>

public class GraphManager : MonoBehaviour
{
	public static GraphManager Instance;
	GraphChart graph;
	float q0MinCurve0;
	float q0MaxCurve0;
	string[] dataCategories;
	string[] nodesCategories;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
		graph = GetComponent<GraphChart>();
		dataCategories = new string[2] { "Data1", "Data2" };
		nodesCategories = new string[2] { "Nodes1", "Nodes2" };
	}

	// =================================================================================================================================================================
	/// <summary> Afficher les angles interpolés et les noeuds de l'articulation spécifié. </summary>

	public void DisplayCurveAndNodes(int curve, int ddl, float[] t0, float[,] q0, MainParameters.StrucNodes[] nodes)
	{
		if (graph == null) return;
		graph.DataSource.StartBatch();

		// Effacer la ou les courbes précédentes, Data et Nodes

		graph.DataSource.ClearCategory(dataCategories[curve]);
		graph.DataSource.ClearCategory(nodesCategories[curve]);
		if (curve <= 0)
		{
			graph.DataSource.ClearCategory(dataCategories[1]);
			graph.DataSource.ClearCategory(nodesCategories[1]);
		}

		// Ajouter toutes les données dans la nouvelle courbe Data
		// Calculer les valeurs minimum et maximum

		float q0Min = 360;
		float q0Max = -360;
		float q0Value;
		for (int i = 0; i < t0.Length; i++)
		{
			q0Value = q0[ddl, i] * 180 / Mathf.PI;
			graph.DataSource.AddPointToCategory(dataCategories[curve], t0[i], q0Value);
			if (q0Value < q0Min) q0Min = q0Value;
			if (q0Value > q0Max) q0Max = q0Value;
		}

		// Ajouter les noeuds dans la nouvelle courbe Nodes

		for (int i = 0; i < nodes[ddl].T.Length; i++)
			graph.DataSource.AddPointToCategory(nodesCategories[curve], nodes[ddl].T[i], nodes[ddl].Q[i] * 180 / Mathf.PI);

		// Définir les échelles des temps et des angles

		float timeBorder = (t0[t0.Length - 1] - t0[0]) * 0.01f;
		graph.DataSource.HorizontalViewOrigin = t0[0] - timeBorder;
		graph.DataSource.HorizontalViewSize = t0[t0.Length -1] + timeBorder - graph.DataSource.HorizontalViewOrigin;
		if (curve <= 0)
		{
			q0MinCurve0 = q0Min;
			q0MaxCurve0 = q0Max;
		}
		else
		{
			q0Min = Mathf.Min(q0Min, q0MinCurve0);
			q0Max = Mathf.Max(q0Max, q0MaxCurve0);
		}
		graph.DataSource.VerticalViewOrigin = Mathf.Round(q0Min - 30);
		graph.DataSource.VerticalViewSize = Mathf.Round(q0Max + 30) - graph.DataSource.VerticalViewOrigin;
		graph.DataSource.EndBatch();
	}
}
