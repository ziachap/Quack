using System.Text.RegularExpressions;

namespace Quack.Lexer.TokenDefinitions
{
	public class NumberTokenDefinition : ITokenDefinition
	{
		private const string NumberRegex = "^[0-9]+$";

		public bool IsMatch(string term) => Regex.Match(term, NumberRegex).Success;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.NUMBER, info, term);
	}
}