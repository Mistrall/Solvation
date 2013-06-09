using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Solvation.Domain.DomainObjects.Simplex
{
	public class SimplexTuple
	{
		public ObjectiveFunctionType Type { get; set; }

		public DenseMatrix EqualityCoeffs { get; set; }
		public List<EquationType> EquationTypes { get; set; }
		public DenseVector FreeTerms { get; set; }
		public DenseVector ObjFuncCoeffs { get; set; }
		public double ObjFuncFreeTerm { get; set; }

		public SimplexTuple(ObjectiveFunctionType type, double[,] eqCoeffs, List<EquationType> equationTypes,
		                    double[] freeTerms, double[] objFuncCoeffs, double objFuncFreeTerm = 0)
		{
			Type = type;
			EqualityCoeffs = new DenseMatrix(eqCoeffs);
			EquationTypes = equationTypes;
			FreeTerms = new DenseVector(freeTerms);
			ObjFuncCoeffs = new DenseVector(objFuncCoeffs);
			ObjFuncFreeTerm = objFuncFreeTerm;
		}

		//Use this only for proper standart form
		public SimplexTuple(double[,] eqCoeffs, double[] freeTerms, double[] objFuncCoeffs, double objFuncFreeTerm)
			: this(ObjectiveFunctionType.Max, eqCoeffs, new List<EquationType>(), freeTerms, objFuncCoeffs, objFuncFreeTerm)
		{
			for (int i = 0; i < freeTerms.Length; i++)
			{
				EquationTypes.Add(EquationType.LessOrEqual);
			}
		}
	}

	public class StartingBasis
	{
		public int[] InequalityIndexes { get; set; }
		public double[] FeasibleBasis { get; set; }
	}
}