using System;

namespace Quack.File
{
	public class FileWriter : IFileWriter
	{
		public void WriteToFile(string input, string path, string filename)
		{
			Console.WriteLine("--- WRITER ---");
			System.IO.File.WriteAllText($"{path}/{filename}", input);
		}
	}
}