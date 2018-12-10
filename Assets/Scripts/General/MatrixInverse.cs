using System;

public class MatrixInverse
{
	// =================================================================================================================================================================
	// Ref: Test Run - Matrix Inversion Using C#
	//		By James McCaffrey | July 2016
	//
	//		https://msdn.microsoft.com/en-us/magazine/mt736457.aspx

	public static double[,] MtrxInverse(double[,] matrix)
	{
		// assumes determinant is not 0
		// that is, the matrix does have an inverse
		int n = matrix.GetUpperBound(0) + 1;
		double[,] result = MtrxCreate(n, n); // make a copy of matrix
		for (int i = 0; i < n; ++i)
			for (int j = 0; j < n; ++j)
				result[i, j] = matrix[i, j];

		double[,] lum; // combined lower & upper
		int[] perm;
		int toggle;
		toggle = MtrxDecompose(matrix, out lum, out perm);
		toggle.ToString();	// Pour éliminer un warning à la compilation

		double[] b = new double[n];
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
				if (i == perm[j])
					b[j] = 1.0;
				else
					b[j] = 0.0;

			double[] x = MtrxHelper(lum, b);
			for (int j = 0; j < n; ++j)
				result[j, i] = x[j];
		}
		return result;
	}

	// =================================================================================================================================================================

	static int MtrxDecompose(double[,] m, out double[,] lum, out int[] perm)
	{
		// Crout's LU decomposition for matrix determinant and inverse
		// stores combined lower & upper in lum[][]
		// stores row permuations into perm[]
		// returns +1 or -1 according to even or odd number of row permutations
		// lower gets dummy 1.0s on diagonal (0.0s above)
		// upper gets lum values on diagonal (0.0s below)

		int toggle = +1; // even (+1) or odd (-1) row permutatuions
		int n = m.GetUpperBound(0) + 1;

		// make a copy of m[][] into result lu[][]
		lum = MtrxCreate(n, n);
		for (int i = 0; i < n; ++i)
			for (int j = 0; j < n; ++j)
				lum[i, j] = m[i, j];


		// make perm[]
		perm = new int[n];
		for (int i = 0; i < n; ++i)
			perm[i] = i;

		for (int j = 0; j < n - 1; ++j) // process by column. note n-1 
		{
			double max = Math.Abs(lum[j, j]);
			int piv = j;

			for (int i = j + 1; i < n; ++i) // find pivot index
			{
				double xij = Math.Abs(lum[i, j]);
				if (xij > max)
				{
					max = xij;
					piv = i;
				}
			}

			if (piv != j)
			{
				double[] tmp = new double[lum.GetUpperBound(1) + 1]; // swap rows j, piv
				for (int i = 0; i < lum.GetUpperBound(1) + 1; i++)
					tmp[i] = lum[piv, i];
				for (int i = 0; i < lum.GetUpperBound(1) + 1; i++)
					lum[piv, i] = lum[j, i];
				for (int i = 0; i < lum.GetUpperBound(1) + 1; i++)
					lum[j, i] = tmp[i];

				int t = perm[piv]; // swap perm elements
				perm[piv] = perm[j];
				perm[j] = t;

				toggle = -toggle;
			}

			double xjj = lum[j, j];
			if (xjj != 0.0)
			{
				for (int i = j + 1; i < n; ++i)
				{
					double xij = lum[i, j] / xjj;
					lum[i, j] = xij;
					for (int k = j + 1; k < n; ++k)
						lum[i, k] -= xij * lum[j, k];
				}
			}

		}

		return toggle;
	}

	// =================================================================================================================================================================

	static double[] MtrxHelper(double[,] luMatrix, double[] b)
	{
		int n = luMatrix.GetUpperBound(0) + 1;
		double[] x = new double[n];
		b.CopyTo(x, 0);

		for (int i = 1; i < n; ++i)
		{
			double sum = x[i];
			for (int j = 0; j < i; ++j)
				sum -= luMatrix[i, j] * x[j];
			x[i] = sum;
		}

		x[n - 1] /= luMatrix[n - 1, n - 1];
		for (int i = n - 2; i >= 0; --i)
		{
			double sum = x[i];
			for (int j = i + 1; j < n; ++j)
				sum -= luMatrix[i, j] * x[j];
			x[i] = sum / luMatrix[i, i];
		}

		return x;
	}

	// =================================================================================================================================================================

	public static double MtrxDeterminant(double[,] matrix)
	{
		double[,] lum;
		int[] perm;
		int toggle = MtrxDecompose(matrix, out lum, out perm);
		double result = toggle;
		for (int i = 0; i < lum.GetUpperBound(0) + 1; ++i)
			result *= lum[i, i];
		return result;
	}

	// =================================================================================================================================================================

	public static double[,] MtrxCreate(int rows, int cols)
	{
		double[,] result = new double[rows, cols];
		return result;
	}

	// =================================================================================================================================================================

	public static double[,] MtrxProduct(double[] matrixA, double[,] matrixB)
	{
		double[,] matrix = new double[matrixA.Length, 1];
		for (int i = 0; i < matrixA.Length; i++)
			matrix[i, 0] = matrixA[i];
		return MtrxProduct(matrix, matrixB);
	}

	// =================================================================================================================================================================

	public static double[,] MtrxProduct(double[,] matrixA, double[] matrixB)
	{
		double[,] matrix = new double[matrixB.Length, 1];
		for (int i = 0; i < matrixB.Length; i++)
			matrix[i, 0] = matrixB[i];
		return MtrxProduct(matrixA, matrix);
	}

	// =================================================================================================================================================================

	public static double[,] MtrxProduct(double[,] matrixA, double[,] matrixB)
	{
		int aRows = matrixA.GetUpperBound(0) + 1;
		int aCols = matrixA.GetUpperBound(1) + 1;
		int bRows = matrixB.GetUpperBound(0) + 1;
		int bCols = matrixB.GetUpperBound(1) + 1;
		if (aCols != bRows)
			throw new Exception("Non-conformable matrices");

		double[,] result = MtrxCreate(aRows, bCols);

		for (int i = 0; i < aRows; ++i) // each row of A
			for (int j = 0; j < bCols; ++j) // each col of B
				for (int k = 0; k < aCols; ++k) // could use k < bRows
					result[i, j] += matrixA[i, k] * matrixB[k, j];

		return result;
	}

	// =================================================================================================================================================================

	public static string MtrxAsString(double[,] matrix)
	{
		string s = "";
		for (int i = 0; i < matrix.GetUpperBound(0) + 1; ++i)
		{
			for (int j = 0; j < matrix.GetUpperBound(1) + 1; ++j)
				s += matrix[i, j].ToString("F3").PadLeft(8) + " ";
			s += Environment.NewLine;
		}
		return s;
	}

	// =================================================================================================================================================================

	public static double[,] MtrxExtractLower(double[,] lum)
	{
		// lower part of an LU Doolittle decomposition (dummy 1.0s on diagonal, 0.0s above)
		int n = lum.GetUpperBound(0) + 1;
		double[,] result = MtrxCreate(n, n);
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
			{
				if (i == j)
					result[i, j] = 1.0;
				else if (i > j)
					result[i, j] = lum[i, j];
			}
		}
		return result;
	}

	// =================================================================================================================================================================

	public static double[,] MtrxExtractUpper(double[,] lum)
	{
		// upper part of an LU (lu values on diagional and above, 0.0s below)
		int n = lum.GetUpperBound(0) + 1;
		double[,] result = MtrxCreate(n, n);
		for (int i = 0; i < n; ++i)
		{
			for (int j = 0; j < n; ++j)
			{
				if (i <= j)
					result[i, j] = lum[i, j];
			}
		}
		return result;
	}
}
