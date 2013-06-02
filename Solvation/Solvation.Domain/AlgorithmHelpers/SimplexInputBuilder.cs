using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.DomainObjects.Simplex;

namespace Solvation.Domain.AlgorithmHelpers
{
	public class SimplexInputBuilder
	{
		public SimplexTuple BuildFromBasePlan(List<PlanStep> basePlan, List<Job> jobs, List<Resource> resources,
		                                      List<double[]> dependencyValues)
		{
			int tVarCounter = basePlan.Count;
			int xVarCounter = basePlan.Sum(step => step.ExecutingJobs.Count);
			//count final variable number
			int totalVariables = tVarCounter + xVarCounter;
			//count final inequality number (for each variable 1 non-negative condition and 2*intencity restrictions)
			//also adding 2 eq per each stage -> total volumes for all stages should match initial total job volumes
			//also adding resource limitations per each stage
			int totalInequalities = totalVariables + 2*xVarCounter + (2 + resources.Count)*basePlan.Count;

			//initialize
			const ObjectiveFunctionType type = ObjectiveFunctionType.Min;
			var objFunctionCoeffs = new double[totalVariables];
			var eqTypes = new EquationType[totalInequalities];
			var eqCoeffs = new double[totalInequalities,totalVariables];
			var freeTerms = new double[totalInequalities];
			const double objFunctionFreeTerm = 0;
			//Fill help array
			var helpArrCnt = GenerateHelpDataArray(basePlan, xVarCounter);

			//Build objective function (to minimize total time -> coeffs for x's is zero) and non-negativity for t
			BuildNonNegativityInequalities(tVarCounter, xVarCounter, objFunctionCoeffs, eqCoeffs, eqTypes, freeTerms);

			int ineqCounter = totalVariables, xVarCnt1 = tVarCounter, xVarCnt2 = tVarCounter;
			for (int stepNumber = 0; stepNumber < basePlan.Count; stepNumber++)
			{
				//Create ineq for intensity part
				foreach (var executingJob in basePlan[stepNumber].ExecutingJobs)
				{
					//Add inequalities for intencity limitations
					//get min intensity for this job
					eqCoeffs[ineqCounter, stepNumber] = executingJob.JobReference.MinimumIntensity;
					eqCoeffs[ineqCounter, xVarCnt1] = -1;
					eqTypes[ineqCounter] = EquationType.LessOrEqual;
					freeTerms[ineqCounter++] = 0;

					//get max intensity for this job
					eqCoeffs[ineqCounter, stepNumber] = -1*executingJob.JobReference.MaximumIntensity;
					eqCoeffs[ineqCounter, xVarCnt1++] = 1;
					eqTypes[ineqCounter] = EquationType.LessOrEqual;
					freeTerms[ineqCounter++] = 0;
				}

				//Add inequalities for resource limitations
				var xVarCnt3 = xVarCnt2;
				foreach (var resource in resources)
				{
					eqCoeffs[ineqCounter, stepNumber] = -1*resource.Value;
					foreach (var runningJob in basePlan[stepNumber].ExecutingJobs)
					{
						eqCoeffs[ineqCounter, xVarCnt2++] = dependencyValues[runningJob.JobReference.Number - 1][resource.Number - 1];
					}
					eqTypes[ineqCounter] = EquationType.LessOrEqual;
					freeTerms[ineqCounter++] = 0;
					xVarCnt2 = xVarCnt3;
				}
				xVarCnt2 += basePlan[stepNumber].ExecutingJobs.Count;
			}

			//Add inequalities for total work volumes (should remain disregarding work steps)
			foreach (var job in jobs)
			{
				var indexList = new List<int>();
				for (int x = 0; x < helpArrCnt.Length; x++)
				{
					if (helpArrCnt[x] == job.Number) indexList.Add(x);
				}

				foreach (var i in indexList)
				{
					eqCoeffs[ineqCounter, tVarCounter + i] = 1;
				}

				eqTypes[ineqCounter] = EquationType.LessOrEqual;
				freeTerms[ineqCounter++] = job.FullWorkVolume;

				foreach (var i in indexList)
				{
					eqCoeffs[ineqCounter, tVarCounter + i] = -1;
				}
				
				eqTypes[ineqCounter] = EquationType.LessOrEqual;
				freeTerms[ineqCounter++] = -job.FullWorkVolume;
			}

			return new SimplexTuple(type, eqCoeffs, eqTypes.ToList(), freeTerms, objFunctionCoeffs, objFunctionFreeTerm);
		}

		internal void BuildNonNegativityInequalities(int tVarCounter, int xVarCounter, double[] objFunctionCoeffs,
		                                             double[,] eqCoeffs, EquationType[] eqTypes, double[] freeTerms)
		{
			for (int tnum = 0; tnum < tVarCounter; tnum++)
			{
				//Put non-negative coeffs to objective function
				objFunctionCoeffs[tnum] = 1;
				//Build eqCoeff matrix (A)
				//Add non-negativity to delta t
				eqCoeffs[tnum, tnum] = -1;
				eqTypes[tnum] = EquationType.LessOrEqual;
				freeTerms[tnum] = 0;
			}
			//non-negativity for x
			for (int xnum = 0; xnum < xVarCounter; xnum++)
			{
				eqCoeffs[tVarCounter + xnum, tVarCounter + xnum] = -1;
				eqTypes[tVarCounter + xnum] = EquationType.LessOrEqual;
				freeTerms[tVarCounter + xnum] = 0;
			}
		}

		public SimplexTuple ConvertToStandartForm(SimplexTuple tuple)
		{
			if (tuple.Type == ObjectiveFunctionType.Min)
			{
				tuple.ObjFuncCoeffs *= -1;
				tuple.ObjFuncFreeTerm *= -1;
				tuple.Type = ObjectiveFunctionType.Max;
			}
			int length = tuple.EquationTypes.Count;
			for (int i = 0; i < length; i++)
			{
				var equation = tuple.EquationTypes[i];
				if (equation == EquationType.MoreOrEqual)
				{
					tuple.EqualityCoeffs.SetRow(i, -1*tuple.EqualityCoeffs.Row(i));
					tuple.FreeTerms[i] *= -1;
					tuple.EquationTypes[i] = EquationType.LessOrEqual;
				}
				if (equation == EquationType.Equal)
				{
					tuple.EquationTypes[i] = EquationType.LessOrEqual;
					var additionalRow = tuple.EqualityCoeffs.Row(i).Clone();
					tuple.EqualityCoeffs = (DenseMatrix) tuple.EqualityCoeffs.InsertRow(i + 1, additionalRow);
					tuple.EquationTypes.Insert(i + 1, EquationType.MoreOrEqual);
					var val = tuple.FreeTerms[i];
					var freeTerms = tuple.FreeTerms.ToList();
					freeTerms.Insert(i + 1, val);
					tuple.FreeTerms = new DenseVector(freeTerms.ToArray());
					length++;
				}
			}

			return tuple;
		}

		internal int[] GenerateHelpDataArray(List<PlanStep> basePlan, int xVarCounter)
		{
			var deltaXHelpArr = new int[xVarCounter];
			int helpArrCnt = 0;
			foreach (var job in basePlan.SelectMany(planStep => planStep.ExecutingJobs))
			{
				deltaXHelpArr[helpArrCnt++] = job.JobReference.Number;
			}
			return deltaXHelpArr;
		}
	}
}