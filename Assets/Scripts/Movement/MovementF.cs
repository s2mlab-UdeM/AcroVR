using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.FB;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées par les contrôles de la section Movement. </summary>

public class MovementF : MonoBehaviour
{
	public static MovementF Instance;

	public Text textFileName;

	public Dropdown dropDownDDLNames;
	public Dropdown dropDownInterpolation;
	public Button buttonLoad;
	public Image buttonLoadImage;
	public Button buttonSave;
	public Image buttonSaveImage;
	public Button buttonSymetricLeftRight;
	public Image buttonSymetricLeftRightImage;
	public Button buttonASymetricLeftRight;
	public Image buttonASymetricLeftRightImage;
	public Button buttonGraphSettings;
	public Image buttonGraphSettingsImage;
	public GameObject buttonSymetricLeftRightGameObject;
	public GameObject buttonAsymetricLeftRightGameObject;
	public GameObject panelGraphSettings;

	public Dropdown dropDownCondition;
	public InputField inputFieldSomersaultPosition;
	public InputField inputFieldTilt;
	public InputField inputFieldHorizontalSpeed;
	public InputField inputFieldVerticalSpeed;
	public InputField inputFieldSomersaultSpeed;
	public InputField inputFieldTwistSpeed;

	System.IntPtr hMainUnityWnd;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;

