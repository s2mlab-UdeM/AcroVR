using System;

// =================================================================================================================================================================
/// <summary> Classe Initia12Simple pour le modèle lagrangien Simple. </summary>

public class Inertia12Simple
{
	// =================================================================================================================================================================
	/// <summary> Fonction Inertia12 pour le modèle lagrangien Simple. </summary>

	public double[,] Inertia12(double[] q)
	{
		double[] M12 = new double[36];
		double t1 = Math.Cos(q[10]);
		double t2 = Math.Sin(q[11]);
		double t3 = t1 * t2;
		double t4 = Math.Cos(q[0]);
		double t5 = Math.Sin(q[1]);
		double t6 = Math.Sqrt(3);
		double t7 = t5 * t6;
		double t8 = 0.3770863500e-1 * t7;
		double t9 = Math.Cos(q[1]);
		double t10 = 0.1358694235e1 * t9;
		double t11 = 0.418970917e1 + t8 + t10;
		double t12 = t4 * t11;
		double t13 = Math.Sin(q[0]);
		double t14 = t9 * t6;
		double t17 = 0.3770863500e-1 * t14 - 0.1358694235e1 * t5;
		double t18 = t13 * t17;
		double t19 = 0.3587298000e-1 * t7;
		double t20 = 0.1315599980e1 * t9;
		double t21 = 0.414769126e1 + t19 + t20;
		double t22 = t4 * t21;
		double t25 = 0.3587298000e-1 * t14 - 0.1315599980e1 * t5;
		double t26 = t13 * t25;
		double t27 = t12 - t18 + t22 - t26;
		double t29 = Math.Sin(q[10]);
		double t30 = t13 * t11;
		double t31 = t4 * t17;
		double t32 = t13 * t21;
		double t33 = t4 * t25;
		double t34 = t30 + t31 + t32 + t33;
		double t37 = Math.Cos(q[9]);
		double t38 = Math.Cos(q[11]);
		double t39 = t37 * t38;
		double t41 = Math.Sin(q[9]);
		double t42 = t29 * t2;
		double t45 = t42 * t27 + t1 * t34;
		double t48 = t41 * t38;
		double t54 = 0.3933776135e1 + 0.6298586244e-1 * t7 + 0.2289195848e1 * t9;
		double t60 = 0.160e0 * t30 + 0.160e0 * t31 - 0.160e0 * t32 - 0.160e0 * t33;
		double t68 = -0.160e0 * t12 + 0.160e0 * t18 + 0.160e0 * t22 - 0.160e0 * t26;
		double t74 = -t8 - t10;
		double t75 = t4 * t74;
		double t76 = -t13 * t17;
		double t77 = -t19 - t20;
		double t78 = t4 * t77;
		double t79 = -t13 * t25;
		double t80 = t75 - t76 + t78 - t79;
		double t82 = t13 * t74;
		double t83 = -t4 * t17;
		double t84 = t13 * t77;
		double t85 = -t4 * t25;
		double t86 = t82 + t83 + t84 + t85;
		double t92 = t42 * t80 + t1 * t86;
		double t100 = -0.9348052668e0 - 0.3149293122e-1 * t7 - 0.1144597924e1 * t9;
		double t106 = 0.160e0 * t82 + 0.160e0 * t83 - 0.160e0 * t84 - 0.160e0 * t85;
		double t114 = -0.160e0 * t75 + 0.160e0 * t76 + 0.160e0 * t78 - 0.160e0 * t79;
		double t120 = Math.Cos(q[2]);
		double t121 = Math.Cos(q[3]);
		double t122 = t120 * t121;
		double t125 = Math.Sin(q[2]);
		double t137 = 0.121304235e1 * t42 * t122 + 0.121304235e1 * t1 * t125 * t121;
		double t144 = t121 * t121;
		double t146 = Math.Sin(q[3]);
		double t147 = t146 * t146;
		double t150 = 0.4994459548e0 * t144 + 0.523e-2 * t147 - 0.5810472857e0 * t122;
		double t152 = t121 * t146;
		double t157 = 0.4942159548e0 * t152 * t125 + 0.2244128348e0 * t125 * t121;
		double t164 = -0.4942159548e0 * t152 * t120 - 0.2244128348e0 * t122;
		double t173 = t2 * t125 * t146;
		double t175 = -0.121304235e1 * t38 * t121 + 0.121304235e1 * t173;
		double t184 = t38 * t125 * t146;
		double t186 = -0.121304235e1 * t2 * t121 - 0.121304235e1 * t184;
		double t192 = -t29 * t175 + 0.121304235e1 * t1 * t120 * t146;
		double t203 = 0.4993659548e0 * t120 - 0.5810472857e0 * t121 + 0.2244128348e0 * t120 * t146;
		double t210 = 0.4993659548e0 * t125 + 0.2244128348e0 * t125 * t146;
		double t216 = Math.Cos(q[4]);
		double t217 = Math.Cos(q[5]);
		double t218 = t216 * t217;
		double t221 = Math.Sin(q[4]);
		double t233 = 0.125848136e1 * t42 * t218 + 0.125848136e1 * t1 * t221 * t217;
		double t240 = t217 * t217;
		double t242 = Math.Sin(q[5]);
		double t243 = t242 * t242;
		double t246 = 0.4937392784e0 * t240 + 0.548e-2 * t243 - 0.6028125714e0 * t218;
		double t248 = t217 * t242;
		double t253 = -0.4882592784e0 * t248 * t221 - 0.2328190516e0 * t221 * t217;
		double t260 = 0.4882592784e0 * t248 * t216 + 0.2328190516e0 * t218;
		double t269 = t2 * t221 * t242;
		double t271 = 0.125848136e1 * t38 * t217 + 0.125848136e1 * t269;
		double t280 = t38 * t221 * t242;
		double t282 = 0.125848136e1 * t2 * t217 - 0.125848136e1 * t280;
		double t288 = -t29 * t271 + 0.125848136e1 * t1 * t216 * t242;
		double t299 = -0.4935792784e0 * t216 + 0.6028125714e0 * t217 - 0.2328190516e0 * t216 * t242;
		double t306 = -0.4935792784e0 * t221 - 0.2328190516e0 * t221 * t242;
		M12[0] = -t3 * t27 + t29 * t34;
		M12[1] = t39 * t27 + t41 * t45;
		M12[2] = -t48 * t27 + t37 * t45;
		M12[3] = -t1 * (t38 * t54 - t2 * t60) - t29 * t68;
		M12[4] = t2 * t54 + t38 * t60;
		M12[5] = t68;
		M12[6] = -t3 * t80 + t29 * t86;
		M12[7] = t39 * t80 + t41 * t92;
		M12[8] = -t48 * t80 + t37 * t92;
		M12[9] = -t1 * (t38 * t100 - t2 * t106) - t29 * t114;
		M12[10] = t2 * t100 + t38 * t106;
		M12[11] = t114;
		M12[12] = -0.121304235e1 * t3 * t122 + 0.121304235e1 * t29 * t125 * t121;
		M12[13] = 0.121304235e1 * t39 * t122 + t41 * t137;
		M12[14] = -0.121304235e1 * t48 * t122 + t37 * t137;
		M12[15] = -t1 * (t38 * t150 - t2 * t157) - t29 * t164;
		M12[16] = t2 * t150 + t38 * t157;
		M12[17] = t164;
		M12[18] = t1 * t175 + 0.121304235e1 * t29 * t120 * t146;
		M12[19] = t37 * t186 + t41 * t192;
		M12[20] = -t41 * t186 + t37 * t192;
		M12[21] = -t1 * (0.5810472857e0 * t184 - t2 * t203) - t29 * t210;
		M12[22] = 0.5810472857e0 * t173 + t38 * t203;
		M12[23] = t210;
		M12[24] = -0.125848136e1 * t3 * t218 + 0.125848136e1 * t29 * t221 * t217;
		M12[25] = 0.125848136e1 * t39 * t218 + t41 * t233;
		M12[26] = -0.125848136e1 * t48 * t218 + t37 * t233;
		M12[27] = -t1 * (t38 * t246 - t2 * t253) - t29 * t260;
		M12[28] = t2 * t246 + t38 * t253;
		M12[29] = t260;
		M12[30] = t1 * t271 + 0.125848136e1 * t29 * t216 * t242;
		M12[31] = t37 * t282 + t41 * t288;
		M12[32] = -t41 * t282 + t37 * t288;
		M12[33] = -t1 * (0.6028125714e0 * t280 - t2 * t299) - t29 * t306;
		M12[34] = 0.6028125714e0 * t269 + t38 * t299;
		M12[35] = t306;

		double[,] result = new double[6,6];
		for (int i = 0; i < 6; i++)         // Colonne
			for (int j = 0; j < 6; j++)     // Rangée
				result[j, i] = M12[i * 6 + j];

		return result;
	}
}
