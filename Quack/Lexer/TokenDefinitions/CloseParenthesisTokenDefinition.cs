namespace Quack.Lexer.TokenDefinitions
{
	public class CloseParenthesisTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "(" || term == ")";

		public Token GetToken(string term) => new Token(TokenType.CLOSE_PARENTHESES, term);
	}
}