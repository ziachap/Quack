using System.Text.RegularExpressions;

namespace Quack.Lexer.TokenDefinitions
{
	public class IdentifierTokenDefinition : ITokenDefinition
	{
		private const string NumberRegex = "^[a-zA-Z]+$";

		public bool IsMatch(string term) => Regex.Match(term, NumberRegex).Success;

		public Token GetToken(string term) => new Token(TokenType.IDENTIFIER, term);
	}
}