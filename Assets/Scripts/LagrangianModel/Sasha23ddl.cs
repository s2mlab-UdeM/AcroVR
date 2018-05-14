using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================================================================================================================================
/// <summary> Modèle Lagrangien Sasha23ddl. </summary>

public class Sasha23ddl : MonoBehaviour
{
	public static Sasha23ddl Instance;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
	}

	// =================================================================================================================================================================
	/// <summary> Initialisation du modèle Lagrangien utilisé. </summary>

	public void InitLagrangianModel ()
	{
		MainParameters.Instance.lagrangianModel.nDDL = 23;
		MainParameters.Instance.lagrangianModel.nTAG = 22;
		MainParameters.Instance.lagrangianModel.nSOL = 17;
		MainParameters.Instance.lagrangianModel.q1 = new int[] { 1, 2, 3, 4, 5, 6 };
		MainParameters.Instance.lagrangianModel.q2 = new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
		MainParameters.Instance.lagrangianModel.dt = 0.02f;

		//		ws.root_right = 1;
		//		ws.root_foreward = 2;
		//		ws.root_upward = 3;
		//		ws.root_somersault = 4;
		//		ws.root_tilt = 5;
		//		ws.root_twist = 6;

		MainParameters.Instance.lagrangianModel.ddlName = new string[23];
		MainParameters.Instance.lagrangianModel.ddlName[0] = "";
		MainParameters.Instance.lagrangianModel.ddlName[1] = "";
		MainParameters.Instance.lagrangianModel.ddlName[2] = "";
		MainParameters.Instance.lagrangianModel.ddlName[3] = "";
		MainParameters.Instance.lagrangianModel.ddlName[4] = "";
		MainParameters.Instance.lagrangianModel.ddlName[5] = "";
		MainParameters.Instance.lagrangianModel.ddlName[6] = "thorax_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[7] = "thorax_inclinaison_laterale";
		MainParameters.Instance.lagrangianModel.ddlName[8] = "thorax_rotation";
		MainParameters.Instance.lagrangianModel.ddlName[9] = "tete_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[10] = "bras_droit_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[11] = "bras_droit_abduction_adduction";
		MainParameters.Instance.lagrangianModel.ddlName[12] = "bras_droit_rotation_ext_int";
		MainParameters.Instance.lagrangianModel.ddlName[13] = "abras_droit_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[14] = "bras_gauche_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[15] = "bras_gauche_abduction_adduction";
		MainParameters.Instance.lagrangianModel.ddlName[16] = "bras_gauche_rotation_ext_int";
		MainParameters.Instance.lagrangianModel.ddlName[17] = "abras_gauche_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[18] = "cuisses_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[19] = "cuisses_inclinaison_laterale";
		MainParameters.Instance.lagrangianModel.ddlName[20] = "cuisses_rotation";
		MainParameters.Instance.lagrangianModel.ddlName[21] = "jambes_flexion_extension";
		MainParameters.Instance.lagrangianModel.ddlName[22] = "pieds_flexion_extension";

		//		ws.feet = [78 95];
		//		ws.hand = [39 71];

		MainParameters.Instance.lagrangianModel.stickFigure = new int[28, 2] { { 96, 97 }, { 15, 16 }, { 16, 17 }, { 17, 13 }, { 13, 14 }, { 14, 15 }, { 14, 16 }, { 13, 15 }, { 15, 17 },
																				{ 7, 99 }, { 99, 100 }, { 100, 101 }, { 101, 102 }, { 102, 39 }, { 7, 103 }, { 103, 104 }, { 104, 105 },
																				{ 105, 106 }, { 106, 61 }, { 107, 110 }, { 96, 107 }, { 107, 108 }, { 108, 109 }, { 109, 77 }, { 96, 110 },
																				{ 110, 111 }, { 111, 112 }, { 112, 94 } };

		MainParameters.Instance.lagrangianModel.filledFigure = new int[4, 4] { { 97, 21, 43, 97 }, { 97, 23, 45, 97 }, { 97, 21, 23, 97 }, { 97, 43, 45, 97 } };
	}
}
