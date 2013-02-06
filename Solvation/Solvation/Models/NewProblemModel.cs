﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Solvation.Annotations;
using Solvation.Domain.DomainObjects;

namespace Solvation.Models
{
	public class NewProblemModel: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
		public NewProblemModel()
		{
			Resources = new BindingList<Resource>();
			Jobs = new BindingList<Job>();
		}

		public NewProblemModel(int resourceCount, int jobCount, IEnumerable<Resource> resources, IEnumerable<Job> jobs)
			: this(resourceCount, jobCount, resources, jobs, null)
		{}

		public NewProblemModel(int resourceCount, int jobCount, IEnumerable<Resource> resources, IEnumerable<Job> jobs, IEnumerable<double[]> dependencies)
		{
			ResourceCount = resourceCount;
			JobCount = jobCount;
			Resources= new BindingList<Resource>(resources.ToList());
			Jobs = new BindingList<Job>(jobs.ToList());
			//var newDependencies = new JobResourceDependency[JobCount, ResourceCount];
			var values = GenerateDependencyValues(dependencies);

			DependencyValues = values;
		}

		private BindingList<double[]> GenerateDependencyValues(IEnumerable<double[]> dependencies)
		{
			var values = new BindingList<double[]>();

			for (int j = 0; j < JobCount; j++)
			{
				var vals = new List<double>();
				for (int r = 0; r < ResourceCount; r++)
					vals.Add(0);
				values.Add(vals.ToArray());
			}

			var deps = dependencies.ToArray();

			for (int i = 0; i < deps.Length; i++)
			{
				if (i < JobCount)
				{
					for (int j = 0; j < deps[i].Length; j++)
					{
						if (j < ResourceCount)
							values[i][j] = deps[i][j];
					}
				}
			}


			return values;
		}

		public int ResourceCount { get; private set; }
		public int JobCount { get; private set; }
		public BindingList<Resource> Resources { get; private set; }
		public BindingList<Job> Jobs { get; private set; }
		//public IEnumerable<JobResourceDependency[]> Dependencies { get; private set; }
		public BindingList<double[]> DependencyValues { get; private set; }
	}
}
