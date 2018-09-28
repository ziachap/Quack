using System.Text.RegularExpressions;

namespace Quack.File
{
	public class SourceSanitizer : ISourceSanitizer
	{
		public string Sanitize(string input)
		{
			var output = input;
			output = RemoveNewLines(output);
			output = ReduceWhitespace(output);
			return output;
		}

		private static string RemoveNewLines(string input)
		{
			return Regex.Replace(input, @"\t|\n|\r", " ");
		}

		private static string ReduceWhitespace(string input)
		{
			RegexOptions options = RegexOptions.None;
			Regex regex = new Regex("[ ]{2,}", options);
			return regex.Replace(input, " ");
		}
	}
}