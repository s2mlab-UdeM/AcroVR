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
		//Implémentation de la méthode RK45 pour la simulation en temps réel
		public static Vector RK4TempsReel(Vector x0, Func<double, Vector, Vector> f)
		{
			float t = 0;
			Vector x = x0;
			int n = x0.Length;
			float dt = 0.02f;
			double dT = 0.02;

			Vector x1 = f(t, x);
			Vector xx = x + x1 * (dt / 2.0);
			Vector x2 = f(t + dt / 2.0f, xx);
			xx = x + x2 * (dt / 2.0);
			Vector x3 = f(t + dt / 2.0f, xx);
			xx = x + (x3 * dT);
			Vector x4 = f(t + dt, xx);
			x = x + (dt / 6.0) * (x1 + 2.0 * x2 + 2.0 * x3 + x4);

			return x;  //retourne état suivant à l'instant t+dt         
		}
	}
}