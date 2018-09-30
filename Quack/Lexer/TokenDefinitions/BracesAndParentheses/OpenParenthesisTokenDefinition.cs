namespace Quack.Lexer.TokenDefinitions.BracesAndParentheses
{
	public class OpenParenthesisTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) =>  term == "(";

		public Token GetToken(string term) => new Token(TokenType.OPEN_PARENTHESES, term);
	}
}
