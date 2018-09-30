namespace Quack.Lexer.TokenDefinitions.BracesAndParentheses
{
	public class CloseParenthesisTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == ")";

		public Token GetToken(string term) => new Token(TokenType.CLOSE_PARENTHESES, term);
	}
}