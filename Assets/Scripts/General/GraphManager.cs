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
	public GameObject panelAddRemoveNode;
	public GameObject buttonAddNode;
	public GameObject buttonRemoveNode;
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
	//string cursorCategorie;

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
		//cursorCategorie = "Cursor";
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

		if (Input.GetMouseButtonDown(0) && mouseRightButtonON && !MouseManager.Instance.IsOnGameObject(panelAddRemoveNode))
		{
			panelAddRemoveNode.SetActive(false);
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

		// Bouton gauche de la souris appuyé => déplacement d'un noeud (Bouton gauche appuyé, effacé le noeud temporaire précédent s'il y a lieu)

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
			graph.DataSource.AddPointToCategory(nodesTemp1Category, mousePosX, mousePosY);
			graph.DataSource.EndBatch();
		}

		// Bouton gauche de la souris appuyé => déplacement d'un noeud (Bouton gauche relâché, effacé le noeud temporaire et modifier le noeud sélectionné, si la position est correct)

		if (Input.GetMouseButtonUp(0) && mouseLeftButtonON)
		{
			// Enlever les noeuds temporaires

			RemoveNodesTemp();

			// Vérifier si la nouvelle position du noeud est correct

			if ((nodeUsed <= 0 && mousePosX >= MainParameters.Instance.joints.nodes[ddlUsed].T[1]) || (nodeUsed >= numNodes - 1 && mousePosX <= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed - 1]) ||
				(nodeUsed > 0 && nodeUsed < numNodes - 1 && (mousePosX <= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed - 1] || mousePosX >= MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed + 1])))
			{
				Main.Instance.EnableDisableControls(false, true);
				panelMoveErrMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgInvalidNodePosition;
				GraphManager.Instance.mouseTracking = false;
				panelMoveErrMsg.SetActive(true);
				return;
			}

			// Conserver le nouveau noeud

			MainParameters.Instance.joints.nodes[ddlUsed].T[nodeUsed] = (float)mousePosX;
			MainParameters.Instance.joints.nodes[ddlUsed].Q[nodeUsed] = (float)mousePosY / radToDeg;

			// Interpolation des positions des angles pour l'articulation sélectionnée

			MovementF.Instance.InterpolationDDL(ddlUsed);

			// Afficher la courbe des positions des angles pour l'articulation sélectionnée

			MovementF.Instance.DisplayDDL(false, ddlUsed, false);

			// Afficher la silhouette au temps du noeud modifié

			AnimationF.Instance.PlayReset();
			int frame = (int)Mathf.Round((float)mousePosX / MainParameters.Instance.joints.lagrangianModel.dt);
			if (frame > MainParameters.Instance.joints.q0.GetUpperBound(1)) frame = MainParameters.Instance.joints.q0.GetUpperBound(1);
			AnimationF.Instance.Play(MainParameters.Instance.joints.q0, frame, 1);
		}

		// Bouton droit de la souris appuyé => ajouter/effacer un noeud

		if (Input.GetMouseButtonDown(1))
		{
			mouseRightButtonON = true;
			mousePosSaveX = (float)mousePosX;
			mousePosSaveY = (float)mousePosY;

			buttonAddNode.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.movementButtonAddNode;
			buttonRemoveNode.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.movementButtonRemoveNode;
			Vector3 mousePosWorldSpace;
			graph.PointToWorldSpace(out mousePosWorldSpace, mousePosX, mousePosY);
			panelAddRemoveNode.transform.position = mousePosWorldSpace + new Vector3(10, 0, 0);
			panelAddRemoveNode.SetActive(true);
		}

		//if (Input.GetMouseButtonDown(2))
		//	Debug.Log("Pressed middle click.");
	}

	// =================================================================================================================================================================
	/// <summary> Bouton OK a été appuyer. </summary>

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
}
