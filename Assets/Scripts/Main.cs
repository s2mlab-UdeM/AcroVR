using UnityEngine;
using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
{
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

        // Mettre à jour les paramètres de décolage à l'écran

        dropDownCondition.interactable = true;
        dropDownCondition.value = MainParameters.Instance.joints.condition;
        inputFieldInitialRotation.interactable = true;
        inputFieldInitialRotation.text = MainParameters.Instance.joints.takeOffParam.rotation.ToString();
        inputFieldTilt.interactable = true;
        inputFieldTilt.text = MainParameters.Instance.joints.takeOffParam.tilt.ToString();
        inputFieldHorizontalSpeed.interactable = true;
        inputFieldHorizontalSpeed.text = MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed.ToString();
        inputFieldVerticalSpeed.interactable = true;
        inputFieldVerticalSpeed.text = MainParameters.Instance.joints.takeOffParam.verticalSpeed.ToString();
        inputFieldSomersaultSpeed.interactable = true;
        inputFieldSomersaultSpeed.text = MainParameters.Instance.joints.takeOffParam.somersaultSpeed.ToString();
        inputFieldTwistSpeed.interactable = true;
        inputFieldTwistSpeed.text = MainParameters.Instance.joints.takeOffParam.twistSpeed.ToString();

		// Initialisation du modèle de Lagrangien utilisé

		LagrangianModelSimple lagrangianModelSimple = new LagrangianModelSimple();
		LagrangianModelManager.StrucLagrangianModel lagrangianModel = lagrangianModelSimple.GetParameters;

		//  Initialisation des vecteurs contenant les temps et les positions des angles des articulations interpolés

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

		//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("GenerateQ0"));
		//for (int i = 0; i < q0.GetLength(1); i++)
		//{
		//	System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}{1}: ", System.Environment.NewLine, t0[i]));
		//	for (int j = 0; j < q0.GetLength(0); j++)
		//		System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}, ", q0[j, i]));
		//}
		//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));

		GraphManager.Instance.DisplayCurveAndNodes(0, t0, q0, MainParameters.Instance.joints.nodes);
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
