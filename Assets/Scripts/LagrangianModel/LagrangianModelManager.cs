// =================================================================================================================================================================
/// <summary> Paramètres et fonctions utilisés selon le modèle de Langrangien utilisé (classe abstraite de défaut, qui ne peut pas être appelé directement). </summary>

public abstract class LagrangianModelManager
{
	/// <summary> Description de la structure contenant les paramètres relatifs au modèle Lagrangien. </summary>
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

	// =================================================================================================================================================================
	/// <summary> Initialisation de la structure des paramètres selon le modèle de Langrangien utilisé (classe de défaut, qui ne peut pas être appelé directement). </summary>

	public abstract StrucLagrangianModel GetParameters { get; }
}

// =================================================================================================================================================================
/// <summary> Paramètres et fonctions utilisés pour le modèle de Langrangien Simple. </summary>

public class LagrangianModelSimple : LagrangianModelManager
{
	// =================================================================================================================================================================
	/// <summary> Initialisation de la structure des paramètres pour le modèle de Langrangien Simple. </summary>

	public override StrucLagrangianModel GetParameters
	{
		get
		{
			StrucLagrangianModel lagrangianModel = new StrucLagrangianModel();
			lagrangianModel.nDDL = 12;
			lagrangianModel.nTAG = 22;
			lagrangianModel.nSOL = 32;
			lagrangianModel.q1 = new int[] { 7, 8, 9, 10, 11, 12 };
			lagrangianModel.q2 = new int[] { 1, 2, 3, 4, 5, 6 };
			lagrangianModel.dt = 0.02f;
			// ws.root_right = 7;
			// ws.root_foreward = 8;
			// ws.root_upward = 9;
			// ws.root_somersault = -10;
			// ws.root_tilt = 11;
			// ws.root_twist = 12;
			lagrangianModel.ddlName = new string[6];
			lagrangianModel.ddlName[0] = "Hip_Flexion";
			lagrangianModel.ddlName[1] = "Knee_Flexion";
			lagrangianModel.ddlName[2] = "Left_Arm_Flexion";
			lagrangianModel.ddlName[3] = "Left_Arm_Abduction";
			lagrangianModel.ddlName[4] = "Right_Arm_Flexion";
			lagrangianModel.ddlName[5] = "Right_Arm_Abduction";
			// ws.feet = [4 8];
			// ws.hand = [15 21];
			lagrangianModel.stickFigure = new int[27, 2] { { 1, 2 }, { 2, 3 }, { 3, 4 }, { 5, 6 }, { 6, 7 }, { 7, 8 }, { 1, 9 }, { 5, 9 }, { 1, 5 },
															{ 9, 11 }, { 12, 9 }, { 9, 17 }, { 18, 9 }, { 17, 18 }, { 18, 12 }, { 12, 11 }, { 11, 17 },
															{ 10, 13 }, { 13, 14 }, { 14, 15 }, { 16, 19 }, { 19, 20 }, { 20, 21 }, { 22, 23 }, { 23, 25 },
															{ 24, 22 }, { 25, 24 } };
			lagrangianModel.filledFigure = new int[6, 3] { { 9, 12, 18 }, { 9, 11, 12 }, { 9, 11, 17 }, { 9, 17, 18 }, { 11, 12, 17 }, { 12, 17, 18 } };

			return lagrangianModel;
		}
	}
}

// =================================================================================================================================================================
/// <summary> Paramètres et fonctions utilisés pour le modèle de Langrangien Sasha23ddl. </summary>

public class LagrangianModelSasha23ddl : LagrangianModelManager
{
	// =================================================================================================================================================================
	/// <summary> Initialisation de la structure des paramètres pour le modèle de Langrangien Sasha23ddl. </summary>

	public override StrucLagrangianModel GetParameters
	{
		get
		{
			StrucLagrangianModel lagrangianModel = new StrucLagrangianModel();
			lagrangianModel.nDDL = 23;
			lagrangianModel.nTAG = 22;
			lagrangianModel.nSOL = 17;
			lagrangianModel.q1 = new int[] { 1, 2, 3, 4, 5, 6 };
			lagrangianModel.q2 = new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
			lagrangianModel.dt = 0.02f;
			// ws.root_right = 1;
			// ws.root_foreward = 2;
			// ws.root_upward = 3;
			// ws.root_somersault = 4;
			// ws.root_tilt = 5;
			// ws.root_twist = 6;
			lagrangianModel.ddlName = new string[23];
			lagrangianModel.ddlName[0] = "";
			lagrangianModel.ddlName[1] = "";
			lagrangianModel.ddlName[2] = "";
			lagrangianModel.ddlName[3] = "";
			lagrangianModel.ddlName[4] = "";
			lagrangianModel.ddlName[5] = "";
			lagrangianModel.ddlName[6] = "thorax_flexion_extension";
			lagrangianModel.ddlName[7] = "thorax_inclinaison_laterale";
			lagrangianModel.ddlName[8] = "thorax_rotation";
			lagrangianModel.ddlName[9] = "tete_flexion_extension";
			lagrangianModel.ddlName[10] = "bras_droit_flexion_extension";
			lagrangianModel.ddlName[11] = "bras_droit_abduction_adduction";
			lagrangianModel.ddlName[12] = "bras_droit_rotation_ext_int";
			lagrangianModel.ddlName[13] = "abras_droit_flexion_extension";
			lagrangianModel.ddlName[14] = "bras_gauche_flexion_extension";
			lagrangianModel.ddlName[15] = "bras_gauche_abduction_adduction";
			lagrangianModel.ddlName[16] = "bras_gauche_rotation_ext_int";
			lagrangianModel.ddlName[17] = "abras_gauche_flexion_extension";
			lagrangianModel.ddlName[18] = "cuisses_flexion_extension";
			lagrangianModel.ddlName[19] = "cuisses_inclinaison_laterale";
			lagrangianModel.ddlName[20] = "cuisses_rotation";
			lagrangianModel.ddlName[21] = "jambes_flexion_extension";
			lagrangianModel.ddlName[22] = "pieds_flexion_extension";
			// ws.feet = [78 95];
			// ws.hand = [39 71];
			lagrangianModel.stickFigure = new int[28, 2] { { 96, 97 }, { 15, 16 }, { 16, 17 }, { 17, 13 }, { 13, 14 }, { 14, 15 }, { 14, 16 }, { 13, 15 }, { 15, 17 },
															{ 7, 99 }, { 99, 100 }, { 100, 101 }, { 101, 102 }, { 102, 39 }, { 7, 103 }, { 103, 104 }, { 104, 105 },
															{ 105, 106 }, { 106, 61 }, { 107, 110 }, { 96, 107 }, { 107, 108 }, { 108, 109 }, { 109, 77 }, { 96, 110 },
															{ 110, 111 }, { 111, 112 }, { 112, 94 } };
			lagrangianModel.filledFigure = new int[4, 4] { { 97, 21, 43, 97 }, { 97, 23, 45, 97 }, { 97, 21, 23, 97 }, { 97, 43, 45, 97 } };

			return lagrangianModel;
		}
	}
}
