using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour {

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
		ReadSomersaultSplinesDataFiles();
		Sasha23ddl.Instance.InitLagrangianModel();

		// check nodes structures and update if required according to the LagrangianModel

		int iN = 0;
		MainParameters.Instance.nodes = new MainParameters.StrucNodes[MainParameters.Instance.lagrangianModel.nSOL];
		foreach (int i in MainParameters.Instance.lagrangianModel.q2)
		{
			// initialize the DDL to default setup

			MainParameters.Instance.nodes[iN].ddlNum = i;
			MainParameters.Instance.nodes[iN].ddlName = MainParameters.Instance.lagrangianModel.ddlName[i - 1];
			//MainParameters.Instance.nodes[iN].T = MainParameters.Instance.pp[MainParameters.Instance.lagrangianModel.q2[iN]].breaks;
			//	Nodes(iN).Q = fnval(MainParameters.Instance.pp[MainParameters.Instance.lagrangianModel.q2[iN]].breaks, MainParameters.Instance.pp[MainParameters.Instance.lagrangianModel.q2[iN]].pp);
			//	Nodes(iN).Qdot = fnval(MainParameters.Instance.pp[MainParameters.Instance.lagrangianModel.q2[iN]].breaks, MainParameters.Instance.pp[MainParameters.Instance.lagrangianModel.q2[iN]].ppdot);
			iN++;
		}

		double[] Q = new double[MainParameters.Instance.jointsAngles.Length + 6];
		for (int i = 0; i < 6; i++)
			Q[i] = 0;
		for (int i = 6; i < Q.Length; i++)
			Q[i] = MainParameters.Instance.jointsAngles[i - 6].Q[0];

		AnimationF.Instance.Play(Q);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Quitter a été appuyer. </summary>

	public void ButtonQuit()
	{
		Application.Quit();
	}

	// =================================================================================================================================================================
	/// <summary> Lecture de tous les fichiers de données Somersault Splines. Les données sont conservé dans des structures de classe MainParameters. </summary>

	void ReadSomersaultSplinesDataFiles()
	{
		string somersaultSplinesDataFilenames = @"C:\Devel\AcroVR\Données\SomersaultData.txt";
		int numDDL = 17;

		MainParameters.Instance.jointsAngles = new MainParameters.StrucJointsAngles[numDDL];
		string[] fileLines = System.IO.File.ReadAllLines(somersaultSplinesDataFilenames);
		for (int i = 0; i < numDDL; i++)
		{
			MainParameters.Instance.jointsAngles[i].T = new float[fileLines.Length];
			MainParameters.Instance.jointsAngles[i].Q = new float[fileLines.Length];
		}
		for (int i = 3; i < fileLines.Length; i++)
		{
			string[] values = Regex.Split(fileLines[i], ",");
			for (int j = 0; j < numDDL; j++)
			{
				MainParameters.Instance.jointsAngles[j].T[i - 3] = float.Parse(values[j * 2]);
				MainParameters.Instance.jointsAngles[j].Q[i - 3] = float.Parse(values[j * 2 + 1]);
			}
		}
	}

	//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("{0}", System.Environment.NewLine));
}
