namespace Quack.Lexer.TokenDefinitions
{
	public class AssignTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ASSIGN;

		public Token GetToken(string term) => new Token(TokenType.ASSIGN);
	}
}