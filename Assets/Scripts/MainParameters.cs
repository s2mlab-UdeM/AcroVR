using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
		/// <summary> Type d'interpolation utilisé (quintic ou spline cubique). </summary>
		public InterpolationType type;
		/// <summary> Nombre d'intervalle utilisé (spline cubique seulement). </summary>
		public int numIntervals;
		/// <summary> Pente initiale et finale (spline cubique seulement). </summary>
		public float[] slope;
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

	#region ScrollViewMessages
	///// <summary> Liste des messages qui seront affichés dans la boîte des messages. </summary>
	public List<string> scrollViewMessages;
	#endregion

	#region Languages
	/// <summary> Description de la structure contenant la liste des messages utilisés. </summary>
	public struct StrucMessageLists
	{
		public string leftSide;
		public string rightSide;

		public string movementDDLHipFlexion;
		public string movementDDLKneeFlexion;
		public string movementDDLLeftArmFlexion;
		public string movementDDLLeftArmAbduction;
		public string movementDDLRightArmFlexion;
		public string movementDDLRightArmAbduction;
		public string movementInterpolationCubicSpline;
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
		public string takeOffInitialPosture;
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
		public string animatorMsgGroundContact;

		public string resultsGraphicsSelectionRotationsVsTime;
		public string resultsGraphicsSelectionTiltVsTime;
		public string resultsGraphicsSelectionTiltVsSomersault;
		public string resultsGraphicsSelectionTiltVsTwist;
		public string resultsGraphicsSelectionTwistVsSomersault;
		public string resultsGraphicsSelectionAngularSpeedVsTime;
		public string resultsGraphicsLabelAxisTime;
		public string resultsGraphicsLabelAxisRotation;
		public string resultsGraphicsLabelAxisTilt;
		public string resultsGraphicsLabelAxisSomersault;
		public string resultsGraphicsLabelAxisTwist;
		public string resultsGraphicsLabelAxisAngularSpeed;
		public string resultsGraphicsLegendCurveNameSomersault;
		public string resultsGraphicsLegendCurveNameTilt;
		public string resultsGraphicsLegendCurveNameTwist;

		public string errorMsgSomersaultPosition;
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

		public string toolTipButtonToolTips;
		public string toolTipButtonLanguage;
		public string toolTipButtonQuit;
		public string toolTipPanelGraph;
		public string toolTipButtonAddNode;
		public string toolTipButtonRemoveNode;
		public string toolTipButtonCancelChanges;
		public string toolTipDropDownDDLNames;
		public string toolTipDropDownInterpolation;
		public string toolTipButtonLoad;
		public string toolTipButtonSave;
		public string toolTipButtonSymetricLeftRight;
		public string toolTipButtonGraphSettings;
		public string toolTipDropDownPlayMode;
		public string toolTipDropDownPlayView;
		public string toolTipDropDownPlaySpeed;
		public string toolTipButtonPlay;
		public string toolTipButtonStop;
		public string toolTipButtonGraph;
		public string toolTipTakeOffCondition;
		public string toolTipTakeOffInitialPosture;
		public string toolTipTakeOffSomersaultPosition;
		public string toolTipTakeOffTilt;
		public string toolTipTakeOffHorizontal;
		public string toolTipTakeOffVertical;
		public string toolTipTakeOffSomersaultSpeed;
		public string toolTipTakeOffTwist;
		public string toolTipGraphSettingsVerticalLowerBound;
		public string toolTipGraphSettingsVerticalUpperBound;
		public string toolTipGraphSettingsVerticalSlider;
		public string toolTipGraphSettingsVerticalLowerMinus10;
		public string toolTipGraphSettingsVerticalLowerPlus10;
		public string toolTipGraphSettingsVerticalUpperMinus10;
		public string toolTipGraphSettingsVerticalUpperPlus10;
		public string toolTipGraphSettingsHorizontalUpperBound;
		public string toolTipGraphSettingsHorizontalSlider;
		public string toolTipGraphSettingsUpdateSimulation;
		public string toolTipGraphSettingsDefaultValues;
		public string toolTipGraphSettingsCancel;
		public string toolTipGraphSettingsOK;
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

	#region BioRBD
	//Librairie et fonctions biorbd
#if UNITY_EDITOR
	const string dllpath = @"Assets\StreamingAssets\biorbd_c.dll";
#else
#if UNITY_STANDALONE_OSX
	const string dllpath = @"AcroVR/Contents/Resources/Data/StreamingAssets/libbiorbd.dylib";	// Fonctionne pas
	//static System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(dllpath);
	//string fileInfo = info.FullName;
#else
	const string dllpath = @"..\StreamingAssets\biorbd_c.dll";
#endif
#endif
	[DllImport(dllpath)] public static extern IntPtr c_biorbdModel(StringBuilder pathToModel);
	[DllImport(dllpath)] public static extern int c_nQ(IntPtr model);
	[DllImport(dllpath)] public static extern int c_nQDot(IntPtr model);
	[DllImport(dllpath)] public static extern void c_inverseDynamics(IntPtr model, IntPtr q, IntPtr qdot, IntPtr qddot, IntPtr tau);
	[DllImport(dllpath)] public static extern void c_massMatrix(IntPtr model, IntPtr q, IntPtr massMatrix);
	[DllImport(dllpath)] public static extern void c_markers(IntPtr model, IntPtr q, IntPtr markPos, bool removeAxis, bool updateKin);
	[DllImport(dllpath)] public static extern int c_nMarkers(IntPtr model);
	[DllImport(dllpath)] public static extern void c_solveLinearSystem(IntPtr matA, int nbCol, int nbLigne, IntPtr matB, IntPtr solX);

	/// <summary> Pointeur qui désigne le modèle BioRBD utilisé. </summary>
	public IntPtr ptr_model;
	#endregion

	/// <summary> Numéros des types de graphique des résultats qui seront affiché dans le panneau des graphiques des résultats. </summary>
	public int[] resultsGraphicsUsed;
	public bool testDataFileDone = false;

	#region singleton 
	// modèle singleton tiré du site : https://msdn.microsoft.com/en-us/library/ff650316.aspx
	private static MainParameters instance;

	// =================================================================================================================================================================

	private MainParameters()
	{
		#region InitParameters
		// Initialisation des paramètres à leurs valeurs de défaut.

		interpolationDefault.type = InterpolationType.Quintic;
		interpolationDefault.numIntervals = 0;
		interpolationDefault.slope = new float[] { 0, 0 };
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

		// Initialisation de la liste des messages, utilisé pour la boîte des messages.

		scrollViewMessages = new List<string>();

		// Initialisation des numéros des types de graphique des résultats qui seront affiché

		resultsGraphicsUsed = new int[2] { 0, 5 };
		#endregion

		#region InitLanguages
		// Initialisation de la liste des messages en français et en anglais.

		languages.french.leftSide = "Gauche";
		languages.english.leftSide = "Left";
		languages.french.rightSide = "Droit";
		languages.english.rightSide = "Right";

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
		languages.french.movementInterpolationCubicSpline = "Spline cub.";
		languages.english.movementInterpolationCubicSpline = "Cubic spl.";

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
		languages.french.takeOffInitialPosture = "Posture initiale:";
		languages.english.takeOffInitialPosture = "Initial posture:";
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
		languages.french.animatorMsgGroundContact = "Note: Contact avec le sol très rapide";
		languages.english.animatorMsgGroundContact = "Note: Contact with the ground very fast";

		languages.french.resultsGraphicsSelectionRotationsVsTime = "Rotations vs temps";
		languages.english.resultsGraphicsSelectionRotationsVsTime = "Rotations vs time";
		languages.french.resultsGraphicsSelectionTiltVsTime = "Inclinaison vs temps";
		languages.english.resultsGraphicsSelectionTiltVsTime = "Tilt vs time";
		languages.french.resultsGraphicsSelectionTiltVsSomersault = "Inclinaison vs salto";
		languages.english.resultsGraphicsSelectionTiltVsSomersault = "Tilt vs somersault";
		languages.french.resultsGraphicsSelectionTiltVsTwist = "Inclinaison vs vrille";
		languages.english.resultsGraphicsSelectionTiltVsTwist = "Tilt vs twist";
		languages.french.resultsGraphicsSelectionTwistVsSomersault = "Vrille vs salto";
		languages.english.resultsGraphicsSelectionTwistVsSomersault = "Twist vs somersault";
		languages.french.resultsGraphicsSelectionAngularSpeedVsTime = "Vitesse angulaire vs temps";
		languages.english.resultsGraphicsSelectionAngularSpeedVsTime = "Angular speed vs time";
		languages.french.resultsGraphicsLabelAxisTime = "Temps (s)";
		languages.english.resultsGraphicsLabelAxisTime = "Time (s)";
		languages.french.resultsGraphicsLabelAxisRotation = "Rotation (rév.)";
		languages.english.resultsGraphicsLabelAxisRotation = "Rotation (rev.)";
		languages.french.resultsGraphicsLabelAxisTilt = "Inclinaison (rév.)";
		languages.english.resultsGraphicsLabelAxisTilt = "Tilt (rev.)";
		languages.french.resultsGraphicsLabelAxisSomersault = "Salto (rév.)";
		languages.english.resultsGraphicsLabelAxisSomersault = "Somersault (rev.)";
		languages.french.resultsGraphicsLabelAxisTwist = "Vrille (rév.)";
		languages.english.resultsGraphicsLabelAxisTwist = "Twist (rev.)";
		languages.french.resultsGraphicsLabelAxisAngularSpeed = "Vitesse angulaire (rév./s)";
		languages.english.resultsGraphicsLabelAxisAngularSpeed = "Angular speed (rev./s)";
		languages.french.resultsGraphicsLegendCurveNameSomersault = "Salto";
		languages.english.resultsGraphicsLegendCurveNameSomersault = "Somersault";
		languages.french.resultsGraphicsLegendCurveNameTilt = "Inclinaison";
		languages.english.resultsGraphicsLegendCurveNameTilt = "Tilt";
		languages.french.resultsGraphicsLegendCurveNameTwist = "Vrille";
		languages.english.resultsGraphicsLegendCurveNameTwist = "Twist";

		languages.french.errorMsgSomersaultPosition = string.Format("Valeur du paramètre position du salto{0}  doit être entre -180° et 180°", System.Environment.NewLine);
		languages.english.errorMsgSomersaultPosition = string.Format("Value of the somersault position parameter{0} must be between -180° and 180°", System.Environment.NewLine);
		languages.french.errorMsgVerticalSpeed = string.Format("Valeur du paramètre vitesse verticale {0}	  doit être égal ou supérieur à 0", System.Environment.NewLine);
		languages.english.errorMsgVerticalSpeed = string.Format("Value of the vertical speed parameter {0} must be equal to or greater than 0", System.Environment.NewLine);
		languages.french.errorMsgInvalidNodePosition = string.Format("Le noeud ne peut pas être déplacer avant/après {0} le noeud précédent/suivant (selon l'échelle des temps)", System.Environment.NewLine);
		languages.english.errorMsgInvalidNodePosition = string.Format("Node cannot be place before/after {0} the previous/next node (following the time scale)", System.Environment.NewLine);
		languages.french.errorMsgNotEnoughNodes = string.Format("Au moins 2 noeuds doit être défini,{0}donc la suppression est ignoré", System.Environment.NewLine);
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
		#endregion

		#region InitLanguagesToolTips
		// Aide contextuelle

		languages.french.toolTipButtonToolTips = "Afficher une aide contextuelle, selon le mouvement de la souris";
		languages.english.toolTipButtonToolTips = "Display tool tips, following mouse movement";
		languages.french.toolTipButtonLanguage = "Switch all texts in english";
		languages.english.toolTipButtonLanguage = "Change tous les textes en français";
		languages.french.toolTipButtonQuit = "Quitter le logiciel";
		languages.english.toolTipButtonQuit = "Quit";

		languages.french.toolTipPanelGraph = string.Format("Déplacer/Ajouter/Effacer un noeud,{0}    modifier pente initiale/finale{0}    ou réinitialiser le graphique{0} avec un des boutons de la souris", System.Environment.NewLine);
		languages.english.toolTipPanelGraph = string.Format("     Move/Add/Remove node,{0}      modify initial/final slope{0}           or reset graphic{0}with the use of a mouse buttons", System.Environment.NewLine);
		languages.french.toolTipButtonAddNode = "Ajouter un noeud à la position de la souris";
		languages.english.toolTipButtonAddNode = "Add node at the mouse position";
		languages.french.toolTipButtonRemoveNode = "Effacer le noeud qui est le plus près de la position de la souris";
		languages.english.toolTipButtonRemoveNode = "Remove closest node of the mouse position";
		languages.french.toolTipButtonCancelChanges = "Annuler toutes les modifications depuis le dernier chargement/conservation d'un fichier";
		languages.english.toolTipButtonCancelChanges = "Cancel all changes since last file load/save";
		languages.french.toolTipDropDownDDLNames = "Sélectionner une articulation à afficher";
		languages.english.toolTipDropDownDDLNames = "Select a DDL to display";
		languages.french.toolTipDropDownInterpolation = "Sélectionner un type d'interpolation";
		languages.english.toolTipDropDownInterpolation = "Select an interpolation type";
		languages.french.toolTipButtonLoad = "Charger un nouveau fichier de simulation";
		languages.english.toolTipButtonLoad = "Load a new simulation file";
		languages.french.toolTipButtonSave = "Conserver dans un fichier";
		languages.english.toolTipButtonSave = "Save in a file";
		languages.french.toolTipButtonSymetricLeftRight = string.Format("Déplacer les articulations gauche & droite{0}de façon symétrique ou asymétrique", System.Environment.NewLine);
		languages.english.toolTipButtonSymetricLeftRight = string.Format("Move DDL left & right{0}in symetric or asymetric manner", System.Environment.NewLine);
		languages.french.toolTipButtonGraphSettings = "Modifier les échelles du graphique";
		languages.english.toolTipButtonGraphSettings = "Modify the graphic scales";

		languages.french.toolTipTakeOffCondition = "Sélectionner une condition d'envol";
		languages.english.toolTipTakeOffCondition = "Select flight condition";
		languages.french.toolTipTakeOffInitialPosture = "Sélectionner la posture initial, si dans mode temps réel";
		languages.english.toolTipTakeOffInitialPosture = "Select initial posture, if in real time mode";
		languages.french.toolTipTakeOffSomersaultPosition = "Sélectionner la position de salto initiale";
		languages.english.toolTipTakeOffSomersaultPosition = "Select somersault initial position";
		languages.french.toolTipTakeOffTilt = "Sélectionner la position d'inclinaison initiale";
		languages.english.toolTipTakeOffTilt = "Select tilt initial position";
		languages.french.toolTipTakeOffHorizontal = "Sélectionner la vitesse de déplacement horizontal";
		languages.english.toolTipTakeOffHorizontal = "Select horizontal movement speed";
		languages.french.toolTipTakeOffVertical = "Sélectionner la vitesse de déplacement vertical";
		languages.english.toolTipTakeOffVertical = "Select vertical movement speed";
		languages.french.toolTipTakeOffSomersaultSpeed = "Sélectionner la vitesse de rotation des saltos";
		languages.english.toolTipTakeOffSomersaultSpeed = "Select somersault rotation speed";
		languages.french.toolTipTakeOffTwist = "Sélectionner la vitesse de rotation des vrilles";
		languages.english.toolTipTakeOffTwist = "Select twist rotation speed";

		languages.french.toolTipGraphSettingsVerticalLowerBound = "Entrer une valeur d'angle inférieur à la borne supérieur";
		languages.english.toolTipGraphSettingsVerticalLowerBound = "Enter an angle value smaller than the upper bound";
		languages.french.toolTipGraphSettingsVerticalUpperBound = "Entrer une valeur d'angle supérieur à la borne inférieur";
		languages.english.toolTipGraphSettingsVerticalUpperBound = "Enter an angle value greater than the lower bound";
		languages.french.toolTipGraphSettingsVerticalSlider = "Déplacer l'un des deux boutons pour modifier les bornes de l'axe vertical";
		languages.english.toolTipGraphSettingsVerticalSlider = "Move one of the two knobs to modify the bounds of the vertical axis";
		languages.french.toolTipGraphSettingsVerticalLowerMinus10 = "Diminuer la borne inférieur de 10°";
		languages.english.toolTipGraphSettingsVerticalLowerMinus10 = "Decrease the lower bound by 10°";
		languages.french.toolTipGraphSettingsVerticalLowerPlus10 = "Augmenter la borne inférieur de 10°";
		languages.english.toolTipGraphSettingsVerticalLowerPlus10 = "Increase the lower bound by 10°";
		languages.french.toolTipGraphSettingsVerticalUpperMinus10 = "Diminuer la borne supérieur de 10°";
		languages.english.toolTipGraphSettingsVerticalUpperMinus10 = "Decrease the upper bound by 10°";
		languages.french.toolTipGraphSettingsVerticalUpperPlus10 = "Augmenter la borne supérieur de 10°";
		languages.english.toolTipGraphSettingsVerticalUpperPlus10 = "Increase the upper bound by 10°";
		languages.french.toolTipGraphSettingsHorizontalUpperBound = "Entrer une valeur d'angle supérieur à la borne inférieur";
		languages.english.toolTipGraphSettingsHorizontalUpperBound = "Enter an angle value greater than the lower bound";
		languages.french.toolTipGraphSettingsHorizontalSlider = "Déplacer le bouton pour modifier la borne supérieur de l'axe horizontal";
		languages.english.toolTipGraphSettingsHorizontalSlider = "Move the knob to modify the upper bound of the horizontal axis";
		languages.french.toolTipGraphSettingsUpdateSimulation = "Mettre à jour l'axe horizontal (temps) pour fixer le maximum à la borne supérieur";
		languages.english.toolTipGraphSettingsUpdateSimulation = "Update horizontal axe (time) to fix the maximum at the upper bound";
		languages.french.toolTipGraphSettingsDefaultValues = "Remettre toutes les bornes à leurs valeurs de défaut";
		languages.english.toolTipGraphSettingsDefaultValues = "Reset all bounds to theirs defaults values";
		languages.french.toolTipGraphSettingsCancel = "Quitter le menu sans utiliser les nouvelles bornes";
		languages.english.toolTipGraphSettingsCancel = "Quit menu without using the new bounds";
		languages.french.toolTipGraphSettingsOK = "Quitter le menu et utiliser les nouvelles bornes";
		languages.english.toolTipGraphSettingsOK = "Quit menu and use the new bounds";

		languages.french.toolTipDropDownPlayMode = "Sélectionner le type d'animation";
		languages.english.toolTipDropDownPlayMode = "Select animation playing mode";
		languages.french.toolTipDropDownPlayView = "Sélectionner la vue de l'animation";
		languages.english.toolTipDropDownPlayView = "Select animation view";
		languages.french.toolTipDropDownPlaySpeed = "Sélectionner la vitesse d'exécution de l'animation";
		languages.english.toolTipDropDownPlaySpeed = "Select animation playing speed";
		languages.french.toolTipButtonPlay = "Démarrer l'animation";
		languages.english.toolTipButtonPlay = "Start animation";
		languages.french.toolTipButtonStop = "Arrêter l'animation";
		languages.english.toolTipButtonStop = "Stop animation";
		languages.french.toolTipButtonGraph = "Afficher les graphiques résultats";
		languages.english.toolTipButtonGraph = "Display result graphics";

		languages.Used = languages.french;
		#endregion
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
