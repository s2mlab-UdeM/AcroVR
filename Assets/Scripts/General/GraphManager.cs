using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChartAndGraph;

// =================================================================================================================================================================
/// <summary> Accès au graphique du panneau PanelMovement, qui est utilisé pour afficher les angles interpolés et les noeuds d'une articulation. </summary>

public class GraphManager : MonoBehaviour
{
	public static GraphManager Instance;
	public Canvas canvas;
	public Material data1GraphLine;
	public Material nodes1GraphPoint;
	public Material dataLeftGraphLine;
	public Material nodesLeftGraphPoint;
	public Material dataRightGraphLine;
	public Material nodesRightGraphPoint;
	public GameObject panelAddRemoveNode;
	public GameObject buttonAddNode;
	public GameObject buttonRemoveNode;
	public GameObject buttonCancelChanges1;
	public GameObject panelCancelChanges;
	public GameObject buttonCancelChanges2;
	public GameObject panelMoveErrMsg;

	public int ddlUsed = 0;
	public float mousePosSaveX;
	public float mousePosSaveY;
	public bool mouseTracking = false;
	public bool mouseRightButtonON = false;
	public bool mouseDisableLastButton = false;

	public float axisXmin = 0;
	public float axisXmax = 0;
	public float axisYmin = 0;
	public float axisYmax = 0;
	public float axisXmaxDefault = 0;
	public float axisYminDefault = 0;
	public float axisYmaxDefault = 0;

	public GraphChart graph;
	float q0MinCurve0;
	float q0MaxCurve0;
	string[] dataCategories;
	string[] nodesCategories;
	string nodesTemp1Category;
	string nodesTemp2Category;
	string dataTempCategory;
	//string cursorCategorie;

	string[] dataCurvesCategories;

	int nodeUsed = 0;
	int numNodes = 0;
	float radToDeg = 180 / Mathf.PI;
	float factorGraphRatioX = 0;            // Facteurs relatifs qui tient compte du ratio X vs Y du graphique, dont les unités sont similaire dans les 2 coordonnées
	float factorGraphRatioY = 0;

	double mousePosX;
	double mousePosY;
	bool mouseLeftButtonON = false;
	//bool mouseMiddleButtonON = false;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
		graph = GetComponent<GraphChart>();
		dataCategories = new string[2] { "Data1", "Data2" };
		nodesCategories = new string[2] { "Nodes1", "Nodes2" };
		nodesTemp1Category = "NodesTemp1";
		nodesTemp2Category = "NodesTemp2";
		dataTempCategory = "DataTemp";
		//cursorCategorie = "Cursor";