		Main.Instance.EnableDisableControls(false, false);
	}

	// =================================================================================================================================================================
	/// <summary> Liste déroulante Nom de l'articulation a été modifié. </summary>

	public void DropDownDDLNamesOnValueChanged(int value)
	{
		// Afficher la courbe des positions des angles pour l'articulation sélectionné par défaut

		GraphManager.Instance.DisplayCurveAndNodes(0, value, true);
		if (MainParameters.Instance.joints.nodes[value].ddlOppositeSide >= 0)
			GraphManager.Instance.DisplayCurveAndNodes(1, MainParameters.Instance.joints.nodes[value].ddlOppositeSide, true);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Charger a été appuyer. </summary>

	public void ButtonLoad()
	{
		// Sélection d'un fichier de données dans le répertoire des fichiers de simulation, par défaut

		ExtensionFilter[] extensions = new[]
		{
			new ExtensionFilter(MainParameters.Instance.languages.Used.movementLoadDataFileTxtFile, "txt"),
			new ExtensionFilter(MainParameters.Instance.languages.Used.movementLoadDataFileAllFiles, "*" ),
		};
		string dirSimulationFiles = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Tekphy\AcroVR\SimulationFiles");
		string fileName = FileBrowser.OpenSingleFile(MainParameters.Instance.languages.Used.movementLoadDataFileTitle, dirSimulationFiles, extensions);
		if (fileName.Length <= 0)
			return;

		// Lecture du fichier de données

		DataFileManager.Instance.ReadDataFiles(fileName);

		// Définir un nom racourci pour avoir accès à la structure Joints

		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Afficher le nom du fichier à l'écran

		textFileName.text = System.IO.Path.GetFileName(joints.fileName);

		// Mettre à jour les paramètres de décolage à l'écran

		dropDownCondition.interactable = true;
		dropDownCondition.value = joints.condition;
		inputFieldSomersaultPosition.interactable = true;
		inputFieldSomersaultPosition.text = string.Format("{0:0.0}", joints.takeOffParam.rotation);
		inputFieldTilt.interactable = true;
		inputFieldTilt.text = string.Format("{0:0.0}", joints.takeOffParam.tilt);
		inputFieldHorizontalSpeed.interactable = true;
		inputFieldHorizontalSpeed.text = string.Format("{0:0.0}", joints.takeOffParam.anteroposteriorSpeed);
		inputFieldVerticalSpeed.interactable = true;
		inputFieldVerticalSpeed.text = string.Format("{0:0.0}", joints.takeOffParam.verticalSpeed);
		inputFieldSomersaultSpeed.interactable = true;
		inputFieldSomersaultSpeed.text = string.Format("{0:0.000}", joints.takeOffParam.somersaultSpeed);
		inputFieldTwistSpeed.interactable = true;
		inputFieldTwistSpeed.text = string.Format("{0:0.000}", joints.takeOffParam.twistSpeed);

		// Initialisation du modèle de Lagrangien utilisé

		if (MainParameters.Instance.joints.lagrangianModelName == MainParameters.LagrangianModelNames.Sasha23ddl)
		{
			LagrangianModelSasha23ddl lagrangianModelSasha23ddl = new LagrangianModelSasha23ddl();
			MainParameters.Instance.joints.lagrangianModel = lagrangianModelSasha23ddl.GetParameters;
		}
		else
		{
			LagrangianModelSimple lagrangianModelSimple = new LagrangianModelSimple();
			MainParameters.Instance.joints.lagrangianModel = lagrangianModelSimple.GetParameters;
		}
		joints = MainParameters.Instance.joints;

		// Vérifier la définition des noeuds, si incorrect alors la corriger selon le modèle Lagrangien sélectionné

		int nDDL = 0;
		MainParameters.StrucNodes[] nodes = new MainParameters.StrucNodes[joints.lagrangianModel.q2.Length];
		int nNodes = joints.nodes.Length;
		MainParameters.StrucInterpolation interpolation = joints.nodes[0].interpolation;
		foreach (int i in joints.lagrangianModel.q2)
		{
			int j = 0;
			string ddlname = joints.lagrangianModel.ddlName[i - 1].ToLower();
			while (j < nNodes && !ddlname.Contains(joints.nodes[j].name.ToLower()))
				j++;
			if (j < nNodes)                                 // Articulations défini dans le fichier de données, le conserver
			{
				nodes[nDDL] = joints.nodes[j];
				nodes[nDDL].ddl = i;
			}
			else                                            // Articulations non défini dans le fichier de données, alors utilisé la définition de défaut selon le modèle Lagrangien
			{
				nodes[nDDL].ddl = i;
				nodes[nDDL].name = joints.lagrangianModel.ddlName[i - 1];
				nodes[nDDL].T = new float[3] { joints.duration * 0.25f, joints.duration * 0.5f, joints.duration * 0.75f };
				nodes[nDDL].Q = new float[3] { 0, 0, 0 };
				nodes[nDDL].interpolation = interpolation;
				nodes[nDDL].ddlOppositeSide = -1;
			}
			nDDL++;
		}

		// Trouver le numéro d'articulation (ddl) du côté opposé utilisé pour chacune des articulations, qui à un côté gauche ou droit

		for (int i = 0; i < nodes.Length; i++)
		{
			string nameOppSide = "";
			string name = nodes[i].name.ToLower();
			if (name.Contains("left") || name.Contains("right"))
			{
				if (name.Contains("left"))
					nameOppSide = "right" + name.Substring(name.IndexOf("left") + 4);
				else
					nameOppSide = "left" + name.Substring(name.IndexOf("right") + 5);
				for (int j = 0; j < nodes.Length; j++)
				{
					name = nodes[j].name.ToLower();
					if (name.Contains(nameOppSide))
						nodes[i].ddlOppositeSide = j;
				}
			}
		}
		MainParameters.Instance.joints.nodes = nodes;

		// Interpolation des positions des angles des articulations à traiter

		InterpolationDDL(-1);

		// Afficher la courbe des positions des angles pour l'articulation sélectionné par défaut

		DisplayDDL(true, 0, true);

		// Afficher la silhouette au temps t = 0

		AnimationF.Instance.PlayReset();
		AnimationF.Instance.Play(MainParameters.Instance.joints.q0, 0, 1);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Conserver a été appuyer. </summary>

	public void ButtonSave()
	{
		// Utilisation d'un répertoire de données par défaut, alors si ce répertoire n'existe pas, il faut le créer

		string dirSimulationFiles = Environment.ExpandEnvironmentVariables(@"%UserProfile%\Documents\AcroVR");
		if (!System.IO.Directory.Exists(dirSimulationFiles))
		{
			try
			{
				System.IO.Directory.CreateDirectory(dirSimulationFiles);
			}
			catch
			{
				dirSimulationFiles = "";
			}
		}

		// Sélection d'un ancien fichier de données qui sera modifié ou d'un nouveau fichier de données qui sera créé

		string fileName = FileBrowser.SaveFile(MainParameters.Instance.languages.Used.movementSaveDataFileTitle, dirSimulationFiles, "DefaultFile", "txt");
		if (fileName.Length <= 0)
			return;

		// Conserver un fichier de données

		DataFileManager.Instance.WriteDataFiles(fileName);

		// Afficher le nom du fichier à l'écran

		MainParameters.Instance.joints.fileName = fileName;
		textFileName.text = fileName;
	}

	// =================================================================================================================================================================
	/// <summary> Un des boutons Symétrie ou Asymétrie côté gauche et droit a été appuyer. </summary>

	public void ButtonSymetricLeftRight(bool symetricState)
	{
		buttonSymetricLeftRightGameObject.SetActive(!symetricState);
		buttonAsymetricLeftRightGameObject.SetActive(symetricState);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Paramètres du Graphique a été appuyer. </summary>

	public void ButtonGraphSettings()
	{
		GraphManager.Instance.mouseTracking = false;
		panelGraphSettings.SetActive(true);
		GraphSettings.Instance.Init();
	}

	// =================================================================================================================================================================
	/// <summary> Ajouter un noeud, après le noeud précédent la position de la souris. </summary>

	public void AddNode()
	{
		// Trouver le numéro du noeud précédent celui qui sera ajouter

		int node = GraphManager.Instance.FindPreviousNode();

		// Ajouter le noeud à la liste des noeuds

		int ddl = GraphManager.Instance.ddlUsed;
		float[] T = new float[MainParameters.Instance.joints.nodes[ddl].T.Length + 1];
		float[] Q = new float[MainParameters.Instance.joints.nodes[ddl].Q.Length + 1];
		for (int i = 0; i <= node; i++)
		{
			T[i] = MainParameters.Instance.joints.nodes[ddl].T[i];
			Q[i] = MainParameters.Instance.joints.nodes[ddl].Q[i];
		}
		T[node + 1] = GraphManager.Instance.mousePosSaveX;
		Q[node + 1] = GraphManager.Instance.mousePosSaveY * Mathf.PI / 180;
		for (int i = node + 1; i < MainParameters.Instance.joints.nodes[ddl].T.Length; i++)
		{
			T[i + 1] = MainParameters.Instance.joints.nodes[ddl].T[i];
			Q[i + 1] = MainParameters.Instance.joints.nodes[ddl].Q[i];
		}
		MainParameters.Instance.joints.nodes[ddl].T = MathFunc.MatrixCopy(T);
		MainParameters.Instance.joints.nodes[ddl].Q = MathFunc.MatrixCopy(Q);

		// Interpolation des positions des angles pour l'articulation sélectionnée

		MovementF.Instance.InterpolationDDL(ddl);

		// Afficher la courbe des positions des angles pour l'articulation sélectionnée

		DisplayDDL(false, ddl, false);

		// Afficher la silhouette au temps du noeud modifié

		AnimationF.Instance.PlayReset();
		int frame = (int)Mathf.Round(GraphManager.Instance.mousePosSaveX / MainParameters.Instance.joints.lagrangianModel.dt);
		if (frame > MainParameters.Instance.joints.q0.GetUpperBound(1)) frame = MainParameters.Instance.joints.q0.GetUpperBound(1);
		AnimationF.Instance.Play(MainParameters.Instance.joints.q0, frame, 1);

		// Désactiver l'action du clic du bouton droit de la souris

		GraphManager.Instance.mouseRightButtonON = false;
	}

	// =================================================================================================================================================================
	/// <summary> Effacer un noeud, celui qui est le plus près de la position de la souris. </summary>

	public void RemoveNode()
	{
		// Vérifier qu'il reste au moins 2 noeuds après la suppression, sinon ne pas autoriser la suppression

		int ddl = GraphManager.Instance.ddlUsed;
		if (MainParameters.Instance.joints.nodes[ddl].T.Length < 3 || MainParameters.Instance.joints.nodes[ddl].Q.Length < 3)
		{
			GraphManager.Instance.panelMoveErrMsg.GetComponentInChildren<Text>().text = MainParameters.Instance.languages.Used.errorMsgNotEnoughNodes;
			GraphManager.Instance.mouseTracking = false;
			GraphManager.Instance.panelMoveErrMsg.SetActive(true);
			return;
		}

		// Trouver le noeud le plus près de la position de la souris (en tenant compte du ratio X vs Y du graphique), ça sera ce noeud qui sera effacé

		int node = GraphManager.Instance.FindNearestNode();

		// Effacer le noeud de la liste des noeuds

		float[] T = new float[MainParameters.Instance.joints.nodes[ddl].T.Length - 1];
		float[] Q = new float[MainParameters.Instance.joints.nodes[ddl].Q.Length - 1];
		for (int i = 0; i < node; i++)
		{
			T[i] = MainParameters.Instance.joints.nodes[ddl].T[i];
			Q[i] = MainParameters.Instance.joints.nodes[ddl].Q[i];
		}
		for (int i = node + 1; i < MainParameters.Instance.joints.nodes[ddl].T.Length; i++)
		{
			T[i - 1] = MainParameters.Instance.joints.nodes[ddl].T[i];
			Q[i - 1] = MainParameters.Instance.joints.nodes[ddl].Q[i];
		}
		MainParameters.Instance.joints.nodes[ddl].T = MathFunc.MatrixCopy(T);
		MainParameters.Instance.joints.nodes[ddl].Q = MathFunc.MatrixCopy(Q);

		// Interpolation des positions des angles pour l'articulation sélectionnée

		MovementF.Instance.InterpolationDDL(ddl);

		// Afficher la courbe des positions des angles pour l'articulation sélectionnée

		DisplayDDL(false, ddl, false);

		// Afficher la silhouette au temps du noeud modifié

		AnimationF.Instance.PlayReset();
		int frame = (int)Mathf.Round(GraphManager.Instance.mousePosSaveX / MainParameters.Instance.joints.lagrangianModel.dt);
		if (frame > MainParameters.Instance.joints.q0.GetUpperBound(1)) frame = MainParameters.Instance.joints.q0.GetUpperBound(1);
		AnimationF.Instance.Play(MainParameters.Instance.joints.q0, frame, 1);

		// Désactiver l'action du clic du bouton droit de la souris

		GraphManager.Instance.mouseRightButtonON = false;
	}

	// =================================================================================================================================================================
	/// <summary> Interpolation des positions des angles des articulations à traiter. </summary>

	public void InterpolationDDL(int ddl)
	{
		// Initialisation des vecteurs contenant les temps et les positions des angles des articulations interpolés

		int n = (int)(MainParameters.Instance.joints.duration / MainParameters.Instance.joints.lagrangianModel.dt) + 1;
		float[] t0 = new float[n];
		float[,] q0 = new float[MainParameters.Instance.joints.lagrangianModel.nDDL, n];

		// Interpolation des positions des angles des articulations à traiter

		GenerateQ0 generateQ0 = new GenerateQ0(MainParameters.Instance.joints.lagrangianModel, MainParameters.Instance.joints.duration, ddl + 1, out t0, out q0);
		generateQ0.ToString();                  // Pour enlever un warning lors de la compilation

		// Conserver les données interpolées dans MainParameters

		if (ddl < 0)
		{
			MainParameters.Instance.joints.t0 = t0;
			MainParameters.Instance.joints.q0 = q0;
		}
		else
			for (int i = 0; i <= q0.GetUpperBound(1); i++)
				MainParameters.Instance.joints.q0[ddl, i] = q0[ddl, i];
	}

	// =================================================================================================================================================================
	/// <summary> Afficher la courbe des positions des angles pour l'articulation sélectionné, ainsi que les noeuds. </summary>

	public void DisplayDDL(bool options,int ddl, bool axisRange)
	{
		if (ddl >= 0)
		{
			GraphManager.Instance.DisplayCurveAndNodes(0, ddl, axisRange);
			if (MainParameters.Instance.joints.nodes[ddl].ddlOppositeSide >= 0)
				GraphManager.Instance.DisplayCurveAndNodes(1, MainParameters.Instance.joints.nodes[ddl].ddlOppositeSide, axisRange);
		}
		if (options)
		{
			List<string> dropDownOptions = new List<string>();
			for (int i = 0; i < MainParameters.Instance.joints.nodes.Length; i++)
			{
				if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLHipFlexion.ToLower())
					dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLHipFlexion);
				else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLKneeFlexion.ToLower())
					dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLKneeFlexion);
				else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLLeftArmFlexion.ToLower())
					dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLLeftArmFlexion);
				else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLLeftArmAbduction.ToLower())
					dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLLeftArmAbduction);
				else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLRightArmFlexion.ToLower())
					dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLRightArmFlexion);
				else if (MainParameters.Instance.joints.nodes[i].name.ToLower() == MainParameters.Instance.languages.english.movementDDLRightArmAbduction.ToLower())
					dropDownOptions.Add(MainParameters.Instance.languages.Used.movementDDLRightArmAbduction);
				else
					dropDownOptions.Add(MainParameters.Instance.joints.nodes[i].name);
			}
			dropDownDDLNames.ClearOptions();
			dropDownDDLNames.AddOptions(dropDownOptions);
			if (ddl >= 0)
				dropDownDDLNames.value = ddl;
		}
	}
}
