namespace Solvation.UI.Utils
{
	public interface ICanSaveContent<in T> where T:class
	{
		void SaveContent(T content, string filePath);
	}
}