using System.ComponentModel;
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

		public int ResourceCount { get; set; }
		public int JobCount { get; set; }
	}
}
