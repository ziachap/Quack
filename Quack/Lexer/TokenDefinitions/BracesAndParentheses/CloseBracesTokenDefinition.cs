namespace Quack.Lexer.TokenDefinitions.BracesAndParentheses
{
	public class CloseBracesTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "}";

		public Token GetToken(string term) => new Token(TokenType.CLOSE_BRACES, term);
	}
}