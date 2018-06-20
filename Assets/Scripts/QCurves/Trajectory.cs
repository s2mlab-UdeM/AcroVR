// =================================================================================================================================================================
/// <summary> Interpolation des angles pour chacune des articulations, selon la méthode d'interpolation utilisée (Quintic ou Spline cubique). </summary>

public class Trajectory
{
	public Trajectory(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float t, out float qd)
	{
		float qdotd;
		float qddotd;
		Trajectory trajectory = new Trajectory(lagrangianModel, t, out qd, out qdotd, out qddotd);
	}

	public Trajectory(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float t, out float qd, out float qdotd, out float qddotd)
	{
		int j = lagrangianModel.q2[0];
		int[] ni = lagrangianModel.q2;

		//	qd = zeros(ws.NDDL, 1);
		//	qdotd=qd;
		//    qddotd=qdotd;

		//% j
		//% Ni
		//    for i=Ni

		//        if isfield(SomersaultData.Nodes(i-j+1),'Real') && ...
		//                 ...strcmp(SomersaultData.Nodes(i-j+1).interpolation, 'Real') && ...
		//               ~isempty(SomersaultData.Nodes(i-j+1).Real) && Param.ExecReal
		//			Nodes = SomersaultData.Nodes(i - j + 1).Real;
		//        else	% AcroVR
		//% [i j i-j+1]
		//	Nodes=SomersaultData.Nodes(i-j+1);
		//        end

		//% Nodes.interpolation
		//        switch Nodes.interpolation
		//        case 'Quintic'
		//            [qd(i), qdotd(i), qddotd(i)]=DoMouv(t, Nodes);
		//        case 'Cubic spline'
		//%             fprintf('q(%d) en cubic spline\n', i);
		//	[qd(i), qdotd(i), qddotd(i)] = CubicSpline(t, Nodes, ws.tf);
		//        case 'Real'	% AcroVR
		//			[qd(i), qdotd(i), qddotd(i)] = RecalMouv(t, i);
		//	end
		//end
		//end

		//%===============================================================================================================================

		//function[p, v, a]=DoMouv(t, Data)

		//	i=2;
		//    n=size(Data.T,2);
		//    while (i<n && t> Data.T(i)), i=i+1; end
		// Data
		// [i t Data.T(i - 1) Data.T(i) Data.Q(i - 1) Data.Q(i)]

		//	[p, v, a]=Quintic(t, Data.T(i-1),Data.T(i),Data.Q(i-1),Data.Q(i));
		//end
		qd = 0;
		qdotd = 0;
		qddotd = 0;
	}
}
