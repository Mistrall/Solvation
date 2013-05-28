namespace Solvation.UI.Utils
{
	public interface ICanSaveContent
	{
		void SaveContent<T>(T content, string filePath) where T : class;
	}
}