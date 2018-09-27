using System.Text.RegularExpressions;

namespace Quack.Lexer.TokenDefinitions
{
	public class NumberTokenDefinition : ITokenDefinition
	{
		private const string NumberRegex = "^[0-9]+$";

		public bool IsMatch(string term) => Regex.Match(term, NumberRegex).Success;

		public Token GetToken(string term) => new Token(TokenType.NUMBER, term);
	}
}