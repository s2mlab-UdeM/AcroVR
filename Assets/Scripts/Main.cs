using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
{
	public Toggle toggleDynamique;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
	}

	// =================================================================================================================================================================
	/// <summary> Exécution du script à chaque frame. </summary>

	void Update ()
	{ }

	// =================================================================================================================================================================
	/// <summary> Bouton Charger a été appuyer. </summary>

	public void ButtonLoad()
	{
		System.Windows.Forms.OpenFileDialog openDialog = new System.Windows.Forms.OpenFileDialog();

		openDialog.InitialDirectory = "c:\\";
		openDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
		openDialog.FilterIndex = 1;

		openDialog.ShowDialog();

		//Stream myStream = null;
		//OpenFileDialog openFileDialog1 = new OpenFileDialog();

		//openFileDialog1.InitialDirectory = "c:\\";
		//openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
		//openFileDialog1.FilterIndex = 2;
		//openFileDialog1.RestoreDirectory = true;

		//if (openFileDialog1.ShowDialog() == DialogResult.OK)
		//{
		//	try
		//	{
		//		if ((myStream = openFileDialog1.OpenFile()) != null)
		//		{
		//			using (myStream)
		//			{
		//				// Insert code to read the stream here.
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
		//	}
		//}
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Démarrer a été appuyer. </summary>

	public void ButtonStart()
	{
		if (toggleDynamique.isOn)
			MainParameters.Instance.displayType = MainParameters.ListDisplayType.Dynamique;
		else
			MainParameters.Instance.displayType = MainParameters.ListDisplayType.Statique;

		ReadSomersaultSplinesDataFiles();

		Sasha23ddl.Instance.InitLagrangianModel();

		float[] aa = MathFunc.Instance.Fnval(new float[1] { 0 }, MainParameters.Instance.splines.T, MainParameters.Instance.splines.coefs[0].pp);
		for (int i = 0; i < aa.Length; i++)
			Debug.Log(string.Format("i = {0}, aa = {1}", i, aa[i]));

		float bb = MathFunc.Instance.Fnval(0, MainParameters.Instance.splines.T, MainParameters.Instance.splines.coefs[0].pp);
		Debug.Log(string.Format("bb = {0}", bb));
		AnimationF.Instance.Play();
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Quitter a été appuyer. </summary>

	public void ButtonQuit()
	{
		Application.Quit();
	}

	// =================================================================================================================================================================
	/// <summary> Lecture des angles des articulations (DDL) d'un fichier de données Somersault. Les données seront conservé dans une structure de classe MainParameters. </summary>

	void ReadSomersaultSplinesDataFiles()
	{
		// Lecture des données du fichier de données Somersault

		string[] fileLines = System.IO.File.ReadAllLines(@"C:\Devel\AcroVR\Données\SomersaultPP.txt");

		// Initialisation de quelques paramètres

		int numDDL = 23;                            // 17 DDL obtenus du fichier de données + 6 constants (= 0)
		int numSkipLines = 1;                       // Ignorer la première ligne du fichier de données
		int numFrames = fileLines.Length - numSkipLines - 1; // Ignorer la dernière ligne du fichier de données (contient juste le temps)
		string[] values;

		// Initialisation de la structure splines

		MainParameters.Instance.splines.T = new float[numFrames + 1];
		MainParameters.Instance.splines.coefs = new MainParameters.StrucCoefSplines[numDDL];
		for (int i = 0; i < numDDL; i++)
		{
			MainParameters.Instance.splines.coefs[i].pp = new float[numFrames,4];
			MainParameters.Instance.splines.coefs[i].ppdot = new float[numFrames, 3];
			MainParameters.Instance.splines.coefs[i].ppddot = new float[numFrames, 2];
		}

		// Copier les données dans la structure splines

		for (int i = 0; i < numFrames; i++)
		{
			values = Regex.Split(fileLines[i + numSkipLines], ",");
			MainParameters.Instance.splines.T[i] = float.Parse(values[0]);      // Tous les temps des DDL sont identique
			for (int j = 0; j < numDDL; j++)
			{
				int jj = j * 9;
				MainParameters.Instance.splines.coefs[j].pp[i, 0] = float.Parse(values[jj + 1]);
				MainParameters.Instance.splines.coefs[j].pp[i, 1] = float.Parse(values[jj + 2]);
				MainParameters.Instance.splines.coefs[j].pp[i, 2] = float.Parse(values[jj + 3]);
				MainParameters.Instance.splines.coefs[j].pp[i, 3] = float.Parse(values[jj + 4]);
				MainParameters.Instance.splines.coefs[j].ppdot[i, 0] = float.Parse(values[jj + 5]);
				MainParameters.Instance.splines.coefs[j].ppdot[i, 1] = float.Parse(values[jj + 6]);
				MainParameters.Instance.splines.coefs[j].ppdot[i, 2] = float.Parse(values[jj + 7]);
				MainParameters.Instance.splines.coefs[j].ppddot[i, 0] = float.Parse(values[jj + 8]);
				MainParameters.Instance.splines.coefs[j].ppddot[i, 1] = float.Parse(values[jj + 9]);
			}
		}
		values = Regex.Split(fileLines[numFrames + numSkipLines], ",");
		MainParameters.Instance.splines.T[numFrames] = float.Parse(values[0]);
		MainParameters.Instance.numberOfFrames = numFrames;
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
