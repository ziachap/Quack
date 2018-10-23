namespace Quack.Lexer.TokenDefinitions
{
	public class ReturnTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.RETURN;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.RETURN, info);
	}
}