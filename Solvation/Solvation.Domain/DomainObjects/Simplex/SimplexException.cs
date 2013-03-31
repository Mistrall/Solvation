using System;

namespace Solvation.Domain.DomainObjects.Simplex
{
	public class SimplexException : ApplicationException
	{
		public SimplexException(string problemIsUnbounded) : base(problemIsUnbounded)
		{
		}

		public int Iteration { get; set; }
	}
}