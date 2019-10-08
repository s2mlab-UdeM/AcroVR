using UnityEngine;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour les différents calculs mathématiques. </summary>

public class MathFunc
{
	// =================================================================================================================================================================
	/// <summary> Déterminer la valeur de la fonction spline cubique spécifiée, aux temps spécifiés (similaire à la fonction fnval de MatLab). </summary>

	public static float Fnval(float t, float[] breaks, float[,] coefs)
	{
		float[] value = Fnval(new float[1] { t }, breaks, coefs);
		return value[0];
	}

	public static float[] Fnval(float[] t, float[] breaks, float[,] coefs)
	{
		float[] value = new float[t.Length];

		for (int i = 0; i < t.Length; i++)
		{
			int j = 0;
			while (j < breaks.Length - 1 && t[i] > breaks[j]) j++;
			if (j > 0 && t[i] == breaks[j]) j--;

			float tt = t[i] - breaks[j];
			switch (coefs.GetUpperBound(1))
			{
				case 3:
					value[i] = coefs[j, 0] * tt * tt * tt + coefs[j, 1] * tt * tt + coefs[j, 2] * tt + coefs[j, 3];
					break;
				case 2:
					value[i] = coefs[j, 0] * tt * tt + coefs[j, 1] * tt + coefs[j, 2];
					break;
				case 1:
					value[i] = coefs[j, 0] * tt + coefs[j, 1];
					break;
				default:
					Debug.Log("Erreur MathFunc.Fnval: Matrice coefs doit contenir entre 2 et 4 colonnes");
					break;
			}

		}
		return value;
	}

	// =================================================================================================================================================================
	/// <summary> Déterminer la valeur, à un temps T, en utilisant une interpolation de forme Quintic. </summary>

	public static void Quintic(float t, float ti, float tj, float qi, float qj, out float p, out float v, out float a)
	{
		if (t < ti)
			t = ti;
		else if (t > tj)
			t = tj;
		float tp0 = tj - ti;
		float tp1 = t - ti;
		float tp2 = tp1 / tp0;
		float tp3 = tp2 * tp2;
		float tp4 = tp3 * tp2 * (6 * tp3 - 15 * tp2 + 10);
		float tp5 = qj - qi;
		float tp6 = tj - t;
		float tp7 = Mathf.Pow(tp0, 5);
		p = qi + tp5 * tp4;
		v = 30 * tp5 * tp1 * tp1 * tp6 * tp6 / tp7;
		a = 60 * tp5 * tp1 * tp6 * (tj + ti - 2 * t) / tp7;
	}

	// =================================================================================================================================================================
	/// <summary>
	/// <para> Vérification du signe d'un vecteur. </para>
	/// Retourne: -1 si vect inférieur à 0, 0 si vect égal 0, 1 si vect supérieur à 0
	/// </summary>

	public static int[] Sign(int[] vect)
	{
		int[] vectS = new int[vect.Length];
		for (int i = 0; i < vect.Length; i++)
		{
			if (vect[i] < 0) vectS[i] = -1;
			else if (vect[i] == 0) vectS[i] = 0;
			else vectS[i] = 1;
		}
		return vectS;
	}


	// =================================================================================================================================================================
	/// <summary> Copié le contenu d'une matrice dans une nouvelle matrice. </summary>

