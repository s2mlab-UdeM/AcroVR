using UnityEngine;
using System.Collections;

// =================================================================================================================================================================
/// <summary>
/// Cette classe contient les paramètres généraux et globaux utilisés dans le logiciel.
/// N'hérite pas de monobehavior et est un singleton.
/// </summary>

public class MainParameters
{
	#region JointsAnglesParameters
	/// <summary> Description de la structure contenant les données réelles des angles des articulations. </summary>
	public struct StrucJointsAngles
	{
		public float[] T;
		public float[] Q;
	}

	/// <summary> Structure contenant les données réelles des angles des articulations. </summary>
	public StrucJointsAngles[] jointsAngles;
	#endregion

	#region SplinesParameters
	/// <summary> Description de la structure contenant les données splines. </summary>
	public struct StrucSplines
	{
		public float[] breaks;
		public float[,] coefs;
	}

	/// <summary> Structure contenant les données splines (pp). </summary>
	public StrucSplines[] pp;

	/// <summary> Structure contenant les données splines (ppdot). </summary>
	public StrucSplines[] ppdot;

	/// <summary> Structure contenant les données splines (ppddot). </summary>
	public StrucSplines[] ppddot;
	#endregion

	#region NodesParameters
	///// <summary> Description de la structure contenant les données relatifs aux noeuds. </summary>
	public struct StrucNodes
	{
		public int ddlNum;
		public string ddlName;
		public float[] T;
		public float[] Q;
	}

	/// <summary> Structure contenant les données relatifs aux noeuds. </summary>
	public StrucNodes[] nodes;

	///// <summary> Description de la structure contenant les données relatifs aux paramètres initiaux d'envol (Take off). </summary>
	//public struct StrucTakeOffParam
	//{
	//	public float verticalSpeed;
	//	public float anteroposteriorSpeed;
	//	public float somersaultSpeed;
	//	public float twistSpeed;
	//	public float tilt;
	//	public float rotation;
	//}

	/// <summary> Durée de la simulation en secondes. </summary>
	//public int duration;

	///// <summary> Structure contenant les données relatifs aux noeuds. </summary>
	//public StrucTakeOffParam takeOffParam;

	///// <summary> Condition utilisée. </summary>
	//public int condition;
	#endregion

	#region LagrangianModelParameters
	/// <summary> Description de la structure contenant les paramètres relatifs au modèle Lagrangien utilisé. </summary>
	public struct StrucLagrangianModel
	{
		public int nDDL;        // Nombre de DDL total.
		public int nTAG;
		public int nSOL;
		public int[] q1;        // DDL de la racine.
		public int[] q2;        // DDL à contrôler.
		public float dt;
		public string[] ddlName;
		public int[,] stickFigure;
		public int[,] filledFigure;
	}

	/// <summary> Structure contenant les paramètres relatifs au modèle Lagrangien utilisé. </summary>
	public StrucLagrangianModel lagrangianModel;
	#endregion

	#region singleton 
	// modèle singleton tiré du site : https://msdn.microsoft.com/en-us/library/ff650316.aspx
	private static MainParameters instance;

	// =================================================================================================================================================================

	private MainParameters()
	{
		init();
	}

	// =================================================================================================================================================================

	public static MainParameters Instance
	{
		get
		{
			if (instance == null) instance = new MainParameters();
			return instance;
		}
	}
	#endregion

	// =================================================================================================================================================================
	/// <summary> Initialisation des paramètres généraux et globaux. </summary>

	private void init()
	{
		// Initialisation de la structure contenant les données réelles des angles des articulations.

		jointsAngles = null;

		// Initialisation des structures Splines.

		pp = null;
		ppdot = null;
		ppddot = null;

		// Initialisation de la structure contenant les données relatifs aux noeuds.

		nodes = null;

		// Initialisation des paramètres reliés au modèle Lagrangien utilisé.

		lagrangianModel = new StrucLagrangianModel();
		lagrangianModel.nDDL = 0;
		lagrangianModel.nTAG = 0;
		lagrangianModel.nSOL = 0;
		lagrangianModel.q1 = new int[1] { 0 };
		lagrangianModel.q2 = new int[1] { 0 };
		lagrangianModel.dt = 0;
		lagrangianModel.ddlName = new string[1] { "" };
		lagrangianModel.stickFigure = null;
		lagrangianModel.filledFigure = null;
	}
}
