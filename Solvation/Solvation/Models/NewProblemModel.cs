using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DataObjects.BasicStructures;
using Solvation.Annotations;

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

		public NewProblemModel(int resourceCount, int jobCount, IEnumerable<Resource> resources, IEnumerable<Job> jobs, IEnumerable<JobResourceDependency[]> dependencies)
		{
			ResourceCount = resourceCount;
			JobCount = jobCount;
			Resources= new BindingList<Resource>(resources.ToList());
			Jobs = new BindingList<Job>(jobs.ToList());
			var newDependencies = new JobResourceDependency[JobCount, ResourceCount];
			var values = new BindingList<double[]>();
			if (dependencies != null)
			{
				foreach (var jobResourceDependencyArr in dependencies)
				{
					var vals = new List<double>();
					vals.AddRange(jobResourceDependencyArr.Select(d=>d.Value));
					values.Add(vals.ToArray());
				}
			}
		}

		public int ResourceCount { get; private set; }
		public int JobCount { get; private set; }
		public BindingList<Resource> Resources { get; private set; }
		public BindingList<Job> Jobs { get; private set; }
		public IEnumerable<JobResourceDependency[]> Dependencies { get; private set; }
		public BindingList<double> DependencyValues { get; private set; }
	}
}
