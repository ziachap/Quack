namespace Quack.Lexer.TokenDefinitions.BracesAndParentheses
{
	public class OpenBracesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "{";

		public Token GetToken(string term) => new Token(TokenType.OPEN_BRACES, term);
	}
}