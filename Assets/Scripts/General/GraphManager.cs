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

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
		graph = GetComponent<GraphChart>();
	}

	// =================================================================================================================================================================
	/// <summary> Afficher les angles interpolés et les noeuds de l'articulation spécifié. </summary>

	public void DisplayCurveAndNodes(int ddl, float[] t0, float[,] q0, MainParameters.StrucNodes[] nodes)
	{
		if (graph == null) return;
		graph.DataSource.StartBatch();
		graph.DataSource.ClearCategory("Data");
		graph.DataSource.ClearCategory("Nodes");

		float q0Min = 360;
		float q0Max = -360;
		float q0Value;
		for (int i = 0; i < t0.Length; i++)
		{
			q0Value = q0[ddl, i] * 180 / Mathf.PI;
			graph.DataSource.AddPointToCategory("Data", t0[i], q0Value);
			if (q0Value < q0Min) q0Min = q0Value;
			if (q0Value > q0Max) q0Max = q0Value;
		}

		for (int i = 0; i < nodes[ddl].T.Length; i++)
			graph.DataSource.AddPointToCategory("Nodes", nodes[ddl].T[i], nodes[ddl].Q[i] * 180 / Mathf.PI);

		float timeBorder = (t0[t0.Length - 1] - t0[0]) * 0.01f;
		graph.DataSource.HorizontalViewOrigin = t0[0] - timeBorder;
		graph.DataSource.HorizontalViewSize = t0[t0.Length -1] + timeBorder - graph.DataSource.HorizontalViewOrigin;
		graph.DataSource.VerticalViewOrigin = Mathf.Round(q0Min - 30);
		graph.DataSource.VerticalViewSize = Mathf.Round(q0Max + 30) - graph.DataSource.VerticalViewOrigin;
		graph.DataSource.EndBatch();
	}
}
