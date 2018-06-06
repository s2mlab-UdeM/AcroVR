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
		public float[] Qdot;
	}

	/// <summary> Structure contenant les données réelles des angles des articulations (DDL). </summary>
	public StrucJointsAngles[] jointsAngles;
	#endregion

	#region SplinesParameters
	/// <summary> Description de la structure contenant les coefficents splines interpolés des données réelles des angles des articulations (DDL). </summary>
	// Interpolation de type spline cubique
	public struct StrucCoefSplines
	{
		public float[,] pp;					// [m,n] --> m = nombre de frames, n = nombre de coefficents
		public float[,] ppdot;              // [m,n] --> m = nombre de frames, n = nombre de coefficents
		public float[,] ppddot;             // [m,n] --> m = nombre de frames, n = nombre de coefficents
	}

	/// <summary> Description de la structure contenant les données relatifs aux splines interpolés des données réelles des angles des articulations (DDL). </summary>
	public struct StrucSplines
	{
		public float[] T;                   // [n] --> n = nombre de frames
		public StrucCoefSplines[] coefs;    // [n] --> n = nombre de DDL
	}

	/// <summary> Structure contenant les coefficents splines interpolés des données réelles des angles des articulations (DDL). </summary>
	public StrucSplines splines;
	#endregion

	#region TakeOffParameters
	/// <summary> Description de la structure contenant les données relatifs aux paramètres initiaux d'envol. </summary>
	public struct StrucTakeOffParam
	{
		public float verticalSpeed;
		public float anteroposteriorSpeed;
		public float somersaultSpeed;
		public float twistSpeed;
		public float tilt;
		public float rotation;
	}

	/// <summary> Structure contenant les données relatifs aux paramètres initiaux d'envol. </summary>
	public StrucTakeOffParam takeOffParam;
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

		// Initialisation des paramètres reliés aux coefficents splines interpolés des données réelles des angles des articulations.

		splines = new StrucSplines();
		splines.coefs = null;

		// Initialisation des paramètres initiaux d'envol.

		takeOffParam.verticalSpeed = 0;
		takeOffParam.anteroposteriorSpeed = 0;
		takeOffParam.somersaultSpeed = 0;
		takeOffParam.twistSpeed = 0;
		takeOffParam.tilt = 0;
		takeOffParam.rotation = 0;

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