		dataCurvesCategories = new string[3] { "Data1", "Data2", "Data3" };
	}

	// =================================================================================================================================================================
	/// <summary> Exécution de la fonction à chaque frame. </summary>

	void Update()
	{
		// Vérifier si la lecture des contrôles de la souris est active
		// Vérifier aussi si l'option de désactiver l'action du dernier bouton de la souris est active

		if (!mouseTracking) return;
		if (mouseDisableLastButton)
		{
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
				mouseDisableLastButton = false;
				mouseLeftButtonON = false;
				mouseRightButtonON = false;
			}
			return;
		}

		// Désactiver le menu (qui est affiché quand on appuye sur le bouton droit de la souris) si le bouton de gauche de la souris est appuyé à l'extérieur du menu

		if (Input.GetMouseButtonDown(0) && mouseRightButtonON && !MouseManager.Instance.IsOnGameObject(panelAddRemoveNode) && !MouseManager.Instance.IsOnGameObject(panelCancelChanges))
		{
			panelAddRemoveNode.SetActive(false);
			panelCancelChanges.SetActive(false);
			mouseRightButtonON = false;
			return;
		}

		// Lecture de la position de la souris, en unité du graphique
		// Si la souris n'est pas sur le graphique, alors on fait rien

		graph.MouseToClient(out mousePosX, out mousePosY);
		if (mousePosX < graph.DataSource.HorizontalViewOrigin || mousePosX > graph.DataSource.HorizontalViewOrigin + graph.DataSource.HorizontalViewSize ||
			mousePosY < graph.DataSource.VerticalViewOrigin || mousePosY > graph.DataSource.VerticalViewOrigin + graph.DataSource.VerticalViewSize)
		{
			if (mouseLeftButtonON)
				RemoveNodesTemp();
			return;
		}

		// Bouton gauche de la souris appuyé => déplacement d'un noeud (Bouton gauche appuyé, modifié l'affichage du noeud à déplacer)

		if (Input.GetMouseButtonDown(0) && !mouseRightButtonON)
		{
			DisplayNodesTemp2();
			mouseLeftButtonON = true;
		}

		// Bouton gauche de la souris appuyé => déplacement d'un noeud (Bouton gauche toujours appuyé, affiché le noeud temporaire)

		if (mouseLeftButtonON)
		{
			graph.DataSource.StartBatch();
			graph.DataSource.ClearCategory(nodesTemp1Category);
			if (MainParameters.Instance.joints.nodes[ddlUsed].interpolation.type == MainParameters.InterpolationType.Quintic)
				graph.DataSource.AddPointToCategory(nodesTemp1Category, mousePosX, mousePosY);
			else
				graph.DataSource.AddPointToCategory(nodesTemp1Category, MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed], mousePosY);
			graph.DataSource.EndBatch();
		}

		// Bouton gauche de la souris appuyé => déplacement d'un noeud (Bouton gauche relâché, effacé le noeud temporaire et modifier le noeud sélectionné, si la position est correct)

		if (Input.GetMouseButtonUp(0) && mouseLeftButtonON)
		{
			// Enlever les noeuds temporaires

			RemoveNodesTemp();

			// Vérifier si la nouvelle position du noeud est correct, si type d'interpolation est Quintic

			if (MainParameters.Instance.joints.nodes[ddlUsed].interpolation.type == MainParameters.InterpolationType.Quintic)
			{
				if ((nodeUsed <= 0 && mousePosX >= MainParameters.Instance.joints.nodes[ddlUsed].T[1]) || (nodeUsed >= numNodes - 1 && mousePosX <= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed - 1]) ||
				(nodeUsed > 0 && nodeUsed < numNodes - 1 && (mousePosX <= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed - 1] || mousePosX >= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed + 1])))
				{
					Main.Instance.EnableDisableControls(false, true);
					panelMoveErrMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgInvalidNodePosition;
					GraphManager.Instance.mouseTracking = false;
					panelMoveErrMsg.SetActive(true);
					return;
				}

				// Conserver le temps du nouveau noeud, si type d'interpolation est Quintic

				MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed] = (float)mousePosX;
			}

			// Conserver la position de l'angle du nouveau noeud

			MainParameters.Instance.joints.nodes[ddlUsed].Q[nodeUsed] = (float)mousePosY / radToDeg;

			// Interpolation et affichage des positions des angles pour l'articulation sélectionnée. Afficher aussi la silhouette au temps du noeud sélectionné

			MovementF.Instance.InterpolationAndDisplayDDL(ddlUsed, ddlUsed, (int)Mathf.Round(MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed] / MainParameters.Instance.joints.lagrangianModel.dt), false);
		}

		// Bouton droit de la souris appuyé

		if (Input.GetMouseButtonDown(1))
		{
			mouseRightButtonON = true;

			mousePosSaveX = (float)mousePosX;
			mousePosSaveY = (float)mousePosY;

			// Type d'interpolation est Quintic => ajouter/effacer un noeud et autres actions

			if (MainParameters.Instance.joints.nodes[ddlUsed].interpolation.type == MainParameters.InterpolationType.Quintic)
			{
				buttonAddNode.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.movementButtonAddNode;
				buttonRemoveNode.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.movementButtonRemoveNode;
				buttonCancelChanges1.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.movementButtonCancelChanges;
				DisplayContextMenu(panelAddRemoveNode);
			}

			// Type d'interpolation est Spline cubique => modifier la pente initiale ou finale (Trouver le noeud précédent la position de la souris) ou encore afficher le bouton CancelChanges

			else
			{
				nodeUsed = FindPreviousNode();
				if (nodeUsed >= numNodes - 2)
					nodeUsed = numNodes - 1;

				if (nodeUsed > 0 && nodeUsed < numNodes - 1)
				{
					buttonCancelChanges2.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.movementButtonCancelChanges;
					DisplayContextMenu(panelCancelChanges);
				}
			}
		}

		// Bouton droit de la souris a été appuyé et type d'interpolation est Spline cubique => modifier la pente initiale ou finale

		if (mouseRightButtonON && MainParameters.Instance.joints.nodes[ddlUsed].interpolation.type == MainParameters.InterpolationType.CubicSpline && (nodeUsed <= 0 || nodeUsed >= numNodes - 1))
		{
			// Bouton droit relâché, effacer la pente temporaire et modifier la pente sélectionnée

			if (Input.GetMouseButtonUp(1))
			{
				graph.DataSource.StartBatch();
				graph.DataSource.ClearCategory(dataTempCategory);
				graph.DataSource.EndBatch();
				float slope = ((float)mousePosY / radToDeg - MainParameters.Instance.joints.nodes[ddlUsed].Q[nodeUsed]) / ((float)mousePosX - MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed]);
				if (nodeUsed <= 0)
					MainParameters.Instance.joints.nodes[ddlUsed].interpolation.slope[0] = slope;
				else
					MainParameters.Instance.joints.nodes[ddlUsed].interpolation.slope[1] = slope;
				int frame = (int)Mathf.Round(MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed] / MainParameters.Instance.joints.lagrangianModel.dt);
				MovementF.Instance.InterpolationAndDisplayDDL(ddlUsed, ddlUsed, frame, false);
				mouseRightButtonON = false;
			}

			// Bouton droit toujours appuyé, afficher la pente temporaire

			else
			{
				panelCancelChanges.SetActive(false);
				graph.DataSource.StartBatch();
				graph.DataSource.ClearCategory(dataTempCategory);
				graph.DataSource.AddPointToCategory(dataTempCategory, MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed], MainParameters.Instance.joints.nodes[ddlUsed].Q[nodeUsed] * radToDeg);
				graph.DataSource.AddPointToCategory(dataTempCategory, mousePosX, mousePosY);
				graph.DataSource.EndBatch();
			}
		}

		//if (Input.GetMouseButtonDown(2))
		//	Debug.Log("Pressed middle click.");
	}

	// =================================================================================================================================================================
	/// <summary> Bouton OK du panneau qui affiche un message d'erreur a été appuyer. </summary>

	public void ButtonOK()
	{
		Main.Instance.EnableDisableControls(true, true);
		panelMoveErrMsg.SetActive(false);
		mouseTracking = true;
		mouseDisableLastButton = true;
	}

	// =================================================================================================================================================================
	/// <summary> Afficher les angles interpolés et les noeuds de l'articulation spécifié. </summary>

	public void DisplayCurveAndNodes(int curve, int ddl, bool axisRange)
	{
		if (graph == null) return;
		graph.DataSource.StartBatch();

		// Effacer la ou les courbes précédentes, Data et Nodes

		graph.DataSource.ClearCategory(dataCategories[curve]);
		graph.DataSource.ClearCategory(nodesCategories[curve]);
		if (curve <= 0)
		{
			ddlUsed = ddl;
			numNodes = MainParameters.Instance.joints.nodes[ddl].T.Length;
			graph.DataSource.ClearCategory(dataCategories[1]);
			graph.DataSource.ClearCategory(nodesCategories[1]);
		}

		// Ajouter toutes les données dans la nouvelle courbe Data
		// Calculer les valeurs minimum et maximum

		float q0Min = 360;
		float q0Max = -360;
		float value;
		int t0Length = MainParameters.Instance.joints.t0.Length;
		for (int i = 0; i < t0Length; i++)
		{
			value = MainParameters.Instance.joints.q0[ddl, i] * radToDeg;
			if (!axisRange && value < axisYmin)
				value = axisYmin;
			if (!axisRange && value > axisYmax)
				value = axisYmax;
			if (axisRange || MainParameters.Instance.joints.t0[i] <= axisXmax)
				graph.DataSource.AddPointToCategory(dataCategories[curve], MainParameters.Instance.joints.t0[i], value);
			if (value < q0Min) q0Min = value;
			if (value > q0Max) q0Max = value;
		}

		// Ajouter les noeuds dans la nouvelle courbe Nodes

		for (int i = 0; i < MainParameters.Instance.joints.nodes[ddl].T.Length; i++)
		{
			value = MainParameters.Instance.joints.nodes[ddl].Q[i] * radToDeg;
			if (axisRange || (MainParameters.Instance.joints.nodes[ddl].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax))
				graph.DataSource.AddPointToCategory(nodesCategories[curve], MainParameters.Instance.joints.nodes[ddl].T[i], value);
		}

		// Modifier les couleurs des courbes du graphique selon le type d'articulation affiché

		Material dataGraphLine;
		Material nodesGraphPoint;
		MaterialTiling tiling = new MaterialTiling(false, 45.5f);
		if (MainParameters.Instance.joints.nodes[ddl].name.ToLower().Contains(MainParameters.Instance.languages.french.leftSide.ToLower()) ||
			MainParameters.Instance.joints.nodes[ddl].name.ToLower().Contains(MainParameters.Instance.languages.english.leftSide.ToLower()))
		{
			dataGraphLine = dataLeftGraphLine;
			nodesGraphPoint = nodesLeftGraphPoint;
		}
		else if (MainParameters.Instance.joints.nodes[ddl].name.ToLower().Contains(MainParameters.Instance.languages.french.rightSide.ToLower()) ||
			MainParameters.Instance.joints.nodes[ddl].name.ToLower().Contains(MainParameters.Instance.languages.english.rightSide.ToLower()))
		{
			dataGraphLine = dataRightGraphLine;
			nodesGraphPoint = nodesRightGraphPoint;
		}
		else
		{
			dataGraphLine = data1GraphLine;
			nodesGraphPoint = nodes1GraphPoint;
		}
		graph.DataSource.SetCategoryLine(dataCategories[curve], dataGraphLine, 2.58f, tiling);
		graph.DataSource.SetCategoryPoint(nodesCategories[curve], nodesGraphPoint, 8);


		// Définir les échelles des temps et des angles

		if (axisRange)
		{
			axisXmin = Mathf.Round(MainParameters.Instance.joints.t0[0] - 0.5f);
			axisXmax = Mathf.Round(MainParameters.Instance.joints.t0[t0Length - 1] + 0.5f);
			axisXmaxDefault = axisXmax;
			graph.DataSource.HorizontalViewOrigin = axisXmin;
			graph.DataSource.HorizontalViewSize = axisXmax - axisXmin;
			factorGraphRatioX = graph.WidthRatio / (float)graph.DataSource.HorizontalViewSize;
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
			axisYmin = Mathf.Round((q0Min - 30) / 10) * 10;
			axisYmax = Mathf.Round((q0Max + 30) / 10) * 10;
			axisYminDefault = axisYmin;
			axisYmaxDefault = axisYmax;
			graph.DataSource.VerticalViewOrigin = axisYmin;
			graph.DataSource.VerticalViewSize = axisYmax - axisYmin;
			factorGraphRatioY = graph.HeightRatio / (float)graph.DataSource.VerticalViewSize;
		}
		else
		{
			graph.DataSource.HorizontalViewOrigin = axisXmin;
			graph.DataSource.HorizontalViewSize = axisXmax - axisXmin;
			factorGraphRatioX = graph.WidthRatio / (float)graph.DataSource.HorizontalViewSize;
			graph.DataSource.VerticalViewOrigin = axisYmin;
			graph.DataSource.VerticalViewSize = axisYmax - axisYmin;
			factorGraphRatioY = graph.HeightRatio / (float)graph.DataSource.VerticalViewSize;
		}
		graph.DataSource.EndBatch();
	}

	// =================================================================================================================================================================
	/// <summary> Afficher les noeuds de l'articulation spécifié, sauf le noeud modifié qui sera affiché d'une façon différente. </summary>

	public void DisplayNodesTemp2()
	{
		graph.DataSource.StartBatch();

		// Effacer les noeuds précédents

		graph.DataSource.ClearCategory(nodesCategories[0]);

		// Trouver le noeud le plus près de la position de la souris (en tenant compte du ratio X vs Y du graphique), ça sera ce noeud qui sera modifié

		mousePosSaveX = (float)mousePosX;
		mousePosSaveY = (float)mousePosY;
		nodeUsed = FindNearestNode();

		// Ajouter les noeuds dans la nouvelle courbe Nodes et NodesTemp2

		for (int i = 0; i < numNodes; i++)
		{
			float value = MainParameters.Instance.joints.nodes[ddlUsed].Q[i] * radToDeg;
			if (MainParameters.Instance.joints.nodes[ddlUsed].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax)
			{
				if (i != nodeUsed)
					graph.DataSource.AddPointToCategory(nodesCategories[0], MainParameters.Instance.joints.nodes[ddlUsed].T[i], value);
				else
					graph.DataSource.AddPointToCategory(nodesTemp2Category, MainParameters.Instance.joints.nodes[ddlUsed].T[i], value);
			}
		}

		graph.DataSource.EndBatch();
	}

	// =================================================================================================================================================================
	/// <summary> Effacer les noeuds temporaire et réafficher les noeuds de l'articulation spécifié. </summary>

	void RemoveNodesTemp()
	{
		graph.DataSource.StartBatch();
		graph.DataSource.ClearCategory(nodesTemp1Category);
		graph.DataSource.ClearCategory(nodesTemp2Category);
		graph.DataSource.ClearCategory(nodesCategories[0]);
		for (int i = 0; i < MainParameters.Instance.joints.nodes[ddlUsed].T.Length; i++)
		{
			float value = MainParameters.Instance.joints.nodes[ddlUsed].Q[i] * radToDeg;
			if (MainParameters.Instance.joints.nodes[ddlUsed].T[i] <= axisXmax && value >= axisYmin && value <= axisYmax)
				graph.DataSource.AddPointToCategory(nodesCategories[0], MainParameters.Instance.joints.nodes[ddlUsed].T[i], value);
		}
		graph.DataSource.EndBatch();
		mouseLeftButtonON = false;
	}

	// =================================================================================================================================================================
	/// <summary> Afficher le curseur qui indique le temps. </summary>

	//public void DisplayCursor(float t)			// Fonction a modifié (corrigé), si jamais elle vient a être utilisé dans le futur
	//{
	//	if (graph == null) return;
	//	graph.DataSource.StartBatch();

	//	// Effacer le curseur précédent

	//	graph.DataSource.ClearCategory(cursorCategorie);

	//	graph.DataSource.AddPointToCategory(cursorCategorie, t, -360);
	//	graph.DataSource.AddPointToCategory(cursorCategorie, t, 360);

	//	graph.DataSource.EndBatch();
	//}

	// =================================================================================================================================================================
	/// <summary> Trouver le noeud le plus près de la position de la souris (en tenant compte du ratio X vs Y du graphique). </summary>

	public int FindNearestNode()
	{
		int node = 0;
		float minDistanceToNode = 99999;
		for (int i = 0; i < MainParameters.Instance.joints.nodes[ddlUsed].T.Length; i++)
		{
			float distanceToNode = Mathf.Pow((mousePosSaveX - MainParameters.Instance.joints.nodes[ddlUsed].T[i]) * factorGraphRatioX, 2) +
				Mathf.Pow((mousePosSaveY - MainParameters.Instance.joints.nodes[ddlUsed].Q[i] * radToDeg) * factorGraphRatioY, 2);
			if (distanceToNode < minDistanceToNode)
			{
				node = i;
				minDistanceToNode = distanceToNode;
			}
		}
		return node;
	}

	// =================================================================================================================================================================
	/// <summary> Trouver le noeud précédent la position de la souris, selon l'échelle des temps (-1 = avant le premier noeud). </summary>

	public int FindPreviousNode()
	{
		int i = 0;
		while (i < MainParameters.Instance.joints.nodes[ddlUsed].T.Length && mousePosSaveX > MainParameters.Instance.joints.nodes[ddlUsed].T[i])
			i++;
		return i - 1;
	}

	// =================================================================================================================================================================
	/// <summary> Afficher un menu (panelAddRemoveNode ou panelCancelChanges) à la position de la souris. </summary>

	void DisplayContextMenu(GameObject panel)
	{
		Vector3 mousePosWorldSpace;
		Vector3[] menuPos = new Vector3[4];
		Vector3[] graphPos = new Vector3[4];
		graph.PointToWorldSpace(out mousePosWorldSpace, mousePosX, mousePosY);
		panel.GetComponent<RectTransform>().GetWorldCorners(menuPos);
		graph.GetComponent<RectTransform>().GetWorldCorners(graphPos);
		float width = menuPos[2].x - menuPos[1].x;
		if (mousePosWorldSpace.x < graphPos[2].x - width)
			panel.transform.position = mousePosWorldSpace + new Vector3(width / 2, 0, 0);		// On affiche le menu à droite
		else
			panel.transform.position = mousePosWorldSpace - new Vector3(width / 2, 0, 0);		// Pour éviter que le menu ne soit pas caché par le panneau Animator, on affiche le menu à gauche
		panel.SetActive(true);
	}

	// =================================================================================================================================================================
	/// <summary> Afficher une ou plusieurs courbes dans le graphique spécifié. </summary>

	public void DisplayCurves(GraphChart graphCurves, float[] t, float[] data)
	{
		float[,] data1 = new float[data.GetUpperBound(0) + 1,1];
		for (int i = 0; i <= data.GetUpperBound(0); i++)
			data1[i,0] = data[i];
		DisplayCurves(graphCurves, t, data1);
	}

	public void DisplayCurves(GraphChart graphCurves, float[] t, float[,] data)
	{
		if (graphCurves == null) return;
		graphCurves.DataSource.StartBatch();

		// Effacer les courbes précédentes

		for (int i = 0; i < dataCurvesCategories.Length; i++)
			graphCurves.DataSource.ClearCategory(dataCurvesCategories[i]);

		// Ajouter toutes les données dans la ou les nouvelles courbes Data
		// Calculer les valeurs minimum et maximum

		float tMin = 999999;
		float tMax = -999999;
		float dataMin = 999999;
		float dataMax = -999999;
		for (int i = 0; i <= data.GetUpperBound(1); i++)
		{
			for (int j = 0; j < t.Length; j++)
			{
				graphCurves.DataSource.AddPointToCategory(dataCurvesCategories[i], t[j], data[j, i]);
				if (i <= 0 && t[j] < tMin) tMin = t[j];
				if (i <= 0 && t[j] > tMax) tMax = t[j];
				if (data[j, i] < dataMin) dataMin = data[j, i];
				if (data[j, i] > dataMax) dataMax = data[j, i];
			}
		}

		// Définir les échelles des temps et des données

		graphCurves.DataSource.HorizontalViewOrigin = tMin;
		graphCurves.DataSource.HorizontalViewSize = tMax - tMin;
		if (tMax - tMin <= 2)
			graphCurves.GetComponent<HorizontalAxis>().MainDivisions.FractionDigits = 2;
		else
			graphCurves.GetComponent<HorizontalAxis>().MainDivisions.FractionDigits = 1;
		graphCurves.DataSource.VerticalViewOrigin = dataMin;
		graphCurves.DataSource.VerticalViewSize = dataMax - dataMin;
		if (dataMax - dataMin <= 0.2)
			graphCurves.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 3;
		else if (dataMax - dataMin <= 2)
			graphCurves.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 2;
		else
			graphCurves.GetComponent<VerticalAxis>().MainDivisions.FractionDigits = 1;

		graphCurves.DataSource.EndBatch();
	}
}
