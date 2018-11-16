// =================================================================================================================================================================
/// <summary> Interpolation des angles pour chacune des articulations, selon la méthode d'interpolation utilisée (Quintic ou Spline cubique). </summary>

public class GenerateQ0
{
	public GenerateQ0(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float tf, int qi, out float[] t0, out float[,] q0)
	{
		// Initialisation des DDL à traiter

		int[] ni;
		if (qi > 0)
			ni = new int[1] { qi };
		else
			ni = lagrangianModel.q2;

		// Initialisation des vecteurs contenant les temps et les positions des angles des articulations traités

		float[] qd;
		int n = (int)(tf / lagrangianModel.dt) + 1;
		t0 = new float[n];
		q0 = new float[lagrangianModel.nDDL, n];

		// Interpolation des positions des angles des articulations pour chaque intervalle de temps

		int i = 0;
		for (float interval = 0; interval <= tf; interval += lagrangianModel.dt)
		{
			t0[i] = interval;
			Trajectory trajectory = new Trajectory(lagrangianModel, interval, ni, out qd);
			trajectory.ToString();                  // Pour enlever un warning lors de la compilation
			for (int ddl = 0; ddl < qd.Length; ddl++)
				q0[ddl, i] = qd[ddl];
			i++;
		}
	}
}
