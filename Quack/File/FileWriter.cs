namespace Quack.File
{
	public class FileWriter : IFileWriter
	{
		public void WriteToFile(string input, string path, string filename)
		{
			System.IO.File.WriteAllText($"{path}/{filename}", input);
		}
	}
}