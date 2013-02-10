using System;
using System.Collections.Generic;
using System.Linq;
using Solvation.Domain.AlgorithmHelpers;
using Solvation.Domain.DomainObjects;
using Solvation.Domain.SpecialCollections;

namespace Solvation.Domain.Services
{
	public class JobPlanBuilder
	{
		private const double Epsilon = 0.00001;

		public List<PlanStep> GetBasePlan(IEnumerable<Resource> resources, IEnumerable<Job> jobs, IEnumerable<double[]> dependencyValues)
		{
			//Initialization
			var dataContainer = new SchedulingDataContainer(resources, jobs, dependencyValues);
			var plan = new List<PlanStep>();
			var unfinishedJobCount = dataContainer.Jobs.Count;
			var resArr = resources.ToArray();

			var resourcesForStep = new Resource[resArr.Length];

			//Repeat while we have unfinished jobs
			while (unfinishedJobCount>0)
			{
				//Set up resources per step
				for (int r = 0; r < resArr.Length; r++) resourcesForStep[r] = resArr[r].DeepCopy();
				
				//Select jobs we can plan
				var jobsPossibleToExecute = dataContainer.Jobs.Where(j => j.CanStart() && j.State!=JobState.Finished).ToList();
				//Put to heap with greed comparer
				var jobHeap = new Heap<Job>(jobsPossibleToExecute, jobsPossibleToExecute.Count(), new JobGreedyComparer());
				var jobsForStep = new List<RunningJob>();
				do
				{
					
					//Take first 
					var jobToExecute = jobHeap.PopRoot();
					//Check we can plan it
					//and compute intensity
					var maxIntensity = CalcMaxPossibleIntensity(jobToExecute, resourcesForStep);
					if (maxIntensity > 0)
					{
						//Start job
						jobToExecute.State=JobState.Started;
						//calc spended resources, remaining resources and time
						foreach (var resource in resourcesForStep)
						{
							var jobResourceDependency = jobToExecute.ResourceDependencies.FirstOrDefault(d => d.Resource == resource);
							if (jobResourceDependency != null)
								resource.Value -= jobResourceDependency.Value*maxIntensity;
						}

						var time = jobToExecute.FullWorkVolume/maxIntensity;
						jobsForStep.Add(new RunningJob(jobToExecute, maxIntensity, time));
					}

					//Repeat while we have some jobs
					//TODO: Optimize, maybe we don't need to check all jobs
				} while (jobHeap.Count>0);

				var stepTime = jobsForStep.Min(j => j.RunTime);
				var startTime = (plan.Count > 0) ? plan.Last().TimeEnd : 0;
				//Create PlanStep at this moment
				var step = new PlanStep(jobsForStep, startTime, stepTime + startTime);
				//Compute first job to finish
				var finishedJob = jobsForStep.FirstOrDefault(j => Math.Abs(j.RunTime - stepTime) < Epsilon);
				if (finishedJob != null) finishedJob.JobReference.State = JobState.Finished;
				//Memorize job progress
				plan.Add(step);
				unfinishedJobCount--;
			}
			

			return plan;
		}

		internal double CalcMaxPossibleIntensity(Job jobToExecute, IEnumerable<Resource> resourcesForStep)
		{
			var maxJobIntensity = Double.MaxValue;
			foreach (var resource in resourcesForStep)
			{
				var jobResourceDependency = jobToExecute.ResourceDependencies.FirstOrDefault(d => d.Resource == resource);
				if (jobResourceDependency == null) continue;
				var resourceDependencyValue = jobResourceDependency.Value;
				var upperbound = resourceDependencyValue*jobToExecute.MaximumIntensity;

				maxJobIntensity = Math.Min(maxJobIntensity, 
					(upperbound <= resource.Value ? jobToExecute.MaximumIntensity : (resource.Value / resourceDependencyValue)));
			}
			//Check if we are not below minimum intensity
			return maxJobIntensity < jobToExecute.MinimumIntensity ? 0 : maxJobIntensity;
		}
	}
}

