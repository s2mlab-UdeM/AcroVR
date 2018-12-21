using System;
using System.Linq;
using Microsoft.Research.Oslo;

// =================================================================================================================================================================
/// <summary> Exécution des calculs de simulation. </summary>

public class DoSimulation
{
	public DoSimulation(out float[,] qOut)
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

		q0[Math.Abs(joints.lagrangianModel.root_tilt) - 1] = joints.takeOffParam.tilt * (float)Math.PI / 180;                       // en radians
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

		double[] x0 = new double[joints.lagrangianModel.nDDL * 2];
		for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
		{
			x0[i] = q0[i];
			x0[joints.lagrangianModel.nDDL + i] = q0dot[i];
		}

		Options options = new Options();
		options.InitialStep = joints.lagrangianModel.dt;

		var sol = Ode.RK547M(0, joints.duration + joints.lagrangianModel.dt, new Vector(x0), ShortDynamics, options);
		var points = sol.SolveFromToStep(0, joints.duration + joints.lagrangianModel.dt, joints.lagrangianModel.dt).ToArray();

		double[] t = new double[points.GetUpperBound(0) + 1];
		double[,] q = new double[joints.lagrangianModel.nDDL, points.GetUpperBound(0) + 1];
		double[,] qdot = new double[joints.lagrangianModel.nDDL, points.GetUpperBound(0) + 1];
		for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
		{
			for (int j = 0; j <= points.GetUpperBound(0); j++)
			{
				if (i <= 0)
					t[j] = points[j].T;

				q[i,j] = points[j].X[i];
				qdot[i,j] = points[j].X[joints.lagrangianModel.nDDL + i];
			}
		}
		#endregion

