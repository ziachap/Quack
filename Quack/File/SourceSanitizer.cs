using System.Text.RegularExpressions;

namespace Quack.File
{
	public class SourceSanitizer : ISourceSanitizer
	{
		// TODO: Consider a scanner approach for better performance
		public string Sanitize(string input)
		{
			var output = input;
			output = PadArithmeticOperators(output);
			output = PadBooleanOperators(output);
			output = PadUnaryOperators(output);
			output = PadParentheses(output);
			output = PadBraces(output);
			output = PadStatementEnds(output);
			output = PadCommas(output);
			output = RemoveNewLines(output);
			output = ReduceWhitespace(output);
			output = TrimWhitespace(output);
			return output;
		}

		private static string PadUnaryOperators(string input)
		{
			var output = Regex.Replace(input, @"\!", " ! ");
			return output;
		}

		private static string PadArithmeticOperators(string input)
		{
			var output = Regex.Replace(input, @"\+", " + ");
			output = Regex.Replace(output, @"\-", " - ");
			output = Regex.Replace(output, @"\*", " * ");
			return Regex.Replace(output, @"\/", " / ");
		}

		private static string PadBooleanOperators(string input)
		{
			var output = Regex.Replace(input, @"\>!\=\>", " > ");
			output = Regex.Replace(output, @"\<", " < ");
			output = Regex.Replace(output, @"\>\=", " >= ");
			output = Regex.Replace(output, @"\<\=", " <= ");
			output = Regex.Replace(output, @"\!\=", " != ");
			return Regex.Replace(output, @"\=\=", " == ");
		}
		
		private static string PadBraces(string input)
		{
			var output = Regex.Replace(input, @"\{", " { ");
			return Regex.Replace(output, @"\}", " } ");
		}

		private static string PadParentheses(string input)
		{
			var output = Regex.Replace(input, @"\(", " ( ");
			return Regex.Replace(output, @"\)", " ) ");
		}

		private static string PadStatementEnds(string input)
		{
			return Regex.Replace(input, @";", " ; ");
		}

		private static string PadCommas(string input)
		{
			return Regex.Replace(input, @",", " , ");
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

		private static string TrimWhitespace(string input)
		{
			return input.Trim();
		}
	}
}