	public static float[] MatrixCopy(float[] matrix)
	{
		float[] newMatrix = new float[matrix.GetUpperBound(0) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			newMatrix[i] = matrix[i];
		return newMatrix;
	}

	public static float[] MatrixCopy(double[] matrix)
	{
		float[] newMatrix = new float[matrix.GetUpperBound(0) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			newMatrix[i] = (float)matrix[i];
		return newMatrix;
	}

	public static float[,] MatrixCopy(float[,] matrix)
	{
		float[,] newMatrix = new float[matrix.GetUpperBound(0) + 1, matrix.GetUpperBound(1) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			for (int j = 0; j <= matrix.GetUpperBound(1); j++)
				newMatrix[i, j] = matrix[i, j];
		return newMatrix;
	}

	public static float[,] MatrixCopy(double[,] matrix)
	{
		float[,] newMatrix = new float[matrix.GetUpperBound(0) + 1, matrix.GetUpperBound(1) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			for (int j = 0; j <= matrix.GetUpperBound(1); j++)
				newMatrix[i, j] = (float)matrix[i, j];
		return newMatrix;
	}

	// =================================================================================================================================================================
	/// <summary> Multiplication d'une matrice par une autre matrice. </summary>

	public static float[,] MatrixMultiply(float[,] Matrix1, float[,] Matrix2)
	{
		int r1 = Matrix1.GetLength(0);
		int c1 = Matrix1.GetLength(1);
		int r2 = Matrix2.GetLength(0);
		int c2 = Matrix2.GetLength(1);
		float[,] result = new float[r1, c2];

		if (c1 != r2)
			return null;

		for (int r = 0; r < r1; ++r)
			for (int c = 0; c < c2; ++c)
			{
				float s = 0;
				for (int z = 0; z < c1; ++z)
					s += Matrix1[r, z] * Matrix2[z, c];
				result[r, c] = s;
			}
		return result;
	}

	// =================================================================================================================================================================
	/// <summary> Extraire une rangée complète d'une matrice. </summary>

	public static float[] MatrixGetRow(float[,] matrix, int row)
	{
		float[] vector = new float[matrix.GetUpperBound(1) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(1); i++)
			vector[i] = matrix[row, i];
		return vector;
	}

	public static float[] MatrixGetRow(double[,] matrix, int row)
	{
		float[] vector = new float[matrix.GetUpperBound(1) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(1); i++)
			vector[i] = (float)matrix[row, i];
		return vector;
	}

	public static double[] MatrixGetRowD(float[,] matrix, int row)
	{
		double[] vector = new double[matrix.GetUpperBound(1) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(1); i++)
			vector[i] = matrix[row, i];
		return vector;
	}

	// =================================================================================================================================================================
	/// <summary> Extraire une colonne complète d'une matrice. </summary>

	public static float[] MatrixGetColumn(float[,] matrix, int column)
	{
		float[] vector = new float[matrix.GetUpperBound(0) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			vector[i] = matrix[i, column];
		return vector;
	}

	public static float[] MatrixGetColumn(double[,] matrix, int column)
	{
		float[] vector = new float[matrix.GetUpperBound(0) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			vector[i] = (float)matrix[i, column];
		return vector;
	}

	public static double[] MatrixGetColumnD(float[,] matrix, int column)
	{
		double[] vector = new double[matrix.GetUpperBound(0) + 1];
		for (int i = 0; i <= matrix.GetUpperBound(0); i++)
			vector[i] = matrix[i, column];
		return vector;
	}

	// =================================================================================================================================================================
	/// <summary> This is C source code for an implementation of one-dimensional phase unwrapping. It should produce results identical to the function in Matlab. </summary>
	// Ref: http://homepages.cae.wisc.edu/~brodskye/mr/phaseunwrap/

	public static float[] unwrap(float[] p)         // ported from matlab (Dec 2002)
	{
		int n = p.Length;
		float[] dp = new float[n];
		float[] dps = new float[n];
		float[] dp_corr = new float[n];
		float[] cumsum = new float[n];
		float[] pp = new float[n];
		float cutoff = Mathf.PI;               /* default value in matlab */
		int j;

		// incremental phase variation 
		// MATLAB: dp = diff(p, 1, 1);
		for (j = 0; j < n - 1; j++)
			dp[j] = p[j + 1] - p[j];

		// equivalent phase variation in [-pi, pi]
		// MATLAB: dps = mod(dp+dp,2*pi) - pi;
		for (j = 0; j < n - 1; j++)
			dps[j] = (dp[j] + Mathf.PI) - Mathf.Floor((dp[j] + Mathf.PI) / (2 * Mathf.PI)) * (2 * Mathf.PI) - Mathf.PI;

		// preserve variation sign for +pi vs. -pi
		// MATLAB: dps(dps==pi & dp>0,:) = pi;
		for (j = 0; j < n - 1; j++)
			if ((dps[j] == -Mathf.PI) && (dp[j] > 0))
				dps[j] = Mathf.PI;

		// incremental phase correction
		// MATLAB: dp_corr = dps - dp;
		for (j = 0; j < n - 1; j++)
			dp_corr[j] = dps[j] - dp[j];

		// Ignore correction when incremental variation is smaller than cutoff
		// MATLAB: dp_corr(abs(dp)<cutoff,:) = 0;
		for (j = 0; j < n - 1; j++)
			if (Mathf.Abs(dp[j]) < cutoff)
				dp_corr[j] = 0;

		// Find cumulative sum of deltas
		// MATLAB: cumsum = cumsum(dp_corr, 1);
		cumsum[0] = dp_corr[0];
		for (j = 1; j < n - 1; j++)
			cumsum[j] = cumsum[j - 1] + dp_corr[j];

		// Integrate corrections and add to P to produce smoothed phase values
		// MATLAB: p(2:m,:) = p(2:m,:) + cumsum(dp_corr,1);
		pp[0] = p[0];						// Ajouter par Marcel Beaulieu, pour que la fonction soit complètement équivalente à la fonction Matlab
		for (j = 1; j < n; j++)
			pp[j] = p[j] + cumsum[j - 1];

		return pp;
	}
}
