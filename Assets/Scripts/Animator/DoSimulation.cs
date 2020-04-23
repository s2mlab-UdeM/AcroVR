using System;
using System.Linq;
using Microsoft.Research.Oslo;
using System.Runtime.InteropServices;

// =================================================================================================================================================================
/// <summary> Exécution des calculs de simulation. </summary>

public class DoSimulation
{
	/// <summary> Vecteur contenant l'état (q0 et q0dot) au temps t. </summary>
	public static double[] xT;
	/// <summary> Vecteur contenant l'état (q0 et q0dot) au temps t + dt. </summary>
	public static double[] xTplusdT;

	// Déclaration des vecteurs contenant les positions, vitesses et accelerations des articulations à l'instant t et t + dt

	public static float[] q2T;
	public static float[] q2dotT;
	public static float[] q2dotdotT;
	public static float[] q2TplusdT;
	public static float[] q2dotTplusdT;
	public static float[] q2dotdotTplusdT;

	// Déclaration des pointeurs

	static IntPtr ptr_massMatrix;
	static IntPtr ptr_tau;
	static IntPtr ptr_Q;
	static IntPtr ptr_V;
	static IntPtr ptr_qddot2;
	static IntPtr ptr_matA;
	static IntPtr ptr_solX;

	public static bool modeRT = false;

	public static int GetSimulation(out float[,] qOut)
	{
		// Affichage d'un message dans la boîte des messages

		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} = {1:0.0} s", MainParameters.Instance.languages.Used.displayMsgSimulationTime, MainParameters.Instance.joints.duration));

		// Définir un nom racourci pour avoir accès à la structure Joints

		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Init_Move

		#region Init_Move

		float[] q0 = new float[joints.lagrangianModel.nDDL];
		float[] q0dot = new float[joints.lagrangianModel.nDDL];
		float[] q0dotdot = new float[joints.lagrangianModel.nDDL];
		Trajectory trajectory = new Trajectory(joints.lagrangianModel, 0, joints.lagrangianModel.q2, out q0, out q0dot, out q0dotdot);
		trajectory.ToString();                  // Pour enlever un warning lors de la compilation

		int[] rotation = new int[3] { joints.lagrangianModel.root_somersault, joints.lagrangianModel.root_tilt, joints.lagrangianModel.root_twist };
		int[] rotationS = MathFunc.Sign(rotation);
		for (int i = 0; i < rotation.Length; i++) rotation[i] = Math.Abs(rotation[i]);

		int[] translation = new int[3] { joints.lagrangianModel.root_right, joints.lagrangianModel.root_foreward, joints.lagrangianModel.root_upward };
		int[] translationS = MathFunc.Sign(translation);
		for (int i = 0; i < translation.Length; i++) translation[i] = Math.Abs(translation[i]);

		float rotRadians = joints.takeOffParam.rotation * (float)Math.PI / 180;

		float tilt = joints.takeOffParam.tilt;
		if (tilt == 90)                                 // La fonction Ode.RK547M donne une erreur fatale, si la valeur d'inclinaison est de 90 ou -90 degrés
			tilt = 90.001f;
		else if (tilt == -90)
			tilt = -90.01f;
		q0[Math.Abs(joints.lagrangianModel.root_tilt) - 1] = tilt * (float)Math.PI / 180;                                           // en radians
		q0[Math.Abs(joints.lagrangianModel.root_somersault) - 1] = rotRadians;                                                      // en radians
		q0dot[Math.Abs(joints.lagrangianModel.root_foreward) - 1] = joints.takeOffParam.anteroposteriorSpeed;                       // en m/s
		q0dot[Math.Abs(joints.lagrangianModel.root_upward) - 1] = joints.takeOffParam.verticalSpeed;                                // en m/s
		q0dot[Math.Abs(joints.lagrangianModel.root_somersault) - 1] = joints.takeOffParam.somersaultSpeed * 2 * (float)Math.PI;     // en radians/s
		q0dot[Math.Abs(joints.lagrangianModel.root_twist) - 1] = joints.takeOffParam.twistSpeed * 2 * (float)Math.PI;               // en radians/s

		// correction of linear velocity to have CGdot = qdot