		int tIndex = 0;
		MainParameters.Instance.joints.tc = 0;
		for (int i = 0; i <= q.GetUpperBound(1); i++)
		{
			tIndex++;
			double[] qq = new double[joints.lagrangianModel.nDDL];
			for (int j = 0; j < joints.lagrangianModel.nDDL; j++)
				qq[j] = q[j, i];
			AnimationF.Instance.EvaluateTags(qq, out tagX, out tagY, out tagZ);
			if (joints.condition > 0 && tagZ.Min() < -0.05f)
			{
				MainParameters.Instance.joints.tc = (float)t[i];
				AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} {1:0.00} s", MainParameters.Instance.languages.Used.displayMsgContactGround, MainParameters.Instance.joints.tc));
				break;
			}
		}

		MainParameters.Instance.joints.t = new float[tIndex];
		qOut = new float[joints.lagrangianModel.nDDL, tIndex];
		float[,] qdot1 = new float[joints.lagrangianModel.nDDL, tIndex];
		for (int i = 0; i < tIndex; i++)
		{
			MainParameters.Instance.joints.t[i] = (float)t[i];
			for (int j = 0; j < joints.lagrangianModel.nDDL; j++)
			{
				qOut[j, i] = (float)q[j, i];
				qdot1[j, i] = (float)qdot[j, i];
			}
		}

		MainParameters.Instance.joints.rot = new float[tIndex, rotation.Length];
		MainParameters.Instance.joints.rotdot = new float[tIndex, rotation.Length];
		float[,] rotAbs = new float[tIndex, rotation.Length];
		for (int i = 0; i < rotation.Length; i++)
		{
			float[] rotCol = new float[tIndex];
			float[] rotdotCol = new float[tIndex];
			rotCol = MathFunc.unwrap(MathFunc.MatrixGetRow(qOut, rotation[i] - 1));
			rotdotCol = MathFunc.unwrap(MathFunc.MatrixGetRow(qdot1, rotation[i] - 1));
			for (int j = 0; j < tIndex; j++)
			{
				MainParameters.Instance.joints.rot[j, i] = rotCol[j] / (2 * (float)Math.PI);
				MainParameters.Instance.joints.rotdot[j, i] = rotdotCol[j] / (2 * (float)Math.PI);
				rotAbs[j, i] = Math.Abs(MainParameters.Instance.joints.rot[j, i]);
			}
		}

		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} = {1:0.00}", MainParameters.Instance.languages.Used.displayMsgNumberSomersaults, MathFunc.MatrixGetColumn(rotAbs, 0).Max()));
		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} = {1:0.00}", MainParameters.Instance.languages.Used.displayMsgNumberTwists, MathFunc.MatrixGetColumn(rotAbs, 2).Max()));
		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} = {1:0.00}", MainParameters.Instance.languages.Used.displayMsgFinalTwist, MainParameters.Instance.joints.rot[tIndex - 1, 2]));
		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} = {1:0}°", MainParameters.Instance.languages.Used.displayMsgMaxTilt, MathFunc.MatrixGetColumn(rotAbs, 1).Max() * 360));
		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" {0} = {1:0}°", MainParameters.Instance.languages.Used.displayMsgFinalTilt, MainParameters.Instance.joints.rot[tIndex - 1, 1] * 360));
	}

	// =================================================================================================================================================================
	/// <summary> Routine qui sera exécuter par le ODE (Ordinary Differential Equation). </summary>

	Vector ShortDynamics(double t, Vector x)
	{
		//		q1 = ws.q1;% simulation
		//q2 = ws.q2;% driven
		int nDDL = MainParameters.Instance.joints.lagrangianModel.nDDL;

		double[] q = new double[nDDL];
		double[] qdot = new double[nDDL];
		for (int i = 0; i < nDDL; i++)
		{
			q[i] = x[i];
			qdot[i] = x[nDDL + i];
		}

		double[,] m12;
		double[] n1;
		Inertia11Simple inertia11Simple = new Inertia11Simple();
		double[,] m11 = inertia11Simple.Inertia11(q);
		if (MainParameters.Instance.joints.lagrangianModelName == MainParameters.LagrangianModelNames.Sasha23ddl)
		{
			n1 = new double[12];
			m12 = new double[6,6];
		}
		else
		{
			Inertia12Simple inertia12Simple = new Inertia12Simple();
			m12 = inertia12Simple.Inertia12(q);
			NLEffects1Simple nlEffects1Simple = new NLEffects1Simple();
			n1 = nlEffects1Simple.NLEffects1(q, qdot);
			if (MainParameters.Instance.joints.condition <= 0)
			{
				double[] n1zero;
				n1zero = nlEffects1Simple.NLEffects1(q, new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
				for (int i = 0; i < 6; i++)
					n1[i] = n1[i] - n1zero[i];
			}
		}

		float kp = 10;
		float kv = 3;
		float[] qd = new float[nDDL];
		float[] qdotd = new float[nDDL];
		float[] qddotd = new float[nDDL];
		Trajectory trajectory = new Trajectory(MainParameters.Instance.joints.lagrangianModel, (float)t, MainParameters.Instance.joints.lagrangianModel.q2, out qd, out qdotd, out qddotd);
		trajectory.ToString();                  // Pour enlever un warning lors de la compilation

		float[] qddot = new float[nDDL];
		for (int i = 0; i < nDDL; i++)
			qddot[i] = qddotd[i] + kp * (qd[i] - (float)q[i]) + kv * (qdotd[i] - (float)qdot[i]);

		if (MainParameters.Instance.joints.lagrangianModelName == MainParameters.LagrangianModelNames.Sasha23ddl)
		{
			//M12qddotN1 = ForceVector(q, qdot, qddot);
			//qddot(q1) = -M11 \ M12qddotN1; 
		}
		else
		{
			// Calcul "Matrix Left division" suivante: qddot(q1) = M11\(-N1-M12*qddot(q2));
			// On peut faire ce calcul en utilisant le calcul "Matrix inverse": qddot(q1) = inv(M11)*(-N1-M12*qddot(q2));

			double[,] mA = MatrixInverse.MtrxInverse(m11);

			double[] q2qddot = new double[MainParameters.Instance.joints.lagrangianModel.q2.Length];
			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q2.Length; i++)
				q2qddot[i] = qddot[MainParameters.Instance.joints.lagrangianModel.q2[i] - 1];
			double[,] mB = MatrixInverse.MtrxProduct(m12, q2qddot);

			double[,] n1mB = new double[mB.GetUpperBound(0) + 1, mB.GetUpperBound(1) + 1];
			for (int i = 0; i <= mB.GetUpperBound(0); i++)			// Rangée
				for (int j = 0; j <= mB.GetUpperBound(1); j++)      // Colonne
					n1mB[i, j] = -n1[i] - mB[i, j];

			double[,] mC = MatrixInverse.MtrxProduct(mA, n1mB);

			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q1.Length; i++)
				qddot[MainParameters.Instance.joints.lagrangianModel.q1[i] -1] = (float)mC[i,0];
		}

		double[] xdot = new double[MainParameters.Instance.joints.lagrangianModel.nDDL * 2];
		for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.nDDL; i++)
		{
			xdot[i] = qdot[i];
			xdot[MainParameters.Instance.joints.lagrangianModel.nDDL + i] = qddot[i];
		}

		return new Vector(xdot);
	}
}
