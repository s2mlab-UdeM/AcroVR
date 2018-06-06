using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;

public class AcroVRGraphChart : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		GraphChart graph = GetComponent<GraphChart>();
		if (graph != null)
		{
			graph.DataSource.StartBatch();
			graph.DataSource.ClearCategory("Data");
			for (int i = 0; i < 30; i++)
			{
				graph.DataSource.AddPointToCategory("Data", Random.value * 10f, Random.value * 10f);
			}
			graph.DataSource.EndBatch();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
