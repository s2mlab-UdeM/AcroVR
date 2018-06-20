using UnityEngine;
//using System.Collections;

// =================================================================================================================================================================
/// <summary>
/// Cette classe contient les paramètres généraux et globaux utilisés dans le logiciel.
/// N'hérite pas de monobehavior et est un singleton.
/// </summary>

public class MainParameters
{
	#region Interpolation
	public enum InterpolationType { Quintic, CubicSpline };

	/// <summary> Description de la structure contenant les informations sur le type d'interpolation utilisé (quintic ou spline cubique). </summary>
	public struct StrucInterpolation
	{
		public InterpolationType type;
		public int numIntervals;
	}

	/// <summary> Structure contenant les informations sur le type d'interpolation utilisé par défaut. </summary>
	public StrucInterpolation interpolationDefault;
	#endregion

	#region Nodes
	/// <summary> Description de la structure contenant les données des noeuds. </summary>
	public struct StrucNodes
	{
		public int ddl;
		public string name;
		public float[] T;
		public float[] Q;
		public StrucInterpolation interpolation;
	}
	#endregion

	#region TakeOffParameters
	/// <summary> Description de la structure contenant les paramètres de décollage. </summary>
	public struct StrucTakeOffParam
	{
		public float verticalSpeed;
		public float anteroposteriorSpeed;
		public float somersaultSpeed;
		public float twistSpeed;
		public float tilt;
		public float rotation;
	}

    /// <summary> Structure contenant les valeurs de défaut pour les paramètres de décollage. </summary>
    public StrucTakeOffParam takeOffParamDefault;
    #endregion

	public enum DataType { Simulation};
	public enum LagrangianModel { Simple, Sasha23ddl};

    #region Joints
    /// <summary> Description de la structure contenant les données des angles des articulations (DDL). </summary>
    public struct StrucJoints
	{
		/// <summary> Nom du fichier de données utilisé. </summary>
		public string fileName;
		/// <summary> Structure contenant les données des noeuds. </summary>
		public StrucNodes[] nodes;
		/// <summary> Durée de la figure. </summary>
		public int duration;
		/// <summary> Structure contenant les données relatifs aux paramètres initiaux d'envol. </summary>
		public StrucTakeOffParam takeOffParam;
		/// <summary> Condition utilisée pour exécuter la figure. </summary>
		public int condition;
		/// <summary> Type de données utilisée. </summary>
		public DataType dataType;
		/// <summary> Modèle Lagrangien utilisée. </summary>
		public LagrangianModel lagrangianModel;
	}

	/// <summary> Structure contenant les données des angles des articulations (DDL). </summary>
	public StrucJoints joints;

    /// <summary> Valeur de défaut pour la durée de la figure. </summary>
    public int durationDefault;

    /// <summary> Valeur de défaut pour la condition utilisée pour exécuter la figure. </summary>
    public int conditionDefault;
    #endregion

    #region SplinesParameters
    ///// <summary> Description de la structure contenant les coefficents splines interpolés des données réelles des angles des articulations (DDL). </summary>
    //// Interpolation de type spline cubique
    //public struct StrucCoefSplines
    //{
    //	public float[,] pp;					// [m,n] --> m = nombre de frames, n = nombre de coefficents
    //	public float[,] ppdot;              // [m,n] --> m = nombre de frames, n = nombre de coefficents
    //	public float[,] ppddot;             // [m,n] --> m = nombre de frames, n = nombre de coefficents
    //}

    ///// <summary> Description de la structure contenant les données relatifs aux splines interpolés des données réelles des angles des articulations (DDL). </summary>
    //public struct StrucSplines
    //{
    //	public float[] T;                   // [n] --> n = nombre de frames
    //	public StrucCoefSplines[] coefs;    // [n] --> n = nombre de DDL
    //}

    ///// <summary> Structure contenant les coefficents splines interpolés des données réelles des angles des articulations (DDL). </summary>
    //public StrucSplines splines;
    #endregion

	/// <summary> Nombre de frames contenu dans la structure jointsAngles. </summary>
	//public int numberOfFrames;

	#region singleton 
	// modèle singleton tiré du site : https://msdn.microsoft.com/en-us/library/ff650316.aspx
	private static MainParameters instance;

	// =================================================================================================================================================================

	private MainParameters()
	{
		// Initialisation des paramètres à leurs valeurs de défaut.

		interpolationDefault.type = InterpolationType.Quintic;
		interpolationDefault.numIntervals = 0;
        takeOffParamDefault.verticalSpeed = 0;
        takeOffParamDefault.anteroposteriorSpeed = 0;
        takeOffParamDefault.somersaultSpeed = 0;
        takeOffParamDefault.twistSpeed = 0;
        takeOffParamDefault.tilt = 0;
        takeOffParamDefault.rotation = 0;
		durationDefault = 0;
		conditionDefault = 0;

		// Initialisation des paramètres reliés aux données des angles des articulations.

		joints.fileName = "";
		joints.nodes = null;
		joints.duration = durationDefault;
		joints.takeOffParam = takeOffParamDefault;
		joints.condition = conditionDefault;
		joints.dataType = DataType.Simulation;
		joints.lagrangianModel = LagrangianModel.Simple;

		// Initialisation des paramètres reliés aux coefficents splines interpolés des données réelles des angles des articulations.

		//splines = new StrucSplines();
		//splines.coefs = null;

		// Initialisation d'autres paramètres

		//numberOfFrames = 0;
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
}
