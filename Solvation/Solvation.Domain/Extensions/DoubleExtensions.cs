using System;

namespace Solvation.Domain.Extensions
{
	public static class DoubleExtensions
	{
		public static bool FloatEquals(this double value, double otherValue)
		{
			return value.FloatEquals(otherValue, AlgorithmSettings.FloatPointPrecision);
		}

		public static bool FloatEquals(this double value, double otherValue, double precision)
		{
			return Math.Abs(value - otherValue) <= precision;
		}
	}
}
