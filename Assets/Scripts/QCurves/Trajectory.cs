﻿// =================================================================================================================================================================
/// <summary> Interpolation des angles pour chacune des articulations, selon la méthode d'interpolation utilisée (Quintic ou Spline cubique). </summary>

public class Trajectory
{
	public Trajectory(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float t, int[] qi, out float[] qd)
	{
		float[] qdotd;
		float[] qddotd;
		Trajectory trajectory = new Trajectory(lagrangianModel, t, qi, out qd, out qdotd, out qddotd);
		trajectory.ToString();					// Pour enlever un warning lors de la compilation
	}

	public Trajectory(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float t, int[] qi, out float[] qd, out float[] qdotd, out float[] qddotd)
	{
		// Initialisation des DDL à traiter et du nombre de ces DDL

		int n = qi.Length;

		// Initialisation des vecteurs contenant les positions, vitesses et accélérations des angles des articulations traités

		qd = new float[n];
		qdotd = new float[n];
		qddotd = new float[n];

		// Initialisation de la classe contenant le calcul des interpolations de type Quintic

		MathFunc mathFunc = new MathFunc();

		// Boucle sur les DDLs à traiter

		for (int i = 0; i < n; i++)
		{
			MainParameters.StrucNodes nodes = MainParameters.Instance.joints.nodes[qi[i] - lagrangianModel.q2[0]];
			switch (nodes.interpolation.type)
			{
				case MainParameters.InterpolationType.Quintic:
					int j = 1;
					while (j < nodes.T.Length - 1 && t > nodes.T[j]) j++;
					mathFunc.Quintic(t, nodes.T[j - 1], nodes.T[j], nodes.Q[j - 1], nodes.Q[j], out qd[i], out qdotd[i], out qddotd[i]);
					break;
				case MainParameters.InterpolationType.CubicSpline:
					break;
			}
		}
	}
}