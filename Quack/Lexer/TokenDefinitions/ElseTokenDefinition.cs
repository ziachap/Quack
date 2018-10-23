namespace Quack.Lexer.TokenDefinitions
{
	public class ElseTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.ELSE;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.ELSE, info);
	}
}