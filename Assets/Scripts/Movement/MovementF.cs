using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées par les contrôles de la section Movement. </summary>

public class MovementF : MonoBehaviour
{
	public Text textFileName;

	public Dropdown dropDownDDLNames;

	public Dropdown dropDownCondition;
	public InputField inputFieldInitialRotation;
	public InputField inputFieldTilt;
	public InputField inputFieldHorizontalSpeed;
	public InputField inputFieldVerticalSpeed;
	public InputField inputFieldSomersaultSpeed;
	public InputField inputFieldTwistSpeed;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		dropDownCondition.interactable = false;
		inputFieldInitialRotation.interactable = false;
		inputFieldTilt.interactable = false;
		inputFieldHorizontalSpeed.interactable = false;
		inputFieldVerticalSpeed.interactable = false;
		inputFieldSomersaultSpeed.interactable = false;
		inputFieldTwistSpeed.interactable = false;
	}

	// =================================================================================================================================================================
	/// <summary> Exécution du script à chaque frame. </summary>

	void Update ()
	{
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Charger a été appuyer. </summary>

	public void ButtonLoad()
	{
		// Sélection d'un fichier de données

		System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();
		openDialog.InitialDirectory = @"c:\Devel\AcroVR\Données";
		openDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
		openDialog.FilterIndex = 1;
		if (openDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
			return;

		// Lecture du fichier de données

		DataFileManager.Instance.ReadDataFiles(openDialog.FileName);
		//DataFileManager.Instance.ReadDataFiles(@"C:\Devel\AcroVR\Données\1.3.SautDroit.txt");

		// Définir un nom racourci pour avoir accès à la structure Joints

		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Afficher le nom du fichier à l'écran

		textFileName.text = joints.fileName;

		// Mettre à jour les paramètres de décolage à l'écran

		dropDownCondition.interactable = true;
		dropDownCondition.value = joints.condition;
		inputFieldInitialRotation.interactable = true;
		inputFieldInitialRotation.text = joints.takeOffParam.rotation.ToString();
		inputFieldTilt.interactable = true;
		inputFieldTilt.text = joints.takeOffParam.tilt.ToString();
		inputFieldHorizontalSpeed.interactable = true;
		inputFieldHorizontalSpeed.text = joints.takeOffParam.anteroposteriorSpeed.ToString();
		inputFieldVerticalSpeed.interactable = true;
		inputFieldVerticalSpeed.text = joints.takeOffParam.verticalSpeed.ToString();
		inputFieldSomersaultSpeed.interactable = true;
		inputFieldSomersaultSpeed.text = joints.takeOffParam.somersaultSpeed.ToString();
		inputFieldTwistSpeed.interactable = true;
		inputFieldTwistSpeed.text = joints.takeOffParam.twistSpeed.ToString();

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

		float[,] q0t = new float[joints.lagrangianModel.q2.Length, n];
		GenerateQ0 generateQ0 = new GenerateQ0(joints.lagrangianModel, joints.duration, 0, out t0, out q0t);
		generateQ0.ToString();                  // Pour enlever un warning lors de la compilation
		for (int i = 0; i < q0t.GetLength(0); i++)
			for (int j = 0; j < q0t.GetLength(1); j++)
				q0[joints.lagrangianModel.q2[i] - 1, j] = q0t[i, j];

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

		AnimationF.Instance.Play();
	}
}
