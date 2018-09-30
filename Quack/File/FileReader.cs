using System;
using System.IO;
using System.Text;

namespace Quack.File
{
	public class FileReader : IFileReader
	{
		public string LoadFromFile(string path)
		{
			Console.WriteLine("--- READER ---");
			return ReadFileToEnd(path);
		}

		private static string ReadFileToEnd(string path)
		{
			var stringBuilder = new StringBuilder();

			try
			{
				using (var sr = new StreamReader(path))
				{
					String line = sr.ReadToEnd();
					stringBuilder.Append(line);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The file could not be read:");
				Console.WriteLine(e.Message);
			}

			return stringBuilder.ToString();
		}
	}
}