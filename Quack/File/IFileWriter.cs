namespace Quack.File
{
	public interface IFileWriter
	{
		void WriteToFile(string input, string path, string filename);
	}
}