using UnityEngine;
using UnityEngine.UI;
//using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
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
	{ }

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

		LagrangianModelSimple lagrangianModelSimple = new LagrangianModelSimple();
		LagrangianModelManager.StrucLagrangianModel lagrangianModel = lagrangianModelSimple.GetParameters;

		// Vérifier la définition des noeuds, si incorrect alors la corriger selon le modèle Lagrangien sélectionné

		int nDDL = 0;
		MainParameters.StrucNodes[] nodes = new MainParameters.StrucNodes[lagrangianModel.q2.Length];
		int nNodes = MainParameters.Instance.joints.nodes.Length;
		MainParameters.StrucInterpolation interpolation = MainParameters.Instance.joints.nodes[0].interpolation;
		foreach (int i in lagrangianModel.q2)
		{
			int j = 0;
			string ddlname = lagrangianModel.ddlName[i - 1].ToLower();
			while (j < nNodes && !ddlname.Contains(MainParameters.Instance.joints.nodes[j].name.ToLower()))
				j++;
			if (j < nNodes)									// Articulations défini dans le fichier de données, le conserver
			{
				nodes[nDDL] = MainParameters.Instance.joints.nodes[j];
				nodes[nDDL].ddl = i;
			}
			else											// Articulations non défini dans le fichier de données, alors utilisé la définition de défaut selon le modèle Lagrangien
			{
				nodes[nDDL].ddl = i;
				nodes[nDDL].name = lagrangianModel.ddlName[i - 1];
				nodes[nDDL].T = new float[3] { MainParameters.Instance.joints.duration * 0.25f, MainParameters.Instance.joints.duration * 0.5f, MainParameters.Instance.joints.duration * 0.75f };
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

		int n = (int)(MainParameters.Instance.joints.duration / lagrangianModel.dt) + 1;
		float[] t0 = new float[n];
		float[,] q0 = new float[lagrangianModel.nDDL, n];

		// Interpolation des positions des angles des articulations à traiter

		float[,] q0t = new float[lagrangianModel.q2.Length, n];
		GenerateQ0 generateQ0 = new GenerateQ0(lagrangianModel, MainParameters.Instance.joints.duration, 0, out t0, out q0t);
		generateQ0.ToString();                  // Pour enlever un warning lors de la compilation
		for (int i = 0; i < q0t.GetLength(0); i++)
			for (int j = 0; j < q0t.GetLength(1); j++)
				q0[lagrangianModel.q2[i] - 1, j] = q0t[i, j];

		// Conserver les données interpolées dans MainParameters

		MainParameters.Instance.joints.t0 = t0;
		MainParameters.Instance.joints.q0 = q0;

		// Afficher la courbe des positions des angles pour l'articulation sélectionné par défaut

		GraphManager.Instance.DisplayCurveAndNodes(0, 0, t0, q0, MainParameters.Instance.joints.nodes);
		if (MainParameters.Instance.joints.nodes[0].ddlOppositeSide >= 0)
			GraphManager.Instance.DisplayCurveAndNodes(1, MainParameters.Instance.joints.nodes[0].ddlOppositeSide, t0, q0, MainParameters.Instance.joints.nodes);
		List<string> dropDownOptions = new List<string>();
		for (int i = 0; i < MainParameters.Instance.joints.nodes.Length; i++)
			dropDownOptions.Add(MainParameters.Instance.joints.nodes[i].name);
		dropDownDDLNames.ClearOptions();
		dropDownDDLNames.AddOptions(dropDownOptions);
		dropDownDDLNames.value = 0;
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Démarrer a été appuyer. </summary>

	public void ButtonStart()
	{
		//Sasha23ddl.Instance.InitLagrangianModel();

		//float[] aa = MathFunc.Instance.Fnval(new float[1] { 0 }, MainParameters.Instance.splines.T, MainParameters.Instance.splines.coefs[0].pp);
		//for (int i = 0; i < aa.Length; i++)
		//	Debug.Log(string.Format("i = {0}, aa = {1}", i, aa[i]));

		//float bb = MathFunc.Instance.Fnval(0, MainParameters.Instance.splines.T, MainParameters.Instance.splines.coefs[0].pp);
		//Debug.Log(string.Format("bb = {0}", bb));
		//AnimationF.Instance.Play();
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Quitter a été appuyer. </summary>

	public void ButtonQuit()
	{
		Application.Quit();
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
