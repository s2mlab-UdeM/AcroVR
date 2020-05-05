using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace Microsoft.Research.Oslo
{
	/// <summary>
	/// Contains numerical methods for ODE solving.
	/// </summary>
	public static partial class Ode
	{
		// Implémentation de la méthode RK45 pour la simulation en temps réel
		public static Vector RK4TempsReel(Vector x0, Func<Vector, float[], float[], float[], Vector> f)
		{
			//Vector x = x0;
			//double dt = MainParameters.Instance.joints.lagrangianModel.dt;
			//double dt2 = dt / 2;

			//Vector x1 = f(0, x);
			//Vector xx = x + x1 * dt2;
			//Vector x2 = f(0.5f, xx);
			//xx = x + x2 * dt2;
			//Vector x3 = f(0.5f, xx);
			//xx = x + x3 * dt;
			//Vector x4 = f(1, xx);
			//x = x + (dt / 6.0) * (x1 + 2.0 * x2 + 2.0 * x3 + x4);

			Vector x = x0;
			double dt = MainParameters.Instance.joints.lagrangianModel.dt;
			double dt2 = dt / 2;

			Vector x1 = f(x, DoSimulation.qFrame0, DoSimulation.qdotFrame0, DoSimulation.qddotFrame0);
			Vector x2 = f(x + x1 * dt2, DoSimulation.qFrame1, DoSimulation.qdotFrame1, DoSimulation.qddotFrame1);
			Vector x3 = f(x + x2 * dt2, DoSimulation.qFrame1, DoSimulation.qdotFrame1, DoSimulation.qddotFrame1);
			Vector x4 = f(x + x3 * dt, DoSimulation.qFrame2, DoSimulation.qdotFrame2, DoSimulation.qddotFrame2);
			x = x + (dt / 6.0) * (x1 + 2.0 * x2 + 2.0 * x3 + x4);

			return x;  //retourne état suivant à l'instant t+dt         
		}
	}
}