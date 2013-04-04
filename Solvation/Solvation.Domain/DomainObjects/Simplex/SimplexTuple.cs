using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.DomainObjects.Simplex
{
	public class SimplexTuple
	{
		public DenseMatrix EqualityCoeffs { get; private set; }
		public DenseMatrix FreeTerms { get; private set; }
		public DenseMatrix ObjFuncCoeffs { get; private set; }
		public double ObjFuncFreeTerm { get; private set; }

		public SimplexTuple(double[,] eqCoeffs, double[,] freeTerms, double[,] objFuncCoeffs, double objFuncFreeTerm)
		{
			EqualityCoeffs = new DenseMatrix(eqCoeffs);
			FreeTerms = new DenseMatrix(freeTerms);
			ObjFuncCoeffs = new DenseMatrix(objFuncCoeffs);
			ObjFuncFreeTerm = objFuncFreeTerm;
		}
	}
}