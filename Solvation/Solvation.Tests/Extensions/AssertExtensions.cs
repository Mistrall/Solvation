using NUnit.Framework;
using Solvation.Domain.Extensions;

namespace Solvation.Tests.Extensions
{
	public static class AssertExtensions
	{
		public static bool AreFloatEqual(this Assert assert, double expected, double actual)
		{
			if (expected.FloatEquals(actual)) return true;
			throw new AssertionException(string.Format("Supplied values does not match. Expected {0}, Actual {1} ", expected, actual));
		}

		public static bool AreFloatEqual(this Assert assert, double[] expected, double[] actual)
		{
			bool res = true;
			for (int index = 0; index < expected.Length; index++)
			{
				var var = expected[index];
				if (!actual[index].FloatEquals(var)) res=false;
			}

			if (res) return true;
			throw new AssertionException(string.Format("Supplied array does not match. Expected {0}, Actual {1} ", expected, actual));
		}
	}
}
