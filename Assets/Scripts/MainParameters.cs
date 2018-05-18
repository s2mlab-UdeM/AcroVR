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
	/// <summary> Description de la structure contenant les données réelles des angles des articulations (DDL). </summary>
	public struct StrucJointsAngles
	{
		public float T;
		public float[] Q;
	}

	/// <summary> Structure contenant les données réelles des angles des articulations (DDL). </summary>
	public StrucJointsAngles[] jointsAngles;
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

	/// <summary> Nombre de frames contenu dans la structure jointsAngles. </summary>
	public int numberOfFrames;

	public enum ListDisplayType {Statique, Dynamique};
	/// <summary> Type d'affichage de la silhouette (Statique ou Dynamique). </summary>
	public ListDisplayType displayType;

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
		// Initialisation des paramètres reliés aux données réelles des angles des articulations.

		jointsAngles = null;

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

		// Initialisation d'autres paramètres

		numberOfFrames = 0;
		displayType = ListDisplayType.Dynamique;
	}
}
