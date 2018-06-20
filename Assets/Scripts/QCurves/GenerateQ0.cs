using UnityEngine;

// =================================================================================================================================================================
/// <summary> Interpolation des angles pour chacune des articulations, selon la méthode d'interpolation utilisée (Quintic ou Spline cubique). </summary>

public class GenerateQ0
{
	public GenerateQ0(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float tf, out float[] t0, out float[] q0)
	{
		float qd;
		int n = (int)(tf / lagrangianModel.dt) + 1;
		t0 = new float[n];
		q0 = new float[n];
		int i = 0;
		for (float interval = 0; interval <= tf; interval += lagrangianModel.dt)
		{
			t0[i] = interval;
			Trajectory trajectory = new Trajectory(lagrangianModel, interval, out qd);
			//q0(:, i) = q;
			i++;
		}

		//	TIME = 0:ws.dt:ws.tf;
		//	n = length(TIME);
		//		Q0 = zeros(ws.NDDL, n);
		//		for i = 1:n
		//			q = Trajectory(TIME(i));
		//			Q0(:, i) = q;
		//		end
		//Somersault.Q0 = Q0;
		//	Somersault.T0 = TIME;

		// System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("i = {0}, t0 = {1}{2}", i, t0[i], System.Environment.NewLine));
	}
}
