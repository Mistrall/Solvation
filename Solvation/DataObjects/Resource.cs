﻿namespace DataObjects
{
	public class Resource
	{
		public int Number { get; set; }
		public double Value { get; set; }
		public Resource(int n, double v)
		{
			Number = n;
			Value = v;
		}
	}
}