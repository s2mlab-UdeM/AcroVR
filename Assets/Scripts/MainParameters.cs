using UnityEngine;
using System.Collections.Generic;

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
		/// <summary> Pointeur, dans la structure Nodes, de l'articulation (ddl) du côté opposé (gauche <-> droit), si aucun côté = -1. </summary>
		public int ddlOppositeSide;
	}
	#endregion

	#region TakeOffParameters
	/// <summary> Description de la structure contenant les paramètres de décollage. </summary>
	public struct StrucTakeOffParam
	{
		public float verticalSpeed;					// en m/s
		public float anteroposteriorSpeed;			// en m/s
		public float somersaultSpeed;				// en rév/s
		public float twistSpeed;					// en rév/s
		public float tilt;							// en degrés
		public float rotation;						// en degrés
	}

    /// <summary> Structure contenant les valeurs de défaut pour les paramètres de décollage. </summary>
    public StrucTakeOffParam takeOffParamDefault;
    #endregion

	public enum DataType { Simulation};
	public enum LagrangianModelNames { Simple, Sasha23ddl};

    #region Joints
    /// <summary> Description de la structure contenant les données des angles des articulations (DDL). </summary>
    public struct StrucJoints
	{
		/// <summary> Nom du fichier de données utilisé. </summary>
		public string fileName;
		/// <summary> Structure contenant les données des noeuds. </summary>
		public StrucNodes[] nodes;
		/// <summary> Liste des temps utilisés par toutes les données interpolées. [m] = frames. </summary>
		public float[] t0;
		/// <summary> Liste de tous les angles interpolés pour chacune des articulations. [m,n]: m = DDL, n = Frames. </summary>
		public float[,] q0;
		/// <summary> Durée de la figure (en secondes). </summary>
		public float duration;
		/// <summary> Structure contenant les données relatifs aux paramètres initiaux d'envol. </summary>
		public StrucTakeOffParam takeOffParam;
		/// <summary>
		/// <para>Condition utilisée pour exécuter la figure. </para>
		/// (0 = Sans gravité, 1 = Trampoline, 2 = Chute, 3 = Plongeon 1m, 4 = Plongeon 3m, 5 = Plongeon 5m, 6 = Plongeon 10m, 7 = Barre fixe, 8 = Barres asymétriques, 9 = Saut à la perche).
		/// </summary>
		public int condition;
		/// <summary> Type de données utilisée. </summary>
		public DataType dataType;
		/// <summary> Nom du modèle Lagrangien utilisée. </summary>
		public LagrangianModelNames lagrangianModelName;
		/// <summary> Structure du modèle Lagrangien utilisée. </summary>
		public LagrangianModelManager.StrucLagrangianModel lagrangianModel;
		/// <summary> Temps avant contact avec le sol (en secondes). </summary>
		public float tc;
		/// <summary> Liste des temps utilisés par les données interpolées, jusqu'au contact avec le sol. [m] = frames. </summary>
		public float[] t;
		/// <summary> Liste des angles interpolés pour les articulations de rotation (périlleux, inclinaison et torsion), jusqu'au contact avec le sol. [m,n]: m = 3, n = Frames. </summary>
		public float[,] rot;
		/// <summary> Liste des vitesses des angles interpolés pour les articulations de rotation (périlleux, inclinaison et torsion), jusqu'au contact avec le sol. [m,n]: m = 3, n = Frames. </summary>
		public float[,] rotdot;
	}

	/// <summary> Structure contenant les données des angles des articulations (DDL). </summary>
	public StrucJoints joints;

    /// <summary> Valeur de défaut pour la durée de la figure. </summary>
    public float durationDefault;

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

	#region ScrollViewMessages
	///// <summary> Liste des messages qui seront affichés dans la boîte des messages. </summary>
	public List<string> scrollViewMessages;
	#endregion

	#region Languages
	/// <summary> Description de la structure contenant la liste des messages utilisés. </summary>
	public struct StrucMessageLists
	{
		public string movementDDLHipFlexion;
		public string movementDDLKneeFlexion;
		public string movementDDLLeftArmFlexion;
		public string movementDDLLeftArmAbduction;
		public string movementDDLRightArmFlexion;
		public string movementDDLRightArmAbduction;
		public string movementButtonAddNode;
		public string movementButtonRemoveNode;
		public string movementButtonCancelChanges;
		public string movementLoadDataFileTitle;
		public string movementLoadDataFileTxtFile;
		public string movementLoadDataFileAllFiles;
		public string movementSaveDataFileTitle;
		public string movementGraphSettingsVerticalTitle;
		public string movementGraphSettingsHorizontalTitle;
		public string movementGraphSettingsLowerBound;
		public string movementGraphSettingsUpperBound;
		public string movementGraphSettingsUpdateSimulation;
		public string movementGraphSettingsDefaultValuesButton;
		public string movementGraphSettingsCancelButton;

		public string takeOffTitle;
		public string takeOffTitleSpeed;
		public string takeOffConditionNoGravity;
		public string takeOffConditionTrampolining;
		public string takeOffConditionTumbling;
		public string takeOffConditionDiving1m;
		public string takeOffConditionDiving3m;
		public string takeOffConditionDiving5m;
		public string takeOffConditionDiving10m;
		public string takeOffConditionHighBar;
		public string takeOffConditionUnevenBars;
		public string takeOffConditionVault;
		public string takeOffSomersaultPosition;
		public string takeOffTilt;
		public string takeOffHorizontal;
		public string takeOffVertical;
		public string takeOffSomersaultSpeed;
		public string takeOffTwist;

		public string animatorPlayModeGesticulation;
		public string animatorPlayModeSimulation;
		public string animatorPlayViewFrontal;
		public string animatorPlayViewSagittal;
		public string animatorPlaySpeedFast;
		public string animatorPlaySpeedNormal;
		public string animatorPlaySpeedSlow1;
		public string animatorPlaySpeedSlow2;
		public string animatorPlaySpeedSlow3;

		public string errorMsgVerticalSpeed;
		public string errorMsgInvalidNodePosition;
		public string errorMsgNotEnoughNodes;
		public string errorMsgLowerBoundOverflow;
		public string errorMsgLowerBoundInvalid;
		public string errorMsgUpperBoundOverflow;
		public string errorMsgUpperBoundInvalid;

		public string displayMsgTitle;
		public string displayMsgStartSimulation;
		public string displayMsgDtValue;
		public string displayMsgSimulationTime;
		public string displayMsgContactGround;
		public string displayMsgNumberSomersaults;
		public string displayMsgNumberTwists;
		public string displayMsgFinalTwist;
		public string displayMsgMaxTilt;
		public string displayMsgFinalTilt;
		public string displayMsgSimulationDuration;
		public string displayMsgEndSimulation;
	}

	/// <summary> Description de la structure contenant la liste des messages utilisés en français et en anglais. </summary>
	public struct StrucLanguages
	{
		public StrucMessageLists Used;
		public StrucMessageLists french;
		public StrucMessageLists english;
	}

	/// <summary> Structure contenant la liste des messages utilisés en français et en anglais. </summary>
	public StrucLanguages languages;
	#endregion

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
		joints.t0 = null;
		joints.q0 = null;
		joints.duration = durationDefault;
		joints.takeOffParam = takeOffParamDefault;
		joints.condition = conditionDefault;
		joints.dataType = DataType.Simulation;
		joints.lagrangianModelName = LagrangianModelNames.Simple;
		joints.lagrangianModel = new LagrangianModelManager.StrucLagrangianModel();
		joints.tc = 0;
		joints.t = null;
		joints.rot = null;
		joints.rotdot = null;

		// Initialisation des paramètres reliés aux coefficents splines interpolés des données réelles des angles des articulations.

		//splines = new StrucSplines();
		//splines.coefs = null;

		// Initialisation de la liste des messages, utilisé pour la boîte des messages.

		scrollViewMessages = new List<string>();

		// Initialisation de la liste des messages en français et en anglais.

		languages.french.movementDDLHipFlexion = "Hanche_Flexion";
		languages.english.movementDDLHipFlexion = "Hip_Flexion";
		languages.french.movementDDLKneeFlexion = "Genou_Flexion";
		languages.english.movementDDLKneeFlexion = "Knee_Flexion";
		languages.french.movementDDLLeftArmFlexion = "Bras_Gauche_Flexion";
		languages.english.movementDDLLeftArmFlexion = "Left_Arm_Flexion";
		languages.french.movementDDLLeftArmAbduction = "Bras_Gauche_Abduction";
		languages.english.movementDDLLeftArmAbduction = "Left_Arm_Abduction";
		languages.french.movementDDLRightArmFlexion = "Bras_Droit_Flexion";
		languages.english.movementDDLRightArmFlexion = "Right_Arm_Flexion";
		languages.french.movementDDLRightArmAbduction = "Bras_Droit_Abduction";
		languages.english.movementDDLRightArmAbduction = "Right_Arm_Abduction";

		languages.french.movementButtonAddNode = "Ajouter un noeud";
		languages.english.movementButtonAddNode = "Add node";
		languages.french.movementButtonRemoveNode = "Effacer un noeud";
		languages.english.movementButtonRemoveNode = "Remove node";
		languages.french.movementButtonCancelChanges = "Annuler modif.";
		languages.english.movementButtonCancelChanges = "Cancel changes";
		languages.french.movementLoadDataFileTitle = "Ouvrir un Fichier de Simulation";
		languages.english.movementLoadDataFileTitle = "Open a Simulation File";
		languages.french.movementLoadDataFileTxtFile = "Fichiers txt";
		languages.english.movementLoadDataFileTxtFile = "Txt files";
		languages.french.movementLoadDataFileAllFiles = "Tous les fichiers";
		languages.english.movementLoadDataFileAllFiles = "All files";
		languages.french.movementSaveDataFileTitle = "Créer/modifier un Fichier de Simulation";
		languages.english.movementSaveDataFileTitle = "Create/modify a Simulation File";

		languages.french.movementGraphSettingsVerticalTitle = "Axe vertical";
		languages.english.movementGraphSettingsVerticalTitle = "Vertical axis";
		languages.french.movementGraphSettingsHorizontalTitle = "Axe horizontal";
		languages.english.movementGraphSettingsHorizontalTitle = "Horizontal axis";
		languages.french.movementGraphSettingsLowerBound = string.Format("Borne{0}inférieur", System.Environment.NewLine);
		languages.english.movementGraphSettingsLowerBound = string.Format("Lower{0}bound", System.Environment.NewLine);
		languages.french.movementGraphSettingsUpperBound = string.Format("Borne{0}supérieur", System.Environment.NewLine);
		languages.english.movementGraphSettingsUpperBound = string.Format("Upper{0}bound", System.Environment.NewLine);
		languages.french.movementGraphSettingsUpdateSimulation = "Mise à jour simulation";
		languages.english.movementGraphSettingsUpdateSimulation = "Update simulation";
		languages.french.movementGraphSettingsDefaultValuesButton = string.Format("Valeurs{0}de défaut", System.Environment.NewLine);
		languages.english.movementGraphSettingsDefaultValuesButton = string.Format("Default{0}values", System.Environment.NewLine);
		languages.french.movementGraphSettingsCancelButton = "Annuler";
		languages.english.movementGraphSettingsCancelButton = "Cancel";

		languages.french.takeOffTitle = "Paramètres de décollage:";
		languages.english.takeOffTitle = "Take-off parameters:";
		languages.french.takeOffTitleSpeed = "Vitesses";
		languages.english.takeOffTitleSpeed = "Speeds";
		languages.french.takeOffConditionNoGravity = "Sans gravité";
		languages.english.takeOffConditionNoGravity = "No gravity";
		languages.french.takeOffConditionTrampolining = "Trampoline";
		languages.english.takeOffConditionTrampolining = "Trampolining";
		languages.french.takeOffConditionTumbling = "Chute";
		languages.english.takeOffConditionTumbling = "Tumbling";
		languages.french.takeOffConditionDiving1m = "Plongeon 1 m";
		languages.english.takeOffConditionDiving1m = "Diving 1 m";
		languages.french.takeOffConditionDiving3m = "Plongeon 3 m";
		languages.english.takeOffConditionDiving3m = "Diving 3 m";
		languages.french.takeOffConditionDiving5m = "Plongeon 5 m";
		languages.english.takeOffConditionDiving5m = "Diving 5 m";
		languages.french.takeOffConditionDiving10m = "Plongeon 10 m";
		languages.english.takeOffConditionDiving10m = "Diving 10 m";
		languages.french.takeOffConditionHighBar = "Barre fixe";
		languages.english.takeOffConditionHighBar = "High bar";
		languages.french.takeOffConditionUnevenBars = "Barres asymétriques";
		languages.english.takeOffConditionUnevenBars = "Uneven Bars";
		languages.french.takeOffConditionVault = "Saut à la perche";
		languages.english.takeOffConditionVault = "Vault";
		languages.french.takeOffSomersaultPosition = "Salto (°)";
		languages.english.takeOffSomersaultPosition = "Somersault (°)";
		languages.french.takeOffTilt = "Inclinaison (°)";
		languages.english.takeOffTilt = "Tilt (°)";
		languages.french.takeOffHorizontal = "Horizontale (m/s)";
		languages.english.takeOffHorizontal = "Horizontal (m/s)";
		languages.french.takeOffVertical = "Verticale (m/s)";
		languages.english.takeOffVertical = "Vertical (m/s)";
		languages.french.takeOffSomersaultSpeed = "Salto (rév./s)";
		languages.english.takeOffSomersaultSpeed = "Somersault (rev./s)";
		languages.french.takeOffTwist = "Vrille (rév./s)";
		languages.english.takeOffTwist = "Twist (rev./s)";

		languages.french.animatorPlayModeGesticulation = languages.english.animatorPlayModeGesticulation = "Gesticulation";
		languages.french.animatorPlayModeSimulation = languages.english.animatorPlayModeSimulation = "Simulation";
		languages.french.animatorPlayViewFrontal = languages.english.animatorPlayViewFrontal = "Frontal";
		languages.french.animatorPlayViewSagittal = languages.english.animatorPlayViewSagittal = "Sagittal";
		languages.french.animatorPlaySpeedFast = "Vit. rapide";
		languages.english.animatorPlaySpeedFast = "Fast speed";
		languages.french.animatorPlaySpeedNormal = "Vit. normale";
		languages.english.animatorPlaySpeedNormal = "Normal speed";
		languages.french.animatorPlaySpeedSlow1 = "Vit. lente 1";
		languages.english.animatorPlaySpeedSlow1 = "Slow speed 1";
		languages.french.animatorPlaySpeedSlow2 = "Vit. lente 2";
		languages.english.animatorPlaySpeedSlow2 = "Slow speed 2";
		languages.french.animatorPlaySpeedSlow3 = "Vit. lente 3";
		languages.english.animatorPlaySpeedSlow3 = "Slow speed 3";

		languages.french.errorMsgVerticalSpeed = string.Format("Valeur du paramètre Vitesse verticale {0}	  doit être égal ou supérieur à 0", System.Environment.NewLine);
		languages.english.errorMsgVerticalSpeed = string.Format("Value of the vertical speed parameter {0} must be equal to or greater than 0", System.Environment.NewLine);
		languages.french.errorMsgInvalidNodePosition = string.Format("Le noeud ne peut pas être déplacer avant/après {0} le noeud précédent/suivant (selon l'échelle des temps)", System.Environment.NewLine);
		languages.english.errorMsgInvalidNodePosition = string.Format("Node cannot be place before/after {0} the previous/next node (following the time scale)", System.Environment.NewLine);
		languages.french.errorMsgNotEnoughNodes = string.Format("Au moins 2 noeuds doit être défini,{0}donc la suppression ignoré", System.Environment.NewLine);
		languages.english.errorMsgNotEnoughNodes = string.Format("At least 2 nodes must exist,{0}so node can't be removed", System.Environment.NewLine);
		languages.french.errorMsgLowerBoundOverflow = string.Format("La borne inférieur ne peut pas être plus grande ou égale à la borne supérieur");
		languages.english.errorMsgLowerBoundOverflow = string.Format("Lower bound cannot be greater or equal to the upper bound");
		languages.french.errorMsgLowerBoundInvalid = string.Format("La valeur de la borne inférieur spécifié est invalide");
		languages.english.errorMsgLowerBoundInvalid = string.Format("Specified value for the lower bound is invalid");
		languages.french.errorMsgUpperBoundOverflow = string.Format("La borne supérieur ne peut pas être plus petite ou égale à la borne inférieur");
		languages.english.errorMsgUpperBoundOverflow = string.Format("Upper bound cannot be smaller or equal to the lower bound");
		languages.french.errorMsgUpperBoundInvalid = string.Format("La valeur de la borne supérieur spécifié est invalide");
		languages.english.errorMsgUpperBoundInvalid = string.Format("Specified value for the upper bound is invalid");

		languages.french.displayMsgTitle = "Résultats:";
		languages.english.displayMsgTitle = "Results:";
		languages.french.displayMsgStartSimulation = "Visualisation démarrée (Simulation)";
		languages.english.displayMsgStartSimulation = "Visualisation started (Simulation)";
		languages.french.displayMsgDtValue = "Paramètre dt (durée d'un frame)";
		languages.english.displayMsgDtValue = "Parameter dt (frame duration)";
		languages.french.displayMsgSimulationTime = "Temps de la simulation";
		languages.english.displayMsgSimulationTime = "Simulation time";
		languages.french.displayMsgContactGround = "!! ATTENTION: Contact avec le sol à";
		languages.english.displayMsgContactGround = "!! WARNING: Contact with the ground at";
		languages.french.displayMsgNumberSomersaults = "Nombre de salto(s)";
		languages.english.displayMsgNumberSomersaults = "Number of Somersault(s)";
		languages.french.displayMsgNumberTwists = "Nombre de vrille(s)";
		languages.english.displayMsgNumberTwists = "Number of Twist(s)";
		languages.french.displayMsgFinalTwist = "Vrille final (valeur de fin)";
		languages.english.displayMsgFinalTwist = "Final twist (end value)";
		languages.french.displayMsgMaxTilt = "Inclinaison maximale";
		languages.english.displayMsgMaxTilt = "Tilt max";
		languages.french.displayMsgFinalTilt = "Inclinaison finale (valeur de fin)";
		languages.english.displayMsgFinalTilt = "Final tilt (end value)";
		languages.french.displayMsgSimulationDuration = "Durée réelle de la simulation";
		languages.english.displayMsgSimulationDuration = "Simulation real duration";
		languages.french.displayMsgEndSimulation = "Simulation terminée";
		languages.english.displayMsgEndSimulation = "Simulation completed";

		languages.Used = languages.french;
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
