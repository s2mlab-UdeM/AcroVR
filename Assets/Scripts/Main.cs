using UnityEngine;
using UnityEngine.UI;
//using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;

// =================================================================================================================================================================
/// <summary> Script principal du logiciel AcroVR. </summary>

public class Main : MonoBehaviour
{

//	public GameObject animatorF;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
	}

	// =================================================================================================================================================================
	/// <summary> Exécution du script à chaque frame. </summary>

	void Update ()
	{
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
