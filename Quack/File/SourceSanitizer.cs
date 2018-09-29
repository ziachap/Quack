using System.Text.RegularExpressions;

namespace Quack.File
{
	public class SourceSanitizer : ISourceSanitizer
	{
		public string Sanitize(string input)
		{
			var output = input;
			output = SeperateStatementEnds(output);
			output = RemoveNewLines(output);
			output = ReduceWhitespace(output);
			output = TrimWhiteSpace(output);
			return output;
		}

		private static string SeperateParentheses(string input)
		{
			var output = Regex.Replace(input, @"\(", " ( ");
			return Regex.Replace(output, @"\)", " ) ");
		}

		private static string SeperateStatementEnds(string input)
		{
			return Regex.Replace(input, @";", " ; ");
		}

		private static string RemoveNewLines(string input)
		{
			return Regex.Replace(input, @"\t|\n|\r", " ");
		}

		private static string ReduceWhitespace(string input)
		{
			var options = RegexOptions.None;
			var regex = new Regex("[ ]{2,}", options);
			return regex.Replace(input, " ");
		}

		private static string TrimWhiteSpace(string input)
		{
			return input.Trim();
		}
	}
}