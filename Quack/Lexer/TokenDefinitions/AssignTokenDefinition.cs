namespace Quack.Lexer.TokenDefinitions
{
	public class AssignTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ASSIGN;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.ASSIGN, info);
	}
}