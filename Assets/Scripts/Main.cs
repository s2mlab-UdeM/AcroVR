using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
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
	/// <summary> Bouton Démarrer a été appuyer. </summary>

	public void ButtonStart()
	{
		if (toggleDynamique.isOn)
			MainParameters.Instance.displayType = MainParameters.ListDisplayType.Dynamique;
		else
			MainParameters.Instance.displayType = MainParameters.ListDisplayType.Statique;

		ReadSomersaultSplinesDataFiles();

		Sasha23ddl.Instance.InitLagrangianModel();

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

		string[] fileLines = System.IO.File.ReadAllLines(@"C:\Devel\AcroVR\Données\SomersaultData.txt");

		// Initialisation de quelques paramètres

		int numDDL = 23;							// 17 DDL obtenus du fichier de données + 6 constants (= 0)
		int numFrames = fileLines.Length - 3;		// Ignorer les 3 premières lignes du fichier de données
		MainParameters.Instance.jointsAngles = new MainParameters.StrucJointsAngles[numFrames];

		// Initialisation de la structure jointsAngles et mise à zéro des 6 premiers DDL

		for (int i = 0; i < numFrames; i++)
		{
			MainParameters.Instance.jointsAngles[i].T = 0;
			MainParameters.Instance.jointsAngles[i].Q = new float[numDDL];
			for (int j = 0; j < 6; j++)
				MainParameters.Instance.jointsAngles[i].Q[j] = 0;
		}

		// Copier les données dans la structure jointsAngles

		for (int i = 0; i < numFrames; i++)
		{
			string[] values = Regex.Split(fileLines[i + 3], ",");
			MainParameters.Instance.jointsAngles[i].T = float.Parse(values[0]);		// Tous les temps des DDL sont identique
			for (int j = 0; j < numDDL - 6; j++)
				MainParameters.Instance.jointsAngles[i].Q[j + 6] = float.Parse(values[j * 2 + 1]);
		}
		MainParameters.Instance.numberOfFrames = numFrames;
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
