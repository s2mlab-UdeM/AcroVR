using System;

// =================================================================================================================================================================
/// <summary> Exécution des calculs de simulation. </summary>

public class DoSimulation
{
	public DoSimulation()
	{
		// Affichage d'un message dans la boîte des messages

		AnimationF.Instance.DisplayNewMessage(false, true, string.Format(" Simulation time = {0:0.0} s", MainParameters.Instance.joints.duration));

		// Définir un nom racourci pour avoir accès à la structure Joints

		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Initialisation de la classe MathFunc

		MathFunc mathFunc = new MathFunc();

		#region Init_Move
		// Init_Move

		float[] q0 = new float[joints.lagrangianModel.nDDL];
		float[] q0dot = new float[joints.lagrangianModel.nDDL];
		float[] q0dotdot = new float[joints.lagrangianModel.nDDL];
		Trajectory trajectory = new Trajectory(joints.lagrangianModel, 0, joints.lagrangianModel.q2, out q0, out q0dot, out q0dotdot);
		trajectory.ToString();                  // Pour enlever un warning lors de la compilation

		int[] rotation = new int[3] { joints.lagrangianModel.root_somersault, joints.lagrangianModel.root_tilt, joints.lagrangianModel.root_twist };
		int[] rotationS = mathFunc.Sign(rotation);
		for (int i = 0; i < rotation.Length; i++) rotation[i] = Math.Abs(rotation[i]);

		int[] translation = new int[3] { joints.lagrangianModel.root_right, joints.lagrangianModel.root_foreward, joints.lagrangianModel.root_upward };
		int[] translationS = mathFunc.Sign(translation);
		for (int i = 0; i < translation.Length; i++) translation[i] = Math.Abs(translation[i]);

		float rotRadians = joints.takeOffParam.rotation * (float)Math.PI / 180;

		q0[Math.Abs(joints.lagrangianModel.root_tilt) - 1] = joints.takeOffParam.tilt * (float)Math.PI / 180;                       // en radians
		q0[Math.Abs(joints.lagrangianModel.root_somersault) - 1] = rotRadians;														// en radians
		q0dot[Math.Abs(joints.lagrangianModel.root_foreward) - 1] = joints.takeOffParam.anteroposteriorSpeed;						// en m/s
		q0dot[Math.Abs(joints.lagrangianModel.root_upward) - 1] = joints.takeOffParam.verticalSpeed;								// en m/s
		q0dot[Math.Abs(joints.lagrangianModel.root_somersault) - 1] = joints.takeOffParam.somersaultSpeed * 2 * (float)Math.PI;		// en radians/s
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
		float[,] rotM = mathFunc.MultiplyMatrix(u, rot);
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
	}
	//for (int i = 0; i<q0.Length; i++)
	//	System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("q0 = {0}{1}", q0[i], System.Environment.NewLine));
}
