using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.DomainObjects.Simplex
{
	public class SimplexTuple
	{
		public ObjectiveFunctionType Type { get; set; }

		public DenseMatrix EqualityCoeffs { get; private set; }
		public EquationType[] EquationTypes { get; private set; }
		public DenseVector FreeTerms { get; private set; }
		public DenseVector ObjFuncCoeffs { get; private set; }
		public double ObjFuncFreeTerm { get; private set; }

		public SimplexTuple(ObjectiveFunctionType type, double[,] eqCoeffs, EquationType[] equationTypes, double[] freeTerms, double[] objFuncCoeffs, double objFuncFreeTerm)
		{
			Type = type;
			EqualityCoeffs = new DenseMatrix(eqCoeffs);
			EquationTypes = equationTypes;
			FreeTerms = new DenseVector(freeTerms);
			ObjFuncCoeffs = new DenseVector(objFuncCoeffs);
			ObjFuncFreeTerm = objFuncFreeTerm;
		}

		public SimplexTuple(double[,] eqCoeffs, double[] freeTerms, double[] objFuncCoeffs, double objFuncFreeTerm)
			: this(ObjectiveFunctionType.Max, eqCoeffs, new EquationType[freeTerms.Length], freeTerms, objFuncCoeffs, objFuncFreeTerm)
		{
		}
	}
}