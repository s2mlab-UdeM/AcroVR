using UnityEngine;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour les différents calculs mathématiques. </summary>

public class MathFunc
{
	// =================================================================================================================================================================
	/// <summary> Déterminer la valeur de la fonction spline cubique spécifiée, aux temps spécifiés (similaire à la fonction fnval de MatLab). </summary>

	public float Fnval(float t, float[] breaks, float[,] coefs)
	{
		float[] value = Fnval(new float[1] { t }, breaks, coefs);
		return value[0];
	}

	public float[] Fnval(float[] t, float[] breaks, float[,] coefs)
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

	public void Quintic(float t, float ti, float tj, float qi, float qj, out float p, out float v, out float a)
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

	public int[] Sign(int[] vect)
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
	/// <summary> Multiplication d'une matrice par une autre matrice. </summary>

	public float[,] MultiplyMatrix(float[,] Matrix1, float[,] Matrix2)
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
}
