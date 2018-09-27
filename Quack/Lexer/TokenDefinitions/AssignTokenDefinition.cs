namespace Quack.Lexer.TokenDefinitions
{
	public class AssignTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == "=";

		public Token GetToken(string term) => new Token(TokenType.ASSIGN);
	}
}