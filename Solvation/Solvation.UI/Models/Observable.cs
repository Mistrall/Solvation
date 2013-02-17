using System.ComponentModel;

namespace Solvation.UI.Models
{
	public class Observable : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void Set<T>(ref T field, T value, string propertyName)
		{
			if (Equals(field, value)) return;
			field = value;
			OnPropertyChanged(propertyName);
		}
	}
}
