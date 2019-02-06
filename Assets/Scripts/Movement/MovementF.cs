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
	public Dropdown dropDownNumIntervals;
	public Button buttonLoad;
	public Image buttonLoadImage;
	public Button buttonSave;
	public Image buttonSaveImage;

	public Dropdown dropDownCondition;
	public InputField inputFieldInitialRotation;
	public InputField inputFieldTilt;
	public InputField inputFieldHorizontalSpeed;
	public InputField inputFieldVerticalSpeed;
	public InputField inputFieldSomersaultSpeed;
	public InputField inputFieldTwistSpeed;

	System.IntPtr hMainUnityWnd;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;

		Main.Instance.EnableDisableControls(false, false);
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

		textFileName.text = joints.fileName;

		// Mettre à jour les paramètres de décolage à l'écran

		dropDownCondition.interactable = true;
		dropDownCondition.value = joints.condition;
		inputFieldInitialRotation.interactable = true;
		inputFieldInitialRotation.text = string.Format("{0:0.0}", joints.takeOffParam.rotation);
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

		// Initialisation des vecteurs contenant les temps et les positions des angles des articulations interpolés

		int n = (int)(joints.duration / joints.lagrangianModel.dt) + 1;
		float[] t0 = new float[n];
		float[,] q0 = new float[joints.lagrangianModel.nDDL, n];

		// Interpolation des positions des angles des articulations à traiter

		GenerateQ0 generateQ0 = new GenerateQ0(joints.lagrangianModel, joints.duration, 0, out t0, out q0);
		generateQ0.ToString();                  // Pour enlever un warning lors de la compilation

		// Conserver les données interpolées dans MainParameters

		MainParameters.Instance.joints.t0 = t0;
		MainParameters.Instance.joints.q0 = q0;
		MainParameters.Instance.joints.numberFrames = 1;

		// Afficher la courbe des positions des angles pour l'articulation sélectionné par défaut

		GraphManager.Instance.DisplayCurveAndNodes(0, 0, t0, q0, nodes);
		if (nodes[0].ddlOppositeSide >= 0)
			GraphManager.Instance.DisplayCurveAndNodes(1, nodes[0].ddlOppositeSide, t0, q0, nodes);
		List<string> dropDownOptions = new List<string>();
		for (int i = 0; i < nodes.Length; i++)
			dropDownOptions.Add(nodes[i].name);
		dropDownDDLNames.ClearOptions();
		dropDownDDLNames.AddOptions(dropDownOptions);
		dropDownDDLNames.value = 0;

		// Afficher la silhouette au temps t = 0

		AnimationF.Instance.PlayReset();
		AnimationF.Instance.Play(q0);

		// Activer les contrôles disponible à l'utilisateur à l'écran

		Main.Instance.EnableDisableControls(true, false);
	}
}
