using System;

namespace Solvation.Domain.DomainObjects.Simplex
{
	//Represents exception that thrown by Simplex algorithm
	public class SimplexException : ApplicationException
	{
		public SimplexException(string message) : base(message)
		{
		}

		public int Iteration { get; set; }
	}
}