		double[] Q = new double[joints.lagrangianModel.nDDL];
		for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
			Q[i] = q0[i];
		float[] tagX;
		float[] tagY;
		float[] tagZ;
		AnimationF.Instance.EvaluateTags(Q, out tagX, out tagY, out tagZ);

		float[] cg = new float[3];          // CG in initial posture
		cg[0] = tagX[tagX.Length - 1];
		cg[1] = tagY[tagX.Length - 1];
		cg[2] = tagZ[tagX.Length - 1];

		float[] u1 = new float[3];
		float[,] rot = new float[3,1];
		for (int i = 0; i < 3; i++)
		{
			u1[i] = cg[i] - q0[translation[i] - 1] * translationS[i];
			rot[i,0] = q0dot[rotation[i] - 1] * rotationS[i];
		}
		float[,] u = { { 0, -u1[2], u1[1] }, { u1[2], 0, -u1[0] }, { -u1[1], u1[0], 0 } };
		float[,] rotM = MathFunc.MatrixMultiply(u, rot);
		for (int i = 0; i < 3; i++)
		{
			q0dot[translation[i] - 1] = q0dot[translation[i] - 1] * translationS[i] + rotM[i, 0];
			q0dot[translation[i] - 1] = q0dot[translation[i] - 1] * translationS[i];
		}

		float hFeet = Math.Min(tagZ[joints.lagrangianModel.feet[0] - 1], tagZ[joints.lagrangianModel.feet[1] - 1]);
		float hHand = Math.Min(tagZ[joints.lagrangianModel.hand[0] - 1], tagZ[joints.lagrangianModel.hand[1] - 1]);

		if (joints.condition < 8 && Math.Cos(rotRadians) > 0)
			q0[Math.Abs(joints.lagrangianModel.root_upward) - 1] += joints.lagrangianModel.hauteurs[joints.condition] - hFeet;
		else															// bars, vault and tumbling from hands
			q0[Math.Abs(joints.lagrangianModel.root_upward) - 1] += joints.lagrangianModel.hauteurs[joints.condition] - hHand;
		#endregion

		// Sim_Airborn

		#region Sim_Airborn

		xT = new double[joints.lagrangianModel.nDDL * 2];
		for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
		{
			xT[i] = q0[i];
			xT[joints.lagrangianModel.nDDL + i] = q0dot[i];
		}

		Options options = new Options();
		options.InitialStep = joints.lagrangianModel.dt;

		// Extraire les données obtenues du Runge-Kutta et conserver seulement les points interpolés aux frames désirés, selon la durée et le dt utilisé

		DoSimulation.modeRT = false;
		var sol = Ode.RK547M(0, joints.duration + joints.lagrangianModel.dt, new Vector(xT), ShortDynamics, options);
		var points = sol.SolveFromToStep(0, joints.duration + joints.lagrangianModel.dt, joints.lagrangianModel.dt).ToArray();

		double[,] q = new double[joints.lagrangianModel.nDDL, points.GetUpperBound(0) + 1];
        for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
			for (int j = 0; j <= points.GetUpperBound(0); j++)
				q[i,j] = points[j].X[i];
		#endregion

        // Vérifier s'il y a un contact avec le sol

		int index = 0;
		for (int i = 0; i <= q.GetUpperBound(1); i++)
		{
			index++;
			double[] qq = new double[joints.lagrangianModel.nDDL];
			for (int j = 0; j < joints.lagrangianModel.nDDL; j++)
				qq[j] = q[j, i];
			AnimationF.Instance.EvaluateTags(qq, out tagX, out tagY, out tagZ);
            if (joints.condition > 0 && tagZ.Min() < -0.05f)
				break;
		}

        // Copier les q dans une autre matrice qOut, mais contient seulement les données jusqu'au contact avec le sol
        // Utiliser seulement pour calculer la dimension du volume utilisé pour l'animation

        qOut = new float[MainParameters.Instance.joints.lagrangianModel.nDDL, index];
        for (int i = 0; i < index; i++)
            for (int j = 0; j < MainParameters.Instance.joints.lagrangianModel.nDDL; j++)
                qOut[j, i] = (float)q[j, i];

