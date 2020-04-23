// =================================================================================================================================================================
/// <summary> Conversion du modèle Humans à BioRBD ou vice-versa. </summary>

public class ConvertHumansBioRBD
{
	// =================================================================================================================================================================
	/// <summary> Conversion du modèle Humans à BioRBD. Correspondance des DDL entre les 2 modèles, via un fichier matlab. </summary>

	public static double[] Humans2Biorbd(double[] vecteurHumans)
	{
		int nDDL = MainParameters.c_nQ(MainParameters.Instance.ptr_model);
		int nDDLhumans = 12;
		double[] vecteurBiorbd = new double[nDDL * 2];

		vecteurBiorbd[0] = vecteurHumans[6];
		vecteurBiorbd[1] = vecteurHumans[7];
		vecteurBiorbd[2] = vecteurHumans[8];
		vecteurBiorbd[3] = -vecteurHumans[9];
		vecteurBiorbd[4] = vecteurHumans[10];
		vecteurBiorbd[5] = vecteurHumans[11];

		vecteurBiorbd[6] = vecteurHumans[0];
		vecteurBiorbd[7] = -vecteurHumans[1];
		vecteurBiorbd[8] = vecteurHumans[0];
		vecteurBiorbd[9] = -vecteurHumans[1];
		vecteurBiorbd[10] = vecteurHumans[2];
		vecteurBiorbd[11] = vecteurHumans[3];
		vecteurBiorbd[12] = vecteurHumans[4];
		vecteurBiorbd[13] = -vecteurHumans[5];

		vecteurBiorbd[0 + nDDL] = vecteurHumans[6 + nDDLhumans];
		vecteurBiorbd[1 + nDDL] = vecteurHumans[7 + nDDLhumans];
		vecteurBiorbd[2 + nDDL] = vecteurHumans[8 + nDDLhumans];
		vecteurBiorbd[3 + nDDL] = -vecteurHumans[9 + nDDLhumans];
		vecteurBiorbd[4 + nDDL] = vecteurHumans[10 + nDDLhumans];
		vecteurBiorbd[5 + nDDL] = vecteurHumans[11 + nDDLhumans];

		vecteurBiorbd[6 + nDDL] = vecteurHumans[0 + nDDLhumans];
		vecteurBiorbd[7 + nDDL] = -vecteurHumans[1 + nDDLhumans];
		vecteurBiorbd[8 + nDDL] = vecteurHumans[0 + nDDLhumans];
		vecteurBiorbd[9 + nDDL] = -vecteurHumans[1 + nDDLhumans];
		vecteurBiorbd[10 + nDDL] = vecteurHumans[2 + nDDLhumans];
		vecteurBiorbd[11 + nDDL] = vecteurHumans[3 + nDDLhumans];
		vecteurBiorbd[12 + nDDL] = vecteurHumans[4 + nDDLhumans];
		vecteurBiorbd[13 + nDDL] = -vecteurHumans[5 + nDDLhumans];
		return vecteurBiorbd;
	}

	// =================================================================================================================================================================
	/// <summary> Conversion du modèle BioRBD à Humans. Correspondance des DDL entre les 2 modèles, via un fichier matlab. </summary>

	public static double[] Biorbd2Humans(double[] vecteurBiorbd)
	{
		int nDDL = 12;
		int nDDLbiorbd = MainParameters.c_nQ(MainParameters.Instance.ptr_model);
		double[] vecteurHumans = new double[nDDL * 2];

		vecteurHumans[6] = vecteurBiorbd[0];
		vecteurHumans[7] = vecteurBiorbd[1];
		vecteurHumans[8] = vecteurBiorbd[2];
		vecteurHumans[9] = -vecteurBiorbd[3];
		vecteurHumans[10] = vecteurBiorbd[4];
		vecteurHumans[11] = vecteurBiorbd[5];

		vecteurHumans[0] = vecteurBiorbd[6];
		vecteurHumans[1] = -vecteurBiorbd[7];
		vecteurHumans[2] = vecteurBiorbd[10];
		vecteurHumans[3] = vecteurBiorbd[11];
		vecteurHumans[4] = vecteurBiorbd[12];
		vecteurHumans[5] = -vecteurBiorbd[13];

		vecteurHumans[6 + nDDL] = vecteurBiorbd[0 + nDDLbiorbd];
		vecteurHumans[7 + nDDL] = vecteurBiorbd[1 + nDDLbiorbd];
		vecteurHumans[8 + nDDL] = vecteurBiorbd[2 + nDDLbiorbd];
		vecteurHumans[9 + nDDL] = -vecteurBiorbd[3 + nDDLbiorbd];
		vecteurHumans[10 + nDDL] = vecteurBiorbd[4 + nDDLbiorbd];
		vecteurHumans[11 + nDDL] = vecteurBiorbd[5 + nDDLbiorbd];

		vecteurHumans[0 + nDDL] = vecteurBiorbd[6 + nDDLbiorbd];
		vecteurHumans[1 + nDDL] = -vecteurBiorbd[7 + nDDLbiorbd];
		vecteurHumans[2 + nDDL] = vecteurBiorbd[10 + nDDLbiorbd];
		vecteurHumans[3 + nDDL] = vecteurBiorbd[11 + nDDLbiorbd];
		vecteurHumans[4 + nDDL] = vecteurBiorbd[12 + nDDLbiorbd];
		vecteurHumans[5 + nDDL] = -vecteurBiorbd[13 + nDDLbiorbd];

		return vecteurHumans;
	}

	// =================================================================================================================================================================
	/// <summary> Conversion du modèle BioRBD à Humans (valeurs Q seulement). Correspondance des DDL entre les 2 modèles, via un fichier matlab. </summary>

	public static float[] qValuesHumans2Biorbd(float[] vecteurHumans)
	{
		int nDDL = MainParameters.c_nQ(MainParameters.Instance.ptr_model);
		float[] vecteurBiorbd = new float[nDDL];

		vecteurBiorbd[0] = vecteurHumans[6];
		vecteurBiorbd[1] = vecteurHumans[7];
		vecteurBiorbd[2] = vecteurHumans[8];
		vecteurBiorbd[3] = -vecteurHumans[9];
		vecteurBiorbd[4] = vecteurHumans[10];
		vecteurBiorbd[5] = vecteurHumans[11];

		vecteurBiorbd[6] = vecteurHumans[0];
		vecteurBiorbd[7] = -vecteurHumans[1];
		vecteurBiorbd[8] = vecteurHumans[0];
		vecteurBiorbd[9] = -vecteurHumans[1];
		vecteurBiorbd[10] = vecteurHumans[2];
		vecteurBiorbd[11] = vecteurHumans[3];
		vecteurBiorbd[12] = vecteurHumans[4];
		vecteurBiorbd[13] = -vecteurHumans[5];

		return vecteurBiorbd;
	}
}
