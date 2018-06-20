using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartAndGraph;

// =================================================================================================================================================================
/// <summary> Accès au graphique de la partie Movement (affichage des angles des articulations). </summary>

public class GraphManager : MonoBehaviour
{
	public static GraphManager Instance;
	GraphChart graph;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;

		//graph = GetComponent<GraphChart>();
		//if (graph != null)
		//{
		//	graph.DataSource.StartBatch();
		//	graph.DataSource.ClearCategory("Data");
		//	for (int i = 0; i < 30; i++)
		//	{
		//		graph.DataSource.AddPointToCategory("Data", Random.value * 10f, Random.value * 10f);
		//	}
		//	graph.DataSource.EndBatch();
		//}
	}
}