        return points.GetUpperBound(0) + 1;
	}

	// =================================================================================================================================================================
	/// <summary> Routine qui sera exécuter par le ODE (Ordinary Differential Equation). </summary>

	public static Vector ShortDynamics(double t, Vector x)
	{
		int NDDL = MainParameters.c_nQ(MainParameters.Instance.ptr_model);      // Récupère le nombre de DDL du modèle BioRBD
		int NROOT = 6;                                                          // On admet que la racine possède 6 ddl
		int NDDLhumans = 12;
		double[] xBiorbd = new double[NDDL * 2];

		double[] Qintegrateur = new double[NDDL];
		double[] Vintegrateur = new double[NDDL];
		double[] m_taud = new double[NDDL];
		double[] massMatrix = new double[NDDL * NDDL];

		float[] qd = new float[NDDL];
		float[] qdotd = new float[NDDL];
		float[] qddotd = new float[NDDL];

		float[] qTdT = new float[NDDL];                                         // Tableau des DDL(positions) à l'instant t + dt
		float[] qdotTdT = new float[NDDL];                                      // Tableau des DDL(vitesses) à l'instant t + dt
		float[] qddotTdT = new float[NDDL];                                     // Tableau des DDL(accélérations) à l'instant t + dt
		float[] qT = new float[NDDL];                                           // Tableau des DDL(positions) à l'instant t
		float[] qdotT = new float[NDDL];                                        // Tableau des DDL(vitesses) à l'instant t
		float[] qddotT = new float[NDDL];                                       // Tableau des DDL(accélérations) à l'instant t

		double[] qddot2 = new double[NDDL];
		double[] qddot1integ = new double[NDDL * 2];
		double[] qddot1integHumans = new double[NDDLhumans];

		//Allocations des pointeurs, sinon génère erreurs de segmentation

		ptr_Q = Marshal.AllocCoTaskMem(sizeof(double) * Qintegrateur.Length);
		ptr_V = Marshal.AllocCoTaskMem(sizeof(double) * Vintegrateur.Length);
		ptr_qddot2 = Marshal.AllocCoTaskMem(sizeof(double) * qddot2.Length);
		ptr_massMatrix = Marshal.AllocCoTaskMem(sizeof(double) * massMatrix.Length);
		ptr_tau = Marshal.AllocCoTaskMem(sizeof(double) * m_taud.Length);

		// On convertit les DDL du modèle Humans pour le modèle BioRBD

		xBiorbd = ConvertHumansBioRBD.Humans2Biorbd(x);

		for (int i = 0; i < NDDL; i++)
		{
			Qintegrateur[i] = xBiorbd[i];
			Vintegrateur[i] = xBiorbd[i + NDDL];
		}

		if (!modeRT)								// Offline
		{
			float[] qdH = new float[NDDLhumans];
			float[] qdotdH = new float[NDDLhumans];
			float[] qddotdH = new float[NDDLhumans];

			Trajectory trajectory = new Trajectory(MainParameters.Instance.joints.lagrangianModel, (float)t, MainParameters.Instance.joints.lagrangianModel.q2, out qdH, out qdotdH, out qddotdH);
			trajectory.ToString();                  // Pour enlever un warning lors de la compilation

			qd = ConvertHumansBioRBD.qValuesHumans2Biorbd(qdH);
			qdotd = ConvertHumansBioRBD.qValuesHumans2Biorbd(qdotdH);
			qddotd = ConvertHumansBioRBD.qValuesHumans2Biorbd(qddotdH);
		}
		else										// Online
		{
			qT = ConvertHumansBioRBD.qValuesHumans2Biorbd(q2T);
			qdotT = ConvertHumansBioRBD.qValuesHumans2Biorbd(q2dotT);
			qddotT = ConvertHumansBioRBD.qValuesHumans2Biorbd(q2dotdotT);
			qTdT = ConvertHumansBioRBD.qValuesHumans2Biorbd(q2TplusdT);
			qdotTdT = ConvertHumansBioRBD.qValuesHumans2Biorbd(q2dotTplusdT);
			qddotTdT = ConvertHumansBioRBD.qValuesHumans2Biorbd(q2dotdotTplusdT);

			// Fonction qui remplace Trajectory, retourne la valeur de Q, QDot, QDotDot à l'instant t

			Interpolate(t, qT, qTdT, out qd);
			Interpolate(t, qdotT, qdotTdT, out qdotd);
			Interpolate(t, qddotT, qddotTdT, out qddotd);
		}

		for (int i = 0; i < qddot2.Length; i++)
			qddot2[i] = qddotd[i] + 10 * (qd[i] - Qintegrateur[i]) + 3 * (qdotd[i] - Vintegrateur[i]);
		for (int i = 0; i < NROOT; i++)
			qddot2[i] = 0;

		Marshal.Copy(Qintegrateur, 0, ptr_Q, Qintegrateur.Length);
		Marshal.Copy(Vintegrateur, 0, ptr_V, Vintegrateur.Length);
		Marshal.Copy(qddot2, 0, ptr_qddot2, qddot2.Length);

		// Génère la matrice de masse

		MainParameters.c_massMatrix(MainParameters.Instance.ptr_model, ptr_Q, ptr_massMatrix);

		Marshal.Copy(ptr_massMatrix, massMatrix, 0, massMatrix.Length);

		MainParameters.c_inverseDynamics(MainParameters.Instance.ptr_model, ptr_Q, ptr_V, ptr_qddot2, ptr_tau);

		Marshal.Copy(ptr_tau, m_taud, 0, m_taud.Length);

		double[,] squareMassMatrix = new double[NDDL, NDDL];
		squareMassMatrix = MathFunc.ConvertVectorInSquareMatrix(massMatrix);            // La matrice de masse générée est sous forme d'un vecteur de taille NDDL*NDDL

		double[,] matriceA = new double[NROOT, NROOT];
		matriceA = MathFunc.ShrinkSquareMatrix(squareMassMatrix, NROOT);                // On réduit la matrice de masse

		double[] matAGrandVecteur = new double[NROOT * NROOT];
		matAGrandVecteur = MathFunc.ConvertSquareMatrixInVector(matriceA);              // La nouvelle matrice doit être convertie en vecteur pour qu'elle puisse être utilisée dans BioRBD

		ptr_matA = Marshal.AllocCoTaskMem(sizeof(double) * matAGrandVecteur.Length);
		ptr_solX = Marshal.AllocCoTaskMem(sizeof(double) * NROOT);

		Marshal.Copy(matAGrandVecteur, 0, ptr_matA, matAGrandVecteur.Length);

		MainParameters.c_solveLinearSystem(ptr_matA, NROOT, NROOT, ptr_tau, ptr_solX);  // Résouds l'équation Ax=b

		double[] solutionX = new double[NROOT];
		Marshal.Copy(ptr_solX, solutionX, 0, solutionX.Length);

		for (int i = 0; i < NROOT; i++)
			qddot2[i] = -solutionX[i];

		for (int i = 0; i < NDDL; i++)
		{
			qddot1integ[i] = Vintegrateur[i];
			qddot1integ[i + NDDL] = qddot2[i];
		}

		qddot1integHumans = ConvertHumansBioRBD.Biorbd2Humans(qddot1integ);             // Convertir les DDL du modèle BioRBD vers le modèle Humans

		// Désallocation des pointeurs

		Marshal.FreeCoTaskMem(ptr_Q);
		Marshal.FreeCoTaskMem(ptr_V);
		Marshal.FreeCoTaskMem(ptr_qddot2);
		Marshal.FreeCoTaskMem(ptr_massMatrix);
		Marshal.FreeCoTaskMem(ptr_tau);
		Marshal.FreeCoTaskMem(ptr_matA);
		Marshal.FreeCoTaskMem(ptr_solX);

		//return qddot1integHumans;
		return new Vector(qddot1integHumans);
	}

	// =================================================================================================================================================================
	/// <summary> Fonction qui remplace Trajectory, retourne la valeur de q2, q2dot, q2dotdot à l'instant t. </summary>

	public static void Interpolate(double interpolation, float[] vecteurFrom, float[] vecteurTo, out float[] vecteurInterpoler)
	{
		vecteurInterpoler = new float[vecteurFrom.Length];
		float t = (float)interpolation / (MainParameters.Instance.joints.lagrangianModel.dt / 2);		// Valeur du pas = 0.02 donc on divise pour avoir un pourcentage
		for (int i = 0; i < vecteurInterpoler.Length; i++)
			vecteurInterpoler[i] = UnityEngine.Mathf.Lerp(vecteurFrom[i], vecteurTo[i], t);				// Retourne un vecteur intermédiaire, proportionnel aux deux vecteur, selon la valeur interpolée
	}
}
