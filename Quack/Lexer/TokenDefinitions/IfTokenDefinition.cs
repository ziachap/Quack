namespace Quack.Lexer.TokenDefinitions
{
	public class IfTokenDefinition : ITokenDefinition
	{
		public bool IsMatch(string term) => term == LanguageConstants.IF;

		public Token MakeToken(string term, DebugInfo info) => new Token(TokenType.IF, info);
	}
}