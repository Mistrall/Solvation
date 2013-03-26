using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Solvers.Iterative;
using Solvation.Domain.Extensions;

namespace Solvation.Domain.Services
{
	public class SimplexSolver
	{

		public Vector Solve(DenseMatrix A, DenseVector b, DenseVector c, int[] startingInequalities)
		{
			//Init simplex
			var m = A.RowCount;
			var n = A.ColumnCount;

			int iteration = 0;

			var AB = A.Extract(startingInequalities, Enumerable.Range(0, n).ToArray());
			var bB = b.Extract(startingInequalities);
			var solver = new CompositeSolver();
			var x = solver.Solve(AB, bB);

			return x;
		}
	}
